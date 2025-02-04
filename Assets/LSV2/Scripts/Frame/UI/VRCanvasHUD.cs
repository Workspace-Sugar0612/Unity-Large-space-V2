using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.UI;

public class VRCanvasHUD : MonoBehaviour
{
    public bool alwaysAutoStart = false;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public VRNetworkDiscovery networkDiscovery;
    public Text infoText;
    public Button buttonAuto;

    // Start is called before the first frame update
    void Start()
    {
        if (networkDiscovery == null) networkDiscovery = GameObject.FindObjectOfType<VRNetworkDiscovery>();
        if (alwaysAutoStart) StartCoroutine(Waiter());
        if (buttonAuto != null) buttonAuto.onClick.AddListener(ButtonAuto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��ӡ״̬��Ϣ
    public void SetupInfoText(string _info)
    {
        if (infoText != null)
        {
            infoText.text = _info;
        }

    }
    public void ButtonAuto()
    {
        SetupInfoText("Auto Starting.");
        StartCoroutine(Waiter());
    }
    public IEnumerator Waiter()
    {

        SetupInfoText("Discovering servers..");
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery(); //��ʼ��������
        // we have set this as 3.1 seconds, default discovery scan is 3 seconds, allows some time if host and client are started at same time
        yield return new WaitForSeconds(3.1f);
        if (discoveredServers == null || discoveredServers.Count <= 0) //û�в��ҵ��ʹ�������
        {
            SetupInfoText("No Servers found, starting as Host.");
            yield return new WaitForSeconds(1.0f);
            discoveredServers.Clear();
            // NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }
    }

    //�����ҵ����� ��������
    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
        Connect(info);
    }
    //����server
    void Connect(ServerResponse info)
    {
        SetupInfoText("Connecting to: " + info.serverId);

        networkDiscovery.StopDiscovery();//ֹͣ��ѯ
        NetworkManager.singleton.StartClient(info.uri); //�����ͻ�������
    }
}
