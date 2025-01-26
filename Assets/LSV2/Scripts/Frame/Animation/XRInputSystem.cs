using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class XRInputSystem : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice m_TargetDevice;
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        TryInitialize();
    }

    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0 )
            m_TargetDevice = devices[0];
    }

    private void UpdateHandAnimation()
    {
        if (m_TargetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            m_Animator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            m_Animator.SetFloat("Trigger", 0);
        }

        if (m_TargetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            m_Animator.SetFloat("Grip", gripValue);
        }
        else
        {
            m_Animator.SetFloat("Grip", 0);
        }
    }

    private void Update()
    {
        if (!m_TargetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }
    }
}
