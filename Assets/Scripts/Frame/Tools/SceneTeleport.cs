using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : NetworkBehaviour
{
    [Tooltip("A UI control that shows how many people are interacting.")]
    public TMP_Text personCntText;

    /// <summary>
    /// Number of people who initiated the interaction
    /// </summary>
    [SyncVar(hook = nameof(PersonCountChanged))]
    private int m_PersonCount = 0;

    /// <summary>
    /// var m_PersonCount changed callback.
    /// </summary>
    /// <param name="_Old"></param>
    /// <param name="_New"></param>

    private VRSceneManager m_VRSceneManager;

    private void Awake()
    {
        if (m_VRSceneManager == null)
        {
            m_VRSceneManager = (VRSceneManager)FindObjectOfType(typeof(VRSceneManager));
        }
    }

    private void PersonCountChanged(int _Old, int _New)
    {
        personCntText.text = m_PersonCount.ToString();
    }

    [Command(requiresAuthority = false)]
    public void CmdSetPersonCount(int diff)
    {
        m_PersonCount += diff;
    }

    public void OnTriggerEnter(Collider other)//有碰撞体进入该物体时调用
    {
        if (other.CompareTag("Trigger"))//判断进入物体的碰撞体的Tag是Player
        {
            if (isServer)
            {
                CmdSetPersonCount(1);

                if (m_VRSceneManager != null)
                {
                    StartCoroutine(m_VRSceneManager.SwitchScene(m_PersonCount));
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)//有碰撞体退出该物体时调用
    {
        if (other.CompareTag("Trigger"))//判断进入物体的碰撞体的Tag是Player
        {
            if (isServer)
            {
                CmdSetPersonCount(-1);
            }
        }
    }
}
