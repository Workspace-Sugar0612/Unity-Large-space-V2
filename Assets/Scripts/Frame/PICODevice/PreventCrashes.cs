using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class PreventCrashes : MonoBehaviour
{
    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        // DontCrashInEditor();
    }

    public void DontCrashInEditor()
    {
        StartCoroutine(_DontCrashInEditor());
    }

    IEnumerator _DontCrashInEditor()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }

}