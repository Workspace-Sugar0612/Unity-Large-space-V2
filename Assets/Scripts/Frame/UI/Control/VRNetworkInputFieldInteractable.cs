using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRNetworkInputFieldInterable : NetworkBehaviour
{
    /// <summary>
    /// New inputfield text content.
    /// </summary>
    [SyncVar(hook = nameof(OnNewTextChanged))]
    private string m_NewText;

    /// <summary>
    /// InputField control.
    /// </summary>
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
