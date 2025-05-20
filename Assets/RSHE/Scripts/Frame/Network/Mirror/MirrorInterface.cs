using UnityEngine;
using Mirror;

public class MirrorInterface : NetworkBehaviour
{
    static MirrorInterface instance;

    public static MirrorInterface Get()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<MirrorInterface>();
            if (instance == null)
            {
                GameObject obj = new GameObject("MirrorInterface");
                instance = obj.AddComponent<MirrorInterface>();
            }
        }
        return instance;
    }

    public bool IsServer
    {
        get
        {
            return isServer;
        }
    }
}
