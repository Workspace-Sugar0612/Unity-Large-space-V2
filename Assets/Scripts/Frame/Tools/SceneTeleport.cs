using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTeleport : NetworkBehaviour
{
    [Tooltip("A UI control that shows how many people are interacting.")]
    public TMP_Text personCntText;

    [Tooltip("teleport target location.")]
    public Transform teleportTarget;

    [Tooltip("Player")]
    public Transform player;

    /// <summary>
    /// Number of people who initiated the interaction
    /// </summary>
    [SyncVar(hook = nameof(PersonCountChanged))]
    private int m_PersonCount = 0;

    /// <summary> Animation Manager </summary>
    private AnimManager m_AnimManager;

    private VRSceneController m_VRSceneController;

    /// <summary> How long does it take to start the system tp to the target location. </summary>
    private float m_TeleportTime = 3.5f;

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
        Log.input($"m_PersonCount: {m_PersonCount}, MyVRStaticVariables.personCount: {MyVRStaticVariables.personCount}");
        if (MyVRStaticVariables.personCount != 0
            && m_PersonCount != 0
            && m_PersonCount == MyVRStaticVariables.personCount)
        {
            Log.input("Movement teleport");
            RpcSetPlayerParent(transform);
            //player.SetParent(transform);
            StartCoroutine(TeleportRise());
        }
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
        //Debug.Log($"other name: {other.name} and other tag: {other.tag}");
        if (other.CompareTag("Trigger"))//�жϽ����������ײ���Tag��Player
        {
            CmdSetPersonCount(1);
        }
    }

    public void OnTriggerExit(Collider other)//����ײ���˳�������ʱ����
    {
        if (other.CompareTag("Trigger"))//�жϽ����������ײ���Tag��Player
        {
            if (isServer)
            {
                RpcSetPlayerParent(null);
                CmdSetPersonCount(-1);
            }
        }
    }

    /// <summary>
    /// ������ҵ�ǰ�ĸ�����
    /// ��Ҫ������ҿ��Ը���ƽ̨һ���ƶ�
    /// </summary>
    /// <param name="parent"></param>
    [ClientRpc]
    public void RpcSetPlayerParent(Transform parent)
    {
        player.SetParent(parent);
    }

    /// <summary>
    /// ƽ̨�ƶ�
    /// </summary>
    /// <returns></returns>
    public IEnumerator TeleportRise()
    {
        yield return new WaitForSeconds(2.0f);
        transform.DOMove(teleportTarget.position, m_TeleportTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            player.SetParent(null);
            RpcTeleportScene();
        });
    }
}
