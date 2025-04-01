using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CNNetworkPlayerController : NetworkBehaviour
{
    [Header("Location's Transform")]

    [SerializeField]
    [Tooltip("Right Hand Transform")]
    public Transform rHand;

    [SerializeField]
    [Tooltip("Left Hand Transform")]
    public Transform lHand;

    [SerializeField]
    [Tooltip("Head Transform")]
    public Transform head;

    [Tooltip("Player Collider Transform")]
    public Transform m_PlayerCollider;

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
    private CNVRPlayerRig m_VRPlayerRig;

    /// <summary>
    /// Player Name ui component in Scene.
    /// </summary>
    public TMP_Text textPlayerName;

    /// <summary>
    /// Player name variable.
    /// </summary>
    [SyncVar(hook = nameof(OnNameChangedHook))]
    string playerName;

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
            m_VRPlayerRig = (CNVRPlayerRig)FindObjectOfType(typeof(CNVRPlayerRig));

        if (m_VRPlayerRig != null)
        {    
            m_VRPlayerRig.vrPlayerController = this;
        }
    }
}
