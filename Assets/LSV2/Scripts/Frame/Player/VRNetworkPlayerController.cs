using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private VRHUD m_VRHUD;

    [SyncVar(hook = nameof(OnNameChangedHook))]
    public string playerName;
    public TMP_Text textPlayerName;

    public void OnNameChangedHook(string _old, string _new)
    {
        if (textPlayerName != null) { textPlayerName.text = playerName; }
    }

    [Command]
    public void CmdSetupName(string _name)
    {
        playerName = _name;
    }

    // 开启本地玩家
    public override void OnStartLocalPlayer()
    {
        InitObject();

        // 自己看不到自己的镜像模型
        headModel.SetActive(false);
        lHandModel.SetActive(false);
        rHandModel.SetActive(false);

        if (VRStaticVariables.playerName != "") { CmdSetupName(VRStaticVariables.playerName + netId); }
        else { CmdSetupName("Player" + netId); }
    }

    public void InitObject()
    {
        m_VRPlayerRig = (VRPlayerRig)FindObjectOfType(typeof(VRPlayerRig));
        if (m_VRPlayerRig != null) { m_VRPlayerRig.vrPlayerController = this; }

        m_VRHUD = (VRHUD)FindObjectOfType(typeof(VRHUD));
        if (m_VRHUD != null) { m_VRHUD.vrPlayerController = this; }
    }


    public bool isMR = false;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isMR)
        {
            Transform root = GameObject.Find("ROOT").transform;
            if (root != null) 
            {
                transform.SetParent(root);
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
