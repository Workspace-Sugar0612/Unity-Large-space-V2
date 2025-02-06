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

    #endregion

    public VRNetworkPlayerController vrPlayerController;

    private void Start()
    {
        okChangedNameButton.onClick.AddListener(delegate { OnValueChangedName(); });
    }

    public void OnValueChangedName()
    {
        VRStaticVariables.playerName = playerNameInput.text;
        vrPlayerController.CmdSetupName("Player: " + playerNameInput.text);
    }
}
