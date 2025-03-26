using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MyVRPlayerRig : NetworkBehaviour
{
    [Header("Model Transform")]

    [SerializeField]
    [Tooltip("model's lefthand tranform")]
    Transform m_LHand;
    /// <summary>
    /// model's lefthand tranform
    /// </summary>
    public Transform lHand
    {
        get => m_LHand;
        set => m_LHand = value;
    }

    [SerializeField]
    [Tooltip("model's righthand tranform")]
    Transform m_RHand;
    /// <summary>
    /// model's righthand tranform
    /// </summary>
    public Transform rHand
    {
        get => m_RHand;
        set => m_RHand = value;
    }

    [SerializeField]
    [Tooltip("model's headtranform")]
    Transform m_Head;
    /// <summary>
    /// model's headtranform
    /// </summary>
    public Transform head
    {
        get => m_Head;
        set => m_Head = value;
    }

    [Space]
    [Header("Controller")]

    [SerializeField]
    [Tooltip("Prefab model manager. Used to synchronize the transform of different parts of the model with the corresponding parts of the VR origin of the real scene.")]
    VRNetworkPlayerController m_VRPlayerController;
    /// <summary>
    /// Prefab model manager. 
    /// Used to synchronize the transform of different parts of the model with the corresponding parts of the VR origin of the real scene.
    /// </summary>
    public VRNetworkPlayerController vrPlayerController
    {
        get => m_VRPlayerController;
        set => m_VRPlayerController = value;
    }
    
    /// <summary> 进入场景/退出场景时的渐变效果 </summary>
    private VRScreenFade vrScreenFade;

    public void Awake()
    {
        if (vrScreenFade == null)
            vrScreenFade = (VRScreenFade)FindObjectOfType(typeof(VRScreenFade));
    }

    public void Start()
    {
        vrScreenFade.SetAlphaVar(1.0f, 0.0f);
        vrScreenFade.enabled = true;
    }

    private void PlayerModleSync()
    {
        if (vrPlayerController)
        {
            vrPlayerController.head.position = head.transform.position;
            vrPlayerController.head.rotation = head.transform.rotation;

            vrPlayerController.lHand.position = lHand.transform.position;
            vrPlayerController.lHand.rotation = lHand.transform.rotation;

            vrPlayerController.rHand.position = rHand.transform.position;
            vrPlayerController.rHand.rotation = rHand.transform.rotation;
        }
    }

    void Update()
    {
        PlayerModleSync();
    }
}
