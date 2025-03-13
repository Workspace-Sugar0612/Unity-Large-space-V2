using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : NetworkBehaviour
{
    /// <summary> Cutscenes animator. </summary>
    [SerializeField]
    [Tooltip("Cutscenes animator.")]
    private Animator maskAnimator;

    /// <summary>
    /// Set mask animator's bool variable.
    /// </summary>
    private void SetMaskAnimator_Bool(string name, bool b)
    {
        maskAnimator.SetBool(name, b);
    }

    /// <summary>
    /// 过场动画中的结束动画播放。
    /// </summary>
    /// <param name="callback"> 动画播放结束后做的事情 </param>
    /// <returns></returns>
    private IEnumerator PlayMaskEndAnim(Action callback)
    {
        SetMaskAnimator_Bool("isEnd", true);
        SetMaskAnimator_Bool("isStart", false);
        float animDuration = maskAnimator.GetCurrentAnimatorStateInfo(0).length;
        
        yield return new WaitForSeconds(animDuration);
        callback();
    }

    public void PlayMaskStartAnim()
    {
        SetMaskAnimator_Bool("isStart", true);
        SetMaskAnimator_Bool("isEnd", false);
    }

    /// <summary>
    /// 通知客户端,过场动画中的结束动画播放。
    /// </summary>
    /// <param name="callback"> 动画播放结束后做的事情 </param>
    /// <returns></returns>
    [ClientRpc]
    public void RpcPlayMaskEndAnim()
    {
        StartCoroutine(PlayMaskEndAnim(() =>
        {
            VRSceneController m_VRSceneController = (VRSceneController)FindObjectOfType(typeof(VRSceneController));
            m_VRSceneController.SwitchScene(0.0f);
        }));
    }
}
