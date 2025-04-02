using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkMatch))]
public class CNPlayer : NetworkBehaviour
{
    public static CNPlayer localPlayer;

    /// <summary> 房间ID </summary>
    [SyncVar] public string matchID;

    [Tooltip("玩家进入房间的索引id")]
    [SyncVar] public int playerIndex;

    private NetworkMatch networkMatch;

    [Tooltip("玩家当前所在房间")]
    [SyncVar] public Match currentMatch;

    [Tooltip("玩家Lobby UI Prefab")]
    [SerializeField] GameObject playerLobbyUI;

    Guid netIDGuid;
    List<string> matchIDs = new List<string>();

    void Awake()
    {
        networkMatch = GetComponent<NetworkMatch>();
    }

    public override void OnStartServer()
    {
        // Log.cinput("red", $"OnStartServer: {matchID}");
        netIDGuid = matchID.ToGuid();
    }

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
            Log.input($"Spawning other player UI Prefab");
            playerLobbyUI = CNUILobby.instance.SpawnPlayerUIPrefab(this);
        }
    }

    public override void OnStopClient () {
        Log.cinput ("red", $"Client Stopped");
        ClientDisconnect ();
    }

    public override void OnStopServer () {
        Log.cinput ("red", $"Client Stopped on Server");
        ServerDisconnect ();
    }

    /// <summary>
    /// 创建房间行为
    /// </summary>
    /// <param name="_matchId"></param>
    public void HostGame()
    {
        string matchId = CNMatchMaker.GetRandomMatchID();
        CmdHostGame(matchId);
    }
    
    /// <summary>
    /// 向服务器请求创建房间
    /// </summary>
    /// <param name="_matchId"></param>
    [Command]
    public void CmdHostGame(string _matchId)
    {
        matchID = _matchId;
        if (CNMatchMaker.instance.HostGame(_matchId, this, out playerIndex))
        {
            Log.cinput("green", $"Game host successfully!");
            networkMatch.matchId = _matchId.ToGuid();
            TargetHostGame(true, _matchId, playerIndex);
        }
        else
        {
            Log.cinput("red", $"Game host failed!");
            TargetHostGame(false, _matchId, playerIndex);
        }
    }

    [TargetRpc]
    public void TargetHostGame(bool success, string _matchID, int _playerIndex)
    {
        playerIndex = _playerIndex;
        matchID = _matchID;
        Log.input($"Match ID : {matchID}");
        CNUILobby.instance.HostSuccess(success, _matchID);
    }

    /// <summary>
    /// 加入房间行为
    /// </summary>
    /// <param name="_inputID"></param>
    public void JoinGame(string _inputID)
    {
        CmdJoinGame(_inputID);
    }

    [Command]
    public void CmdJoinGame(string _matchID)
    {
        matchID = _matchID;
        if (CNMatchMaker.instance.JoinGame(_matchID, this, out playerIndex))
        {
            Log.cinput("green", $"Game Joined successfully");
            networkMatch.matchId = _matchID.ToGuid();
            TargetJoinGame(true, _matchID, playerIndex);

            if (isServer && playerLobbyUI != null)
            {
                playerLobbyUI.SetActive(true);
            }
        }
        else
        {
            Log.cinput("red", $"Joined failed");
            TargetJoinGame(false, _matchID, playerIndex);
        }
    }

    [TargetRpc]
    void TargetJoinGame(bool success, string _matchID, int _playerIndex)
    {
        playerIndex = _playerIndex;
        matchID = _matchID;
        Debug.Log($"MatchID: {matchID} == {_matchID}");
        CNUILobby.instance.JoinSuccess(success, _matchID);
    }

    /// <summary>
    /// 根据玩家人数更新
    /// </summary>
    /// <param name="playerCount"></param>
    [Server]
    public void PlayerCountUpdated(int playerCount)
    {
        TargetPlayerCountUpdated(playerCount);
    }

    /// <summary>
    /// 根据玩家人数判断是否显示开始游戏按钮
    /// </summary>
    /// <param name="playerCount"></param>
    [TargetRpc]
    void TargetPlayerCountUpdated(int playerCount)
    {
        if (playerCount > 1)
        {
            CNUILobby.instance.SetStartButtonActive(true);
        }
        else
        {
            CNUILobby.instance.SetStartButtonActive(false);
        }
    }

    /// <summary>
    /// 断开连接行为
    /// </summary>
    public void DisconnectGame()
    {
        CmdDisconnectGame();
    }

    /// <summary>
    /// 向服务器请求断开连接
    /// </summary>
    [Command]
    public void CmdDisconnectGame()
    {
        ServerDisconnect();
    }

    void ServerDisconnect () {
        CNMatchMaker.instance.PlayerDisconnected(this, matchID);
        RpcDisconnectGame();   
    }

    [ClientRpc]
    void RpcDisconnectGame()
    {
        ClientDisconnect();
    }

    void ClientDisconnect () 
    {
        networkMatch.matchId = netIDGuid; // 重置matchId
        if (playerLobbyUI != null)
        {
            if (!isServer)
            {
                Destroy(playerLobbyUI);
            }
            else
            {
                playerLobbyUI.SetActive(false);
            }
        }
    }


    /// <summary>
    /// 搜索房间行为
    /// </summary>
    public void SearchGame()
    {
        CmdSearchGame();
    }

    [Command]
    private void CmdSearchGame()
    {
        List<string> matchIDs;
        if (CNMatchMaker.instance.SearchGame(out matchIDs))
        {
            TargetSearchGame(true, matchIDs);
        }
    }

    [TargetRpc]
    void TargetSearchGame (bool success, List<string> _matchIDs)
    {
        matchIDs = _matchIDs;
        CNUILobby.instance.SearchGameSuccess(success, matchIDs);
    }

    /// <summary>
    /// 开始游戏行为
    /// </summary>
    public void BeginGame()
    {
        CmdBeginGame();
    }

    [Command]
    void CmdBeginGame()
    {
        CNMatchMaker.instance.BeginGame(matchID);
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    [TargetRpc]
    void TargetBeginGame () 
    {
        SceneManager.LoadSceneAsync("Game", new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });
        // SceneManager.GetSceneAt(index);
        // CNNetworkManager.singleton.ServerChangeScene("Game");
    }
}
