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

    public void OnTriggerEnter(Collider other)//有碰撞体进入该物体时调用
    {
        if (other.CompareTag("Trigger"))//判断进入物体的碰撞体的Tag是Player
        {
            Debug.Log($"tag perent name: {other.name}, and tag name: {other.tag}");
            m_PersonCount += 1;
        }
    }

    public void OnTriggerExit(Collider other)//有碰撞体退出该物体时调用
    {
        if (other.CompareTag("Trigger"))//判断进入物体的碰撞体的Tag是Player
        {
            m_PersonCount -= 1;
        }
    }
}
