using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNAutoClient : MonoBehaviour
{
    // 用来连接网络
    // [SerializeField] NetworkManager networkManager;

    // 用来查找局域网IP
    [SerializeField] CNNetworkDiscovery cnNetworkDiscovery;

    private void Start()
    {
        if (!Application.isBatchMode)
        {
            Log.input(" ==== Client Build ====");
            StartCoroutine(Discovery());
        }
        else
        {
            Log.input(" ==== Server Build ====");
        }
    }

    /// <summary>
    /// 创建Host
    /// </summary>
    public void HostPublic()
    {
        NetworkManager.singleton.StartHost();
        cnNetworkDiscovery.AdvertiseServer();
    }

    /// <summary>
    /// 加入局域网
    /// </summary>
    public void JoinLocal ()
    {
        // networkManager.networkAddress = "localhost";
        // networkManager.StartClient();
        StartCoroutine(Discovery());
    }


    /// <summary>
    /// 查找连接网络的IP
    /// </summary>
    /// <returns></returns>
    public IEnumerator Discovery()
    {
        cnNetworkDiscovery.StartDiscovery();
        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// 查找到了IP，开始join
    /// </summary>
    /// <param name="response"></param>
    public void OnDiscoveredServer(ServerResponse response)
    {
        Log.cinput("green", $"Connected to: {response.serverId}");
        cnNetworkDiscovery.StopDiscovery();//停止查询
        NetworkManager.singleton.StartClient(response.uri); //启动客户端连接
    }
}
