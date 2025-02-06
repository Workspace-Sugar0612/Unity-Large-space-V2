using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void OnValueChangedName()
    {
        VRStaticVariables.playerName = playerNameInput.text;
        vrPlayerController.CmdSetupName("Player: " + playerNameInput.text);
    }
}
