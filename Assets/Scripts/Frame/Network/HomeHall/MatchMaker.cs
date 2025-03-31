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

    [Tooltip("最大房间人数")]
    [SerializeField] int maxMatchPlayers = 3;

    public void Start()
    {
        m_Instance = this;
    }

    /// <summary>
    ///  创建房间主持游戏
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="player"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="_matchID"></param>
    /// <param name="_player"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public bool JoinGame(string _matchID, Player _player, out int playerIndex)
    {
        playerIndex = -1;
        if (matchIDs.Contains(_matchID))
        {
            for (int i = 0; i < matches.Count; ++i)
            {
                if (matches[i].matchID == _matchID)
                {
                    if (!matches[i].inMatch && !matches[i].matchFull)
                    {
                        matches[i].players.Add(_player);
                        _player.currentMatch = matches[i];
                        playerIndex = matches[i].players.Count;
                        matches[i].players[0].PlayerCountUpdated(matches[i].players.Count);
                        if (matches[i].players.Count == maxMatchPlayers)
                        {
                            matches[i].matchFull = true; // ???????
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

    /// <summary>
    /// 生成随机ID
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 退出房间
    /// </summary>
    /// <param name="player"></param>
    /// <param name="_matchID"></param>
    public void PlayerDisconnected (Player player, string _matchID)
    {
        for (int i = 0; i < matches.Count; ++i)
        {
            if (matches[i].matchID == _matchID)
            {
                int playerIndex = matches[i].players.IndexOf(player);
                if (matches[i].players.Count > playerIndex) matches[i].players.RemoveAt(playerIndex);
                Log.cinput("blue", $"Player disconnected from match {_matchID} | {matches[i].players.Count} players remaining");

                //如何房间人数为0，删除房间
                if (matches[i].players.Count == 0)
                {
                    Log.cinput("blue", "No more players in Match.");
                    matches.RemoveAt(i);
                    matchIDs.Remove(_matchID);
                }
                else
                {
                    Player.localPlayer.PlayerCountUpdated(matches[i].players.Count);
                }
                break;
            }
        }
    }
}

/// <summary> Match扩展 </summary>
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
    public bool inMatch;
    public bool matchFull;
    public List<Player> players = new List<Player>();
    public Match(string id, Player player)
    {
        matchID = id;
        inMatch = false;
        matchFull = false;
        players.Add(player);
    }

    public Match() { }
}