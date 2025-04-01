using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Basic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class CNUILobby : MonoBehaviour
{
    public static CNUILobby instance;

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

    [Space]
    [Header("Search")]
    [Tooltip("搜索界面")]
    [SerializeField] private Canvas searchCanvas;

    /// <summary> 是否正在搜索房间 </summary>
    bool searching; 

    /// <summary> 房间ID列表 </summary>
    List<string> matchIDs = new List<string>();

    [Space]
    [Header("MatchList")]
    [Tooltip("房间列表界面")]
    [SerializeField] Canvas matchListCanvas;

    [Tooltip("房间列表预制体")]
    [SerializeField] CNMatchItem matchItemTemp;

    [Tooltip("房间列表预制体父物体")]
    [SerializeField] Transform matchItemParent;

    /// <summary> matchitem列表 </summary>
    List<CNMatchItem> matchItems = new List<CNMatchItem>();

    private void Start()
    {
       instance = this;
    }

#region Host
    /// <summary>
    /// create public host.
    /// </summary>
    public void HostPublic()
    {
        lobbySelectables.ForEach(x => x.interactable = false);
        CNPlayer.localPlayer.HostGame();
    }

    /// <summary>
    /// 创建房间成功
    /// </summary>
    /// <param name="success"></param>
    /// <param name="matchID"></param>
    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            connectCanvas.enabled = false;
            lobbyCanvas.enabled = true;
            if (LobbyPlayerUI) Destroy(LobbyPlayerUI);
            LobbyPlayerUI = SpawnPlayerUIPrefab(CNPlayer.localPlayer);
            matchIDText.text = matchID;
        }
        else
        {
            lobbySelectables.ForEach(x => x.interactable = true);
        }
    }
#endregion

#region  Join
    /// <summary>
    /// 加入房间
    /// </summary>
    public void Join()
    {
        lobbySelectables.ForEach(x => x.interactable = false);
        CNPlayer.localPlayer.JoinGame(joinMatchID.text.ToUpper());
    }

    /// <summary>
    /// 生成房间玩家UI预制体
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public GameObject SpawnPlayerUIPrefab (CNPlayer player)
    {
        GameObject newUIPlayer = Instantiate(uiPlayerPrefab, uiPlayerParent);
        newUIPlayer.GetComponent<CNUIPlayer>().SetPlayer(player);
        newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);
        return newUIPlayer;
    }
    
    /// <summary>
    /// 加入成功
    /// </summary>
    /// <param name="success"></param>
    /// <param name="_matchID"></param>
    public void JoinSuccess(bool success, string _matchID)
    {
        if (success)
        {
            connectCanvas.enabled = false;
            lobbyCanvas.enabled = true;
            if (LobbyPlayerUI != null) Destroy(LobbyPlayerUI);
            LobbyPlayerUI = SpawnPlayerUIPrefab(CNPlayer.localPlayer);
            matchIDText.text = _matchID;
        }
        else
        {
            lobbySelectables.ForEach(x => x.interactable = true);
        }
    }
    /// <summary>
    /// 设置房间开始游戏按钮的Active
    /// </summary>
    /// <param name="active"></param>
    public void SetStartButtonActive(bool active)
    {
        startButton.gameObject.SetActive(active);
    }
#endregion


#region  Exit
    /// <summary>
    /// 退出房间
    /// </summary>
    public void DisconnectGame()
    {
        if (LobbyPlayerUI != null) Destroy(LobbyPlayerUI);
        CNPlayer.localPlayer.DisconnectGame();

        lobbyCanvas.enabled = false;
        connectCanvas.enabled = true;
        lobbySelectables.ForEach(x => x.interactable = true);
    }
#endregion

#region Search

    /// <summary>
    /// 搜索房间
    /// </summary>
    public void SearchGame()
    {
        StartCoroutine(Searching());
    }

    public IEnumerator Searching() 
    {
        connectCanvas.enabled = false;
        searchCanvas.enabled = true;
        searching = true;
        float searchInterval = 1;
        float currentTime = 1;

        while (searching) 
        {
            if (currentTime > 0) 
            {
                currentTime -= Time.deltaTime;
            }
            else 
            {
                currentTime = searchInterval;
                CNPlayer.localPlayer.SearchGame ();
            }
            yield return null;
        }
        searchCanvas.enabled = false;
    }

    public void SearchGameSuccess(bool success, List<string> _matchIDs)
    {
        if (success)
        {
            searching = false;
            matchIDs = _matchIDs;
            ShowMatchList(matchIDs);
        }
    }

    public void CancelSearchGame()
    {
        searching = false;
        connectCanvas.enabled = true;
        searchCanvas.enabled = false;
    }
#endregion


#region MatchList
    public void ShowMatchList(List<string> matchIDs)
    {
        foreach (var item in matchItems)
        {
            item.gameObject.SetActive(false);
            Destroy(item.gameObject);
        }

        matchListCanvas.enabled = true;
        foreach (string matchID in matchIDs)
        {
            CNMatchItem matchItem = Instantiate(matchItemTemp, matchItemParent);
            matchItem.SetMatchID(matchID);
            matchItem.SetButtonClickListener(() =>
            {
                CNPlayer.localPlayer.JoinGame(matchID);
                matchListCanvas.enabled = false;
            });
            matchItem.gameObject.SetActive(true);
            matchItems.Add(matchItem);
        }
    }
#endregion

#region StartGame
    public void BeginGame()
    {
        CNPlayer.localPlayer.BeginGame();
    }
#endregion


}
