using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class VRNetworkInteractable : NetworkBehaviour
{
    private Rigidbody m_Rigidbody;
    private void Start()
    {
        if (m_Rigidbody == null) { m_Rigidbody = GetComponent<Rigidbody>(); } 
    }

    public void EventPick()
    {
        ResetInteractableVelocity();
        CmdPickup(connectionToClient);
    }

    [Command(requiresAuthority = false)]
    public void CmdPickup(NetworkConnectionToClient sender = null)
    {
        ResetInteractableVelocity();
        if (sender != netIdentity.connectionToClient)
        {
            netIdentity.RemoveClientAuthority();
            netIdentity.AssignClientAuthority(sender);
        }
    }

    private void ResetInteractableVelocity()
    {
        if (m_Rigidbody)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
