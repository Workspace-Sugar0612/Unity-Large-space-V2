using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class TestSc : MonoBehaviour
{
    NetworkManager manager;

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        manager.StartHost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
