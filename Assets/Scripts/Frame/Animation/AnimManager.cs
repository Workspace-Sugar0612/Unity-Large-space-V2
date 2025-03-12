using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : NetworkBehaviour
{
    /// <summary> Cutscenes animator. </summary>
    [Tooltip("Cutscenes animator.")]
    public Animator maskAnimator;

    /// <summary>
    /// Set mask animator's bool variable.
    /// </summary>
    [ClientRpc]
    public void SetMaskAnimator_Bool(string name, bool b)
    {
        maskAnimator.SetBool(name, b);
    }
}
