using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VRNetworkInputFieldInterable : NetworkBehaviour
{
    /// <summary>
    /// New inputfield text content.
    /// </summary>
    [SyncVar(hook = nameof(NewTextChanged))]
    private string m_SyncText;

    /// <summary>
    /// InputField control.
    /// </summary>
    public TMP_InputField playerNameInput;

    void Start()
    {
        playerNameInput.onValueChanged.AddListener(InputFieldValChanged);
    }

    private void NewTextChanged(string _old, string _new)
    {
        playerNameInput.text = m_SyncText;
    }

    private void InputFieldValChanged(string text)
    {
        CmdSyncText(text);
    }


    [Command(requiresAuthority = false)]
    private void CmdSyncText(string t)
    {
        m_SyncText = t;
    }
}
