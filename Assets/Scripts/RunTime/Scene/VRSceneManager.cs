using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRSceneManager : NetworkBehaviour
{
    [Header("Scene Name")]
    [Scene, Tooltip("Need teleport scene name.")]
    public string sceneName;

    [Command(requiresAuthority = false)]
    public void CmdSwitchScene(string sceneName)
    {
        Debug.Log($"Command Switch Scene Name:{sceneName}");
        RpcLoadScene(sceneName);
    }

    [ClientRpc]
    private void RpcLoadScene(string sceneName)
    {
        //StartCoroutine(LoadSceneAsync(sceneName));
        StartCoroutine(SwitchScene());
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

    [ServerCallback]
    public IEnumerator SwitchScene()
    {
        Debug.Log("======= SwitchScene =======");
        yield return new WaitForSeconds(2.0f);
        //SceneManager.LoadScene(sceneName);
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }
}
