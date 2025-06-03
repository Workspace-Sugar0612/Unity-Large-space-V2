using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class HandPosActionManager : MonoBehaviour
{
    private MyVRHUD m_MyVRHud;

    /// <summary> ĿǰHandPose������ </summary>
    private string m_CurrHandPoseName;

    public void Awake()
    {
        if (m_MyVRHud == null)
            m_MyVRHud = (MyVRHUD)FindObjectOfType(typeof(MyVRHUD));
    }

    /// <summary>
    /// �Զ�������ʶ��ִ��
    /// </summary>
    /// <param name="handPoseAction"></param>
    public void HandPosePerformed(string poseName)
    {
        m_CurrHandPoseName = poseName;
        m_MyVRHud.InputLog($"This is {m_CurrHandPoseName} hand pose! ");
    }

    /// <summary>
    /// �Զ��������˳��¼�
    /// </summary>
    public void EndHandPoseAction()
    {
        m_MyVRHud.InputLog($"{m_CurrHandPoseName} hand pose ended!");
    }
}
