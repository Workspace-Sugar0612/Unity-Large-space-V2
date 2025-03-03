using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class XRController : MonoBehaviour
{
    void Start()
    {
        PXR_Manager.EnableVideoSeeThrough = true;
    }

    void Update()
    {
        
    }

    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            PXR_Manager.EnableVideoSeeThrough = true;
        }
    }
}