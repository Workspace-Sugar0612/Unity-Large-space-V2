using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CNUIPlayer : MonoBehaviour
{
    [Tooltip("房间玩家UI")]
    [SerializeField] Text playerName;
    CNPlayer player;

    public void SetPlayer (CNPlayer player)
    {
        this.player = player;
        playerName.text = "Player " + player.playerIndex.ToString();
    }
}
