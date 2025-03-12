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

    /// <summary> Cutscenes Controller. </summary>
    private VRSceneController m_VRSceneController;

    /// <summary> Animation Manager </summary>
    private AnimManager m_AnimManager;

    private void Awake()
    {
        if (m_VRSceneController == null)
            m_VRSceneController = (VRSceneController)FindObjectOfType(typeof(VRSceneController));

        if (m_AnimManager == null)
            m_AnimManager = (AnimManager)FindObjectOfType(typeof(AnimManager));
    }

    /// <summary>
    /// var m_PersonCount changed callback.
    /// </summary>
    /// <param name="_Old"></param>
    /// <param name="_New"></param>
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
                if (m_PersonCount == MyVRStaticVariables.personCount)
                {
                    m_AnimManager.SetMaskAnimator_Bool("isEnd", true);
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
