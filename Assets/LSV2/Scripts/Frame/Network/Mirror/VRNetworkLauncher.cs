using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;

public class VRNetworkLauncher : MonoBehaviour
{
    public bool alwaysAutoStart = false;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    private VRNetworkDiscovery _vrNetworkDiscovery;
    private Log _log;

    // Start is called before the first frame update
    void Start()
    {
        if (_vrNetworkDiscovery == null) _vrNetworkDiscovery = (VRNetworkDiscovery)FindObjectOfType(typeof(VRNetworkDiscovery));
        if (alwaysAutoStart) StartCoroutine(Waiter());
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
        _vrNetworkDiscovery.StartDiscovery(); //��ʼ��������
        // we have set this as 3.1 seconds, default discovery scan is 3 seconds, allows some time if host and client are started at same time
        yield return new WaitForSeconds(3.1f);

        if (discoveredServers == null || discoveredServers.Count <= 0) //û�в��ҵ��ʹ�������
        {
            Log.input("No Servers found, starting as Host.");
            yield return new WaitForSeconds(1.0f);
            discoveredServers.Clear();
            // NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
            NetworkManager.singleton.StartHost();
            _vrNetworkDiscovery.AdvertiseServer(); // ����������
        }
    }

    //�����ҵ����� ��������
    public void OnDiscoveredServer(ServerResponse response)
    {
        discoveredServers[response.serverId] = response;
        Connect(response);
    }

    //����server
    void Connect(ServerResponse response)
    {
        Log.input("Connecting to: " + response.serverId);

        _vrNetworkDiscovery.StopDiscovery();//ֹͣ��ѯ
        NetworkManager.singleton.StartClient(response.uri); //�����ͻ�������
    }
}
