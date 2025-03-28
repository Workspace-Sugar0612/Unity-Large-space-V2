using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player localPlayer;
    [SyncVar] private string matchID;

    [Tooltip("��ҽ��뷿����id")]
    [SyncVar] public int playerIndex;

    private NetworkMatch networkMatch;

    [Tooltip("��ǰ������ڷ���")]
    [SyncVar] public Match currentMatch;

    [Tooltip("���뷿�����ҵ�UI��־")]
    [SerializeField] GameObject playerLobbyUI;

    void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        networkMatch = GetComponent<NetworkMatch>();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="_matchId"></param>
    public void HostGame()
    {
        string matchId = MatchMaker.GetRandomMatchID();
        CmdHostGame(matchId);
    }
    
    [Command]
    public void CmdHostGame(string _matchId)
    {
        matchID = _matchId;
        if (MatchMaker.instance.HostGame(_matchId, this, out playerIndex))
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
        UILobby.instance.HostSuccess(success, _matchID);
    }

    /// <summary>
    /// ���뷿��
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
        if (MatchMaker.instance.JoinGame(_matchID, this, out playerIndex))
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
        UILobby.instance.JoinSuccess(success, _matchID);
    }

    /// <summary>
    /// ���������������Lobby��UI
    /// </summary>
    /// <param name="playerCount"></param>
    public void PlayerCountUpdated(int playerCount)
    {
        TargetPlayerCountUpdated(playerCount);
    }

    /// <summary>
    /// ����������������Ƿ���ʾ��ʼ��Ϸ��ť
    /// </summary>
    /// <param name="playerCount"></param>
    [TargetRpc]
    void TargetPlayerCountUpdated(int playerCount)
    {
        if (playerCount > 1)
        {
            UILobby.instance.SetStartButtonActive(true);
        }
        else
        {
            UILobby.instance.SetStartButtonActive(false);
        }
    }
}
