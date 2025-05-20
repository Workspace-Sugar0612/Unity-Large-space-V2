using Mirror;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    static UIController instance;

    MyNetworkDiscovery networkDiscovery;

    [Header("Control")]

    [Tooltip("人数文本")]
    public TMP_Text personCountText;

    public static UIController Get()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UIController>();
            if (instance == null)
            {
                GameObject obj = new GameObject("UIController");
                instance = obj.AddComponent<UIController>();
            }
        }
        return instance;
    }

    void Awake()
    {
        networkDiscovery = FindObjectOfType<MyNetworkDiscovery>();
    }

    void Start()
    {
        if (!SwitchCameraController.Get().isTeacher)
            OnClickJoinButton();
    }

    public void OnClickHostButton()
    {
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    public void OnClickJoinButton()
    {
        Log.cinput("green", "Join Button Clicked");
        StartCoroutine(networkDiscovery.IEStartDiscovery()); //开始查找主机
    }

    public void ChangedpersonCountText(int personCount)
    {
        MyVRStaticVariables.personCount = personCount;
        personCountText.text = personCount.ToString();
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
