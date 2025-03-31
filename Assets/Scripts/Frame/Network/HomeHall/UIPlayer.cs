using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [Tooltip("房间玩家UI")]
    [SerializeField] Text playerName;
    Player player;

    public void SetPlayer (Player player)
    {
        this.player = player;
        playerName.text = "Player " + player.playerIndex.ToString();
    }
}
