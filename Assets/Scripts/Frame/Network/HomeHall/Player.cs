using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private static Player m_Instance;

    public static Player instance
    {
        get => m_Instance;
    }

    private NetworkMatch networkMatch;
    [SyncVar] private string matchID;

    void Start()
    {
        if (isLocalPlayer)
        {
            m_Instance = this;
        }
        networkMatch = GetComponent<NetworkMatch>();
    }

    public void HostGame()
    {
        string matchId = MatchMaker.GetRandomMatchID();
        CmdHostGame(matchId);
    }

    [Command]
    public void CmdHostGame(string _matchId)
    {
        matchID = _matchId;
        if (MatchMaker.instance.HostGame(_matchId, gameObject))
        {
            Log.input($"<color = green> Game host successfully! </color>");
            networkMatch.matchId = _matchId.ToGuid();
            TargetHostGame(true, _matchId);
        }
        else
        {
            Log.input($"<color = red> Game host failed! </color>");
            TargetHostGame(false, _matchId);
        }
    }

    [TargetRpc]
    public void TargetHostGame(bool success, string _matchID)
    {
        Log.input($"Match ID : {matchID}");
        UILobby.instance.HostSuccess(success);
    }
}
