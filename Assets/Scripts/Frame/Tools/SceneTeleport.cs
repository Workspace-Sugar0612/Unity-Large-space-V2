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

    /// <summary> Animation Manager </summary>
    private AnimManager m_AnimManager;

    private VRSceneController m_VRSceneController;

    private void Awake()
    {
        if (m_AnimManager == null)
            m_AnimManager = (AnimManager)FindObjectOfType(typeof(AnimManager));

        if (m_VRSceneController == null)
            m_VRSceneController = (VRSceneController)FindObjectOfType(typeof(VRSceneController));
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

    [ClientRpc]
    private void RpcTeleportScene()
    {
        VRScreenFade m_ScreenFade = (VRScreenFade)FindObjectOfType(typeof(VRScreenFade));
        m_ScreenFade.SetAlphaVar(0.0f, 1.0f);
        m_ScreenFade.enabled = true;
        StartCoroutine(m_VRSceneController.SwitchScene(m_ScreenFade.gradientTime));
    }

    public void OnTriggerEnter(Collider other)//����ײ����������ʱ����
    {
        Debug.Log($"other name: {other.name} and other tag: {other.tag}");
        if (other.CompareTag("Trigger"))//�жϽ����������ײ���Tag��Player
        {
            if (isServer)
            {
                CmdSetPersonCount(1);
                if (m_PersonCount == MyVRStaticVariables.personCount)
                {
                    RpcTeleportScene();
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)//����ײ���˳�������ʱ����
    {
        if (other.CompareTag("Trigger"))//�жϽ����������ײ���Tag��Player
        {
            if (isServer)
            {
                CmdSetPersonCount(-1);
            }
        }
    }
}
