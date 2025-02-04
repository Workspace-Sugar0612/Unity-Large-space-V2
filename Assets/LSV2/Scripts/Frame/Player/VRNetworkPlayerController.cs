using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRNetworkPlayerController : NetworkBehaviour
{
    public Transform rHandTrans;
    public Transform lHandTrans;
    public Transform headTrans;

    public GameObject headModel;
    public GameObject lHandModel;
    public GameObject rHandModel;

    private VRPlayerRig m_VRPlayerRig;

    // 开启本地玩家
    public override void OnStartLocalPlayer()
    {
        m_VRPlayerRig = GameObject.FindObjectOfType<VRPlayerRig>();
        m_VRPlayerRig.vrNetPlayerControl = this;

        headModel.SetActive(false);
        lHandModel.SetActive(false);
        rHandModel.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
