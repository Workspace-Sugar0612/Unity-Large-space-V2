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


    /// <summary>
    /// Switch Scene.
    /// </summary>
    /// <param name="cnt"> current palyer count. </param>
    /// <returns></returns>
    public IEnumerator SwitchScene(int cnt)
    {
        Debug.Log($"======= SwitchScene =======: {cnt} - {MyVRStaticVariables.personCount}");
        yield return new WaitUntil(() => cnt == MyVRStaticVariables.personCount);

        if (isServer)
        {
            //SceneManager.LoadScene(sceneName);
            NetworkManager.singleton.ServerChangeScene(sceneName);
        }
    }
}
