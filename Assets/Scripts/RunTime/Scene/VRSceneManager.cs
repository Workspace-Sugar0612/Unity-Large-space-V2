using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRSceneManager : NetworkBehaviour
{
    /// <summary>
    /// Need teleport scene name.
    /// </summary>
    public string sceneName;

    [Command]
    public void CmdSwitchScene(string sceneName)
    {
        RpcLoadScene(sceneName);
    }

    [ClientRpc]
    private void RpcLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!ao.isDone)
        {
            // float progress = Mathf.Clamp01(ao.progress / 0.9f); // 进度标准化到0-1
            // EventCenter.GetInstance().EventTrigger("SceneProgress", progress);
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
}
