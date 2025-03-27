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

    public SyncList<Match> matches = new SyncList<Match>(); // bug..
    public SyncList<string> matchIDs = new SyncList<string>();

    public void Start()
    {
        m_Instance = this;
    }

    public bool HostGame(string hostId, GameObject player)
    {
        if (!matchIDs.Contains(hostId))
        {
            matches.Add(new Match(hostId, player));
            matchIDs.Add(hostId);
            Log.input("Match generated.");

            return true;
        }
        else
        {
            Log.input("match ID already exists.");
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
    public List<GameObject> players = new List<GameObject>();

    public Match(string id, GameObject player)
    {
        matchID = id;
        players.Add(player);
    }

    public Match() { }
}