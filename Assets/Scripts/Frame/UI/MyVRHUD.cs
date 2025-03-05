using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MyVRHUD : MonoBehaviour
{
    #region UI Variable

    //public TMP_InputField playerNameInput;
    //public Button okChangedNameButton;
    //public Button autoConnection;

    #endregion

    public VRNetworkPlayerController vrPlayerController;
    private MyNetworkLauncher _vrNetLaucher;

    /// <summary>
    /// AutoConnect network.
    /// If not find Server, then create one server.
    /// </summary>
    public bool isAutoConnect;

    private void Awake()
    {
        if (_vrNetLaucher == null)
            _vrNetLaucher = (MyNetworkLauncher)FindObjectOfType(typeof(MyNetworkLauncher));
    }

    private void Start()
    {
        OnConnection();
        //okChangedNameButton.onClick.AddListener(delegate { OnValueChangedName(); });
        //autoConnection.onClick.AddListener(delegate { OnConnection(); });
    }

    public void OnValueChangedName()
    {
        //MyVRStaticVariables.playerName = playerNameInput.text;
        //vrPlayerController.CmdSetupName("Player: " + playerNameInput.text);
    }

    public void OnConnection()
    {
        if (isAutoConnect == true)
            StartCoroutine(_vrNetLaucher.Waiter());
    }
}
