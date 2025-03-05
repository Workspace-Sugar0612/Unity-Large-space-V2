using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRNetworkInputFieldInterable : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnNewTextChanged))]
    public string m_NewText;

    public TMP_InputField playerNameInput;

    void Start()
    {
        playerNameInput.onValueChanged.AddListener((s) => { CmdInputFieldText(s); });
    }

    public void OnNewTextChanged(string _old, string _new)
    {
        playerNameInput.text = m_NewText;
    }

    public void CmdInputFieldText(string text)
    {
        m_NewText = text;
    }
}
