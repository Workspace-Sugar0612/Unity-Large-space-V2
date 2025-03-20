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
    /// 自定义手势识别在开始阶段
    /// </summary>
    /// <param name="handPoseAction"></param>
    public void StartHandPoseAction(PXR_HandPoseConfig handPoseAction)
    {
        m_CurrHandPoseName = handPoseAction.name;
        m_MyVRHud.InputLog($"Start Hand Pose: {m_CurrHandPoseName}");
    }

    /// <summary>
    /// 自定义手势持续事件
    /// </summary>
    /// <param name="handPoseAction"></param>
    /// <param name="single"></param>
    public void UpdateHandPoseAction(float msDuration)
    {
        //float duration = msDuration / 1000.0f;
        m_MyVRHud.InputLog($"Update Hand Pose: {m_CurrHandPoseName}, duration: {msDuration}");
    }

    /// <summary>
    /// 自定义手势退出事件
    /// </summary>
    /// <param name="handPoseAction"></param>
    public void EndHandPoseAction()
    {
        m_MyVRHud.InputLog($"End Hand Pose: {m_CurrHandPoseName}");
    }
}
