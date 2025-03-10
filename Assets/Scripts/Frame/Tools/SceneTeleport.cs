using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTeleport : NetworkBehaviour
{

    [Header("A UI control that shows how many people are interacting.")]
    public TMP_Text personCntText;

    /// <summary>
    /// Number of people who initiated the interaction
    /// </summary>
    [SyncVar(hook = nameof(PersonCountChanged))]
    private int m_PersonCount;

    private void Awake()
    {
        m_PersonCount = 0;
    }

    void Start()
    {
        
    }

    private void PersonCountChanged(int _Old, int _New)
    {
        Debug.Log("Person Count Changed.");
        personCntText.text = m_PersonCount.ToString();
    }

    public void OnTriggerEnter(Collider other)//����ײ����������ʱ����
    {
        if (other.CompareTag("Trigger"))//�жϽ����������ײ���Tag��Player
        {
            Debug.Log($"tag perent name: {other.name}, and tag name: {other.tag}");
            m_PersonCount += 1;
        }
    }

    public void OnTriggerExit(Collider other)//����ײ���˳�������ʱ����
    {
        if (other.CompareTag("Trigger"))//�жϽ����������ײ���Tag��Player
        {
            m_PersonCount -= 1;
        }
    }
}
