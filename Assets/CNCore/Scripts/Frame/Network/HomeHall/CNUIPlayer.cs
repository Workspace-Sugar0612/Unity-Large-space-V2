using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CNUIPlayer : NetworkBehaviour
{
    [Tooltip("房间玩家UI")]
    [SerializeField] Text playerName;
    CNPlayer player;

    public void SetPlayer (CNPlayer player)
    {
        this.player = player;
        playerName.text = "Player " + RandomID();
    }

    public string RandomID()
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
