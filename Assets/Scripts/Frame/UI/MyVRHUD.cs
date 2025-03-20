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

    /// <summary> Input player want name. </summary>
    public TMP_InputField playerNameInput;

    /// <summary> confirm change name. </summary>
    public Button okChangedNameButton;

    /// <summary> Show debug log in release version Game. </summary>
    public TMP_Text logText;

    #endregion

    public VRNetworkPlayerController vrPlayerController;
    private MyNetworkLauncher m_VrNetLaucher;

    private void Awake()
    {
        if (m_VrNetLaucher == null)
            m_VrNetLaucher = (MyNetworkLauncher)FindObjectOfType(typeof(MyNetworkLauncher));
    }

    private void Start()
    {
        OnConnection();
        okChangedNameButton.onClick.AddListener(delegate { OnValueChangedName(); });
    }

    public void OnValueChangedName()
    {
        MyVRStaticVariables.playerName = playerNameInput.text;
        vrPlayerController.CmdSetupName("Player: " + playerNameInput.text);
    }

    public void OnConnection()
    {
        StartCoroutine(m_VrNetLaucher.Waiter());
    }

    /// <summary>
    /// 显示新的内容在Log Text 控件上
    /// </summary>
    /// <param name="content"></param>
    public void InputLog(string content)
    {
       logText.text = content;
    }
}
