using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimController : MonoBehaviour
{
    public InputActionProperty pinchActionProperty;
    public InputActionProperty gripActionProperty;
    private InputAction m_PinchAction;
    private InputAction m_GripAction;
    private Animator m_Animator;

    void Start()
    {
        m_PinchAction = pinchActionProperty.action;
        m_GripAction = gripActionProperty.action;
        m_Animator = GetComponent<Animator>();  
    }

    void Update()
    {
        float triggerValue = m_PinchAction.ReadValue<float>();
        m_Animator.SetFloat("Trigger", triggerValue);

        float gripValue = m_GripAction.ReadValue<float>();
        m_Animator.SetFloat("Grip", gripValue);
    }
}
