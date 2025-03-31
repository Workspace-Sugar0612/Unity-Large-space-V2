using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    private void Start()
    {
        if (!Application.isBatchMode)
        {
            Log.input(" ==== Client Build ====");
            //networkManager.StartClient();
        }
        else
        {
            Log.input(" ==== Server Build ====");
        }
    }

    public void HostPublic()
    {
        networkManager.StartHost();
    }

    public void JoinLocal ()
    {
        networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    }
}
