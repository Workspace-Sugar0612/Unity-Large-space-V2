using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class VRHUD : MonoBehaviour
{
    #region UI Variable

    public TMP_InputField playerNameInput;
    public Button okChangedNameButton;
    public Button autoConnection;

    #endregion

    public VRNetworkPlayerController vrPlayerController;
    private VRNetworkLauncher _vrNetLaucher;

    private void Awake()
    {
        if (_vrNetLaucher == null)
            _vrNetLaucher = (VRNetworkLauncher)FindObjectOfType(typeof(VRNetworkLauncher));
    }

    private void Start()
    {
        okChangedNameButton.onClick.AddListener(delegate { OnValueChangedName(); });
        autoConnection.onClick.AddListener(delegate { OnConnection(); });
    }

    public void OnValueChangedName()
    {
        VRStaticVariables.playerName = playerNameInput.text;
        vrPlayerController.CmdSetupName("Player: " + playerNameInput.text);
    }

    public void OnConnection()
    {
        StartCoroutine(_vrNetLaucher.Waiter());
    }
}
