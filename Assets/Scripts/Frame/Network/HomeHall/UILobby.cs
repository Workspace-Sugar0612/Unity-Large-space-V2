using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;

    [Header("Host Join")]
    [Tooltip("����UI����")]
    [SerializeField] Canvas connectCanvas;

    [Tooltip("��Ҫ�����ķ����")]
    [SerializeField] private TMP_InputField joinMatchID;

    [Tooltip("���뷿��")]
    [SerializeField] private Button joinButton;

    [Tooltip("��������")]
    [SerializeField] private Button hostButton;

    [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();

    [Space]
    [Header("Lobby")]
    [Tooltip("����UI����")]
    [SerializeField] private Canvas lobbyCanvas;

    [Tooltip("�������UI")]
    [SerializeField] GameObject uiPlayerPrefab;

    [Tooltip("���ɵķ������UI�ĸ�����Transform")]
    [SerializeField] Transform uiPlayerParent;

    [Tooltip("����ID")]
    [SerializeField] Text matchIDText;

    [Tooltip("�˳����䰴ť")]
    [SerializeField] Button exitButton;

    [Tooltip("��ʼ��Ϸ��ť")]
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
