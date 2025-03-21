using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class HandPosActionManager : MonoBehaviour
{
    private MyVRHUD m_MyVRHud;

    /// <summary> 目前HandPose的名字 </summary>
    private string m_CurrHandPoseName;

    public void Awake()
    {
        if (m_MyVRHud == null)
            m_MyVRHud = (MyVRHUD)FindObjectOfType(typeof(MyVRHUD));
    }

    /// <summary>
    /// 自定义手势识别执行
    /// </summary>
    /// <param name="handPoseAction"></param>
    public void HandPosePerformed(string poseName)
    {
        m_CurrHandPoseName = poseName;
        m_MyVRHud.InputLog($"检测识别到{m_CurrHandPoseName}手势! ");
    }

    /// <summary>
    /// 自定义手势退出事件
    /// </summary>
    public void EndHandPoseAction()
    {
        m_MyVRHud.InputLog($"{m_CurrHandPoseName}手势退出！");
    }
}
