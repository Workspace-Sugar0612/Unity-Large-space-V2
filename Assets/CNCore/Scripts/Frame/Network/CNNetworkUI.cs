using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Mirror.Discovery;
using System.Collections;

public class CNNetworkUI : MonoBehaviour
{
    // [Tooltip("Host Button")]
    // [SerializeField]
    // Button hostButton;

    // [Tooltip("Join Button")]
    // [SerializeField]
    // Button joinButton;

    NetworkManager networkManager;
    CNNetworkDiscovery cnNetworkDiscovery;

    void Start()
    {
        if (cnNetworkDiscovery == null) 
            cnNetworkDiscovery = (CNNetworkDiscovery)FindObjectOfType(typeof(CNNetworkDiscovery));
    }


    public void onHostButtonClick()
    {
        NetworkManager.singleton.StartHost();
        cnNetworkDiscovery.AdvertiseServer();
    }

    public void onJoinButtonClick()
    {
        Log.cinput("green", "Join Button Clicked");
        StartCoroutine(StartDiscovery()); //开始查找主机
    }

    IEnumerator StartDiscovery()
    {
        cnNetworkDiscovery.StartDiscovery(); 
        yield return new WaitForSeconds(2.0f);
    }

    /// <summary>
    /// 找到IP地址后，连接到主机
    /// </summary>
    /// <param name="response"></param>
    public void DiscoveryReturn(ServerResponse response)
    {
        Log.cinput("green", $"Connected to: {response.serverId}");
        cnNetworkDiscovery.StopDiscovery(); //停止查找主机
        NetworkManager.singleton.StartClient(response.uri);
    }
}