using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerRig : MonoBehaviour
{
    public Transform rHandTrans;
    public Transform lHandTrans;
    public Transform headTrans;
    public VRNetworkPlayerController vrPlayerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vrPlayerController)
        {
            vrPlayerController.headTrans.position = headTrans.transform.position;
            vrPlayerController.headTrans.rotation = headTrans.transform.rotation;

            vrPlayerController.lHandTrans.position = lHandTrans.transform.position;
            vrPlayerController.lHandTrans.rotation = lHandTrans.transform.rotation;

            vrPlayerController.rHandTrans.position = rHandTrans.transform.position;
            vrPlayerController.rHandTrans.rotation = rHandTrans.transform.rotation;
        }
    }
}
