using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerRig : MonoBehaviour
{
    public Transform rHandTrans;
    public Transform lHandTrans;
    public Transform headTrans;
    public VRNetworkPlayerController vrNetPlayerControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vrNetPlayerControl)
        {
            vrNetPlayerControl.headTrans.position = headTrans.transform.position;
            vrNetPlayerControl.headTrans.rotation = headTrans.transform.rotation;

            vrNetPlayerControl.lHandTrans.position = lHandTrans.transform.position;
            vrNetPlayerControl.lHandTrans.rotation = lHandTrans.transform.rotation;

            vrNetPlayerControl.rHandTrans.position = rHandTrans.transform.position;
            vrNetPlayerControl.rHandTrans.rotation = rHandTrans.transform.rotation;
        }
    }
}
