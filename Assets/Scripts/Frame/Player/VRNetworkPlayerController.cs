using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRNetworkPlayerController : NetworkBehaviour
{
    [Header("Location's Transform")]

    [SerializeField]
    [Tooltip("Right Hand Transform")]
    public Transform m_RHand;
    /// <summary>
    /// Right Hand Transform.
    /// </summary>
    public Transform rHand
    {
        get => m_RHand;
        set => m_RHand = value;
    }

    [SerializeField]
    [Tooltip("Left Hand Transform")]
    Transform m_LHand;
    /// <summary>
    /// Left Hand Transform.
    /// </summary>
    public Transform lHand
    {
        get => m_LHand;
        set => m_LHand = value;
    }

    [SerializeField]
    [Tooltip("Head Transform")]
    Transform m_Head;
    /// <summary>
    /// Head Transform.
    /// </summary>
    public Transform head
    {
        get => m_Head;
        set => m_Head = value;
    }

    [SerializeField]
    [Tooltip("Name Transform")]
    Transform m_UI;
    /// <summary>
    /// Name Transform.
    /// </summary>
    public Transform nameUI
    {
        get => m_UI;
        set => m_UI = value;
    }

    [Space]
    [Header("Model Prefab")]

    [SerializeField]
    [Tooltip("Player Head Model Component")]
    GameObject m_HeadModel;

    [SerializeField]
    [Tooltip("Player Left Hand Model Component")]
    GameObject m_LHandModel;

    [SerializeField]
    [Tooltip("Player Right Hand Model Component")]
    GameObject m_RHandModel;

    [Space]
    [Header("Other Contorller")]

    /// <summary>
    /// PlayerRig component on player in scene.
    /// </summary>
    private MyVRPlayerRig m_VRPlayerRig;

    /// <summary>
    /// VR ui component.
    /// </summary>
    private MyVRHUD m_VRHUD;

    /// <summary>
    /// Player Name ui component in Scene.
    /// </summary>
    public TMP_Text textPlayerName;

    /// <summary>
    /// Player name variable.
    /// </summary>
    [SyncVar(hook = nameof(OnNameChangedHook))]
    string playerName;

    [Space]
    [Header("Touch Object")]
    [Tooltip("The item held in the user's right hand")]
    public GameObject heldObjectTemp;

    /// <summary>
    /// heldObjectTemp's instance in scene.
    /// </summary>
    private GameObject m_HeldObject;

    /// <summary>
    /// The distance between the grasped object and the right hand.
    /// </summary>
    private float m_Dist;

    public void Awake()
    {
        m_Dist = 0.2f;
    }

    public void Start()
    {
        InitAndSpawn();
    }

    private void InitAndSpawn()
    {
        m_HeldObject = Instantiate(heldObjectTemp, this.transform);
    }

    private void ObjectFollowing()
    {
        if (m_HeldObject != null)
        {
            Vector3 PlayerPos = rHand.position;
            Vector3 targetPos = PlayerPos + rHand.forward * m_Dist;
            m_HeldObject.transform.position = Vector3.Lerp(m_HeldObject.transform.position, targetPos, Time.deltaTime * 10f);
            m_HeldObject.transform.rotation = rHand.rotation;
        }
    }


    public void OnNameChangedHook(string _old, string _new)
    {
        if (textPlayerName != null)
        {
            textPlayerName.text = playerName;
        }
    }

    /// <summary>
    /// To request server revise player name.
    /// </summary>
    /// <param name="_name"></param>
    [Command]
    public void CmdSetupName(string _name)
    {
        playerName = _name;
    }

    /// <summary>
    /// Enable local player.
    /// Let the player ignore his own model.
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        InitObject();
        m_HeadModel.SetActive(false);
        m_LHandModel.SetActive(false);
        m_RHandModel.SetActive(false);
        if (VRStaticVariables.playerName != "")
        {
            CmdSetupName(VRStaticVariables.playerName + netId);
        }
        else 
        {
            CmdSetupName("Player" + netId); 
        }
    }

    /// <summary>
    /// Init Controller.
    /// </summary>
    public void InitObject()
    {
        if (m_VRPlayerRig == null)
        {
            m_VRPlayerRig = (MyVRPlayerRig)FindObjectOfType(typeof(MyVRPlayerRig));
            m_VRPlayerRig.vrPlayerController = this;
        }
        
        if (m_VRHUD == null)
        {
            m_VRHUD = (MyVRHUD)FindObjectOfType(typeof(MyVRHUD));
            m_VRHUD.vrPlayerController = this;
        }
    }

    private void Update()
    {
        ObjectFollowing();
    }

    #region OnStartClient
    //public bool isMR = false;
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    if (isMR)
    //    {
    //        Transform root = GameObject.Find("ROOT").transform;
    //        if (root != null)
    //        {
    //            transform.SetParent(root);
    //            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    //        }
    //    }
    //}
    #endregion
}
