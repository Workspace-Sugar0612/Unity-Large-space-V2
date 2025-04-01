using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class CNNetworkLauncher : MonoBehaviour
{
    public bool alwaysAutoStart = false;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    private CNNetworkDiscovery _vrNetworkDiscovery;
    private Log _log;

    // Start is called before the first frame update
    void Start()
    {
        if (_vrNetworkDiscovery == null) 
            _vrNetworkDiscovery = (CNNetworkDiscovery)FindObjectOfType(typeof(CNNetworkDiscovery));

        if (alwaysAutoStart) 
            StartCoroutine(Waiter());

        // PXR_Manager.EnableVideoSeeThrough = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Waiter()
    {
        Log.input("Discovering servers..");
        discoveredServers.Clear();
        _vrNetworkDiscovery.StartDiscovery(); //开始查找主机
        // we have set this as 1.0 seconds, default discovery scan is 3 seconds, allows some time if host and client are started at same time
        yield return new WaitForSeconds(1.0f);

        if (discoveredServers == null || discoveredServers.Count <= 0) //没有查找到就创建主机
        {
            Log.input("No Servers found, starting as Host.");
            yield return new WaitForSeconds(1.0f);
            discoveredServers.Clear();
            // NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
            NetworkManager.singleton.StartHost();
            _vrNetworkDiscovery.AdvertiseServer(); // 服务器公开
        }
    }

    //当查找到主机 进行链接
    public void OnDiscoveredServer(ServerResponse response)
    {
        discoveredServers[response.serverId] = response;
        Connect(response);
    }

    //连接server
    void Connect(ServerResponse response)
    {
        Log.cinput("green", $"Connected to: {response.serverId}");

        _vrNetworkDiscovery.StopDiscovery();//停止查询
        NetworkManager.singleton.StartClient(response.uri); //启动客户端连接
    }
}
