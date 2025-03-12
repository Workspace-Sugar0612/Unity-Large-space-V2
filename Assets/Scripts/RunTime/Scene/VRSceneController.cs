using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRSceneController : NetworkBehaviour
{
    [Header("Scene Name")]
    [Scene, Tooltip("Need teleport scene name.")]
    public string sceneName;

    /// <summary>
    /// Switch Scene.
    /// </summary>
    /// <param name="cnt"> current palyer count. </param>
    /// <returns></returns>
    public void SwitchScene()
    {
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(sceneName);
        }
    }
}
