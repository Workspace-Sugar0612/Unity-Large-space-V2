using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;

    [Header("Host Join")]
    [Tooltip("连接UI界面")]
    [SerializeField] Canvas connectCanvas;

    [Tooltip("想要搜索的房间号")]
    [SerializeField] private TMP_InputField joinMatchID;

    [Tooltip("加入房间")]
    [SerializeField] private Button joinButton;

    [Tooltip("创建房间")]
    [SerializeField] private Button hostButton;

    [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();

    [Space]
    [Header("Lobby")]
    [Tooltip("房间UI界面")]
    [SerializeField] private Canvas lobbyCanvas;

    [Tooltip("房间玩家UI")]
    [SerializeField] GameObject uiPlayerPrefab;

    [Tooltip("生成的房间玩家UI的父物体Transform")]
    [SerializeField] Transform uiPlayerParent;

    [Tooltip("房间ID")]
    [SerializeField] Text matchIDText;

    [Tooltip("退出房间按钮")]
    [SerializeField] Button exitButton;

    [Tooltip("开始游戏按钮")]
    [SerializeField] Button startButton;

    GameObject LobbyPlayerUI; // Lobby palyer perfab ui.
    private void Start()
    {
       instance = this;
    }

    /// <summary>
    /// create public host.
    /// </summary>
    public void HostPublic()
    {
        lobbySelectables.ForEach(x => x.interactable = false);
        Player.localPlayer.HostGame();
    }

    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            connectCanvas.enabled = false;
            lobbyCanvas.enabled = true;
            if (LobbyPlayerUI) Destroy(LobbyPlayerUI);
            LobbyPlayerUI = SpawnPlayerUIPrefab(Player.localPlayer);
            matchIDText.text = matchID;
        }
        else
        {
            lobbySelectables.ForEach(x => x.interactable = true);
        }
    }

    public void Join()
    {
        lobbySelectables.ForEach(x => x.interactable = false);
        Player.localPlayer.JoinGame(joinMatchID.text.ToUpper());
    }

    public GameObject SpawnPlayerUIPrefab (Player player)
    {
        GameObject newUIPlayer = Instantiate(uiPlayerPrefab, uiPlayerParent);
        newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
        newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);
        return newUIPlayer;
    }

    public void JoinSuccess(bool success, string _matchID)
    {
        if (success)
        {
            connectCanvas.enabled = false;
            lobbyCanvas.enabled = true;
            if (LobbyPlayerUI != null) Destroy(LobbyPlayerUI);
            LobbyPlayerUI = SpawnPlayerUIPrefab(Player.localPlayer);
            matchIDText.text = _matchID;
        }
        else
        {
            lobbySelectables.ForEach(x => x.interactable = true);
        }
    }

    public void SetStartButtonActive(bool active)
    {
        startButton.gameObject.SetActive(active);
    }
}
