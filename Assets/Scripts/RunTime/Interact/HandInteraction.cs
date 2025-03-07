using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    [HideInInspector]
    public VRNetworkPlayerController vrNetworkPlayerController;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        if (vrNetworkPlayerController)
        {
            transform.position = vrNetworkPlayerController.rHand.position;
            transform.rotation = vrNetworkPlayerController.rHand.rotation;
        }
    }
}
