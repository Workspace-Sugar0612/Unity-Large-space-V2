using JetBrains.Annotations;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MatchMaker : NetworkBehaviour
{
    private static MatchMaker m_Instance;

    public static MatchMaker instance
    {
        get => m_Instance;
    }

    [Tooltip("房间列表")]
    public SyncList<Match> matches = new SyncList<Match>();

    [Tooltip("房间ID列表")]
    public SyncList<string> matchIDs = new SyncList<string>();

    [Tooltip("房间最大可容纳人数")]
    [SerializeField] int maxMatchPlayers = 3;

    public void Start()
    {
        m_Instance = this;
    }

    public bool HostGame(string hostId, Player player, out int playerIndex)
    {
        playerIndex = -1;
        if (!matchIDs.Contains(hostId))
        {
            Match match = new Match(hostId, player);
            matches.Add(match);
            matchIDs.Add(hostId);
            Log.cinput("green", "Match generated.");
            player.currentMatch = match;
            playerIndex = 1;
            return true;
        }
        else
        {
            Log.input("match ID already exists.");
            return false;
        }
    }

    public bool JoinGame(string _matchID, Player _player, out int playerIndex)
    {
        playerIndex = -1;
        if (matchIDs.Contains(_matchID))
        {
            foreach (Match match in matches)
            {
                if (match.matchID == _matchID)
                {
                    if (!match.inMatch && !match.matchFull)
                    {
                        match.players.Add(_player);
                        _player.currentMatch = match;
                        playerIndex = match.players.Count;
                        match.players[0].PlayerCountUpdated(match.players.Count);
                        if (match.players.Count == maxMatchPlayers)
                        {
                            match.matchFull = true; // 人数满员
                        }
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            Log.cinput("red", "Match ID does not exist");
            return false;
        }
    }

    public static string GetRandomMatchID()
    {
        string id = string.Empty;
        for (int i = 0; i < 5; ++i)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26)
            {
                id += (char)(random + 65);
            }
            else
            {
                id += (random - 26).ToString();
            }
        }
        Debug.Log($"Random ID: {id}");
        return id;
    }
}

public static class MatchExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);
        return new Guid(hashBytes);
    }
}

[Serializable]
public class Match
{
    public string matchID;
    public List<Player> players = new List<Player>();
    public bool inMatch;
    public bool matchFull;
    public Match(string id, Player player)
    {
        matchID = id;
        inMatch = false;
        matchFull = false;
        players.Add(player);
    }

    public Match() { }
}