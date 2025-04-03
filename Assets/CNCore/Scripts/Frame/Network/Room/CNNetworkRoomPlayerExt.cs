using UnityEngine;
using Mirror;

public class CNNetworkRoomPlayerExt : NetworkRoomPlayer
{
    public override void Start()
    {
        base.Start();
        if (isServer)
        {
            Log.cinput("green", $"Is Server");
            CNRoomUI.singleton.SetStartGameButtonActive(true);
        }
        else
        {
            Log.cinput("green", $"Is Client");
            CNRoomUI.singleton.SetStartGameButtonActive(false);
        }
    }


    public override void OnStartClient()
    {
        //Debug.Log($"OnStartClient {gameObject}");
    }

    public override void OnClientEnterRoom()
    {
        //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
    }

    public override void OnClientExitRoom()
    {
        //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        //Debug.Log($"IndexChanged {newIndex}");
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        //Debug.Log($"ReadyStateChanged {newReadyState}");
    }

    public override void OnGUI()
    {
        //base.OnGUI();
    }
}