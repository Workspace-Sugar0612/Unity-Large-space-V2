using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnSelectSetParent : MonoBehaviour
{
    private XRBaseInteractable m_Interactable;
    private Transform m_OriginalSceneParent;
    public bool isMR = true;

    private void Awake()
    {
        if (isMR)
        {
            m_Interactable = transform.GetComponent<XRGrabInteractable>();
            m_Interactable.selectEntered.AddListener(OnSelectEntered);
            m_OriginalSceneParent = transform.parent;
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs arg0)
    {
        this.transform.parent = m_OriginalSceneParent;
    }
}
