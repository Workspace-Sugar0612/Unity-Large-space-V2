using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class SpatialAnchors : MonoBehaviour
{
    private static SpatialAnchors instance;
    public static SpatialAnchors Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType(typeof(SpatialAnchors)) as SpatialAnchors;
            }
            return instance;
        }
    }

    private bool IsCreateAnchor = false;
    public Button CreateAnchorBtn;
    public Button LoadAnchorBtn;
    public Button ClearAnchorBtn;
    public GameObject MenuUI;

    [SerializeField]
    private InputActionReference rightGrip; //抓取键
    [SerializeField]
    private InputActionReference rPrimaryButton_A; //A键

    [SerializeField]
    private GameObject anchorPreview; //锚点预览效果
    [SerializeField]
    private AnchorCTR anchorPrefab; //锚点预制体
    [SerializeField]
    private XRRayInteractor interactor; //手柄射线
    private XRBaseInteractable hoverInteractable; //手柄射线对准的物体
    private AnchorCTR selectAnchorCtr; //选中操作的锚点
    private Dictionary<ulong, AnchorCTR> anchorList = new Dictionary<ulong, AnchorCTR>();//创建锚保存点集合
    private float maxDriftDelay = 0.5f;
    private float currrDriftDelay = 0f;

    private void Awake()
    {
        MenuUI.gameObject.SetActive(false);
        anchorPreview.gameObject.SetActive(false);
        CreateAnchorBtn.onClick.AddListener(OnClickCreateAnchorBtn);
        LoadAnchorBtn.onClick.AddListener(OnClickLoadAnchorBtn);
        ClearAnchorBtn.onClick.AddListener(OnClearAnchorBtn);
    }

    private void Start()
    {
        StartSpatialAnchorProvider();
    }

    private void OnEnable()
    {
        rightGrip.action.started += OnRightGripPressed;
        rightGrip.action.canceled += OnRightGripReleased;

        rPrimaryButton_A.action.started += OnRighPrimaryPressed;
        rPrimaryButton_A.action.canceled += OnRighPrimaryReleased;

        interactor.hoverEntered.AddListener(HoverEntered);
        interactor.hoverExited.AddListener(HoverExited);

        PXR_Manager.SpatialAnchorDataUpdated += SpatialAnchorDataUpdated; // 注册锚点数据更新事件
    }

    private void OnDisable()
    {

        rightGrip.action.started -= OnRightGripPressed;
        rightGrip.action.canceled -= OnRightGripReleased;

        rPrimaryButton_A.action.started -= OnRighPrimaryPressed;
        rPrimaryButton_A.action.canceled -= OnRighPrimaryReleased;

        interactor.hoverEntered.RemoveListener(HoverEntered);
        interactor.hoverExited.RemoveListener(HoverExited);

        PXR_Manager.SpatialAnchorDataUpdated -= SpatialAnchorDataUpdated; // 注销锚点数据更新事件

    }

    private void FixedUpdate()
    {
        HandleSpatialDrift();
    }

    //更新校准描点坐标
    private void HandleSpatialDrift()
    {
        if (anchorList.Count == 0)
            return;
        currrDriftDelay += Time.deltaTime;
        if (currrDriftDelay >= maxDriftDelay)
        {
            currrDriftDelay = 0;
            foreach (var handlePair in anchorList)
            {
                var handle = handlePair.Key;
                var anchorObj = handlePair.Value;
                if (handle == UInt64.MinValue)
                {
                    Debug.LogError("Handle is invalid");
                    continue;
                }

                var result = PXR_MixedReality.LocateAnchor(handle, out var position, out var rotation);
                if (result == PxrResult.SUCCESS)
                {
                    // 如果成功，更新当前对象的位置和旋转
                    anchorObj.transform.position = position;
                    anchorObj.transform.rotation = rotation;
                }
            }
        }
    }

    //点击加载锚点
    private void OnClickLoadAnchorBtn()
    {
        PressedLoadAllAnchors();
    }

    //点击创建锚点
    private void OnClickCreateAnchorBtn()
    {
        if (!IsCreateAnchor)//开启创建
        {
            CreateAnchorBtn.GetComponentInChildren<Text>().text = "Cancel create";
            anchorPreview.gameObject.SetActive(true);
        }
        else
        {
            CreateAnchorBtn.GetComponentInChildren<Text>().text = "Create anchor";
            anchorPreview.gameObject.SetActive(false);
        }
        IsCreateAnchor = !IsCreateAnchor;
    }

    //清空锚点
    private async void OnClearAnchorBtn()
    {

        foreach (var anchor in anchorList)
        {
            var result = await PXR_MixedReality.UnPersistSpatialAnchorAsync(anchor.Key);

            if (result == PxrResult.SUCCESS)
            {
                if (PXR_MixedReality.DestroyAnchor(anchor.Key) == PxrResult.SUCCESS)
                {
                    Destroy(anchor.Value.gameObject);
                }
            }
        }
        anchorList.Clear();
    }

    // 锚点数据更新事件
    private void SpatialAnchorDataUpdated()
    {
        PressedLoadAllAnchors(); // 加载所有锚点
    }

    private async void StartSpatialAnchorProvider()
    {
        var result = await PXR_MixedReality.StartSenseDataProvider(PxrSenseDataProviderType.SpatialAnchor); // 启动空间锚点感知数据提供者

        if (result == PxrResult.SUCCESS) // 成功启动
        {
            PressedLoadAllAnchors();
        }
        Debug.Log("StartSpatialAnchorProvider " + result.ToString());
    }

    //创建锚点
    private void CreateAnchor()
    {
        CreateSpatialAnchor(anchorPreview.transform);
    }

    //加载所有序列化锚点
    private async void PressedLoadAllAnchors()
    {
        var result = await PXR_MixedReality.QuerySpatialAnchorAsync(); // 查询所有空间锚点
        if (result.result == PxrResult.SUCCESS) // 成功查询
        {
            foreach (var key in result.anchorHandleList) // 遍历锚点句柄
            {
                if (!anchorList.ContainsKey(key)) // 如果锚点列表中不存在该锚点
                {
                    if (PXR_MixedReality.GetAnchorUuid(key, out Guid uuid) == PxrResult.SUCCESS)
                    {
                        AnchorCTR anchorObject = Instantiate(anchorPrefab);
                        anchorObject.OnInit(key, uuid);
                        anchorObject.ShowSaveIcon();
                        PXR_MixedReality.LocateAnchor(key, out var position, out var orientation);
                        anchorObject.transform.rotation = orientation;
                        anchorObject.transform.position = position;
                        anchorList.Add(key, anchorObject);
                    }
                }
            }
        }
    }

    // 异步创建空间锚点
    private async void CreateSpatialAnchor(Transform _transform)
    {
        var result = await PXR_MixedReality.CreateSpatialAnchorAsync(_transform.position, _transform.rotation); // 创建锚点

        if (result.result == PxrResult.SUCCESS) // 成功创建
        {
            Debug.Log("创建锚点成功 uuID " + result.uuid.ToString() + " anchorHandle " + result.anchorHandle.ToString());

            AnchorCTR anchorObject = Instantiate(anchorPrefab);
            anchorObject.OnInit(result.anchorHandle, result.uuid);
            anchorObject.transform.rotation = _transform.rotation;
            anchorObject.transform.position = _transform.position;
            anchorList.Add(result.anchorHandle, anchorObject);
        }
    }

    //射线进入
    private void HoverEntered(HoverEnterEventArgs arg0)
    {
        hoverInteractable = (XRBaseInteractable)arg0.interactableObject;
    }

    //射线退出
    private void HoverExited(HoverExitEventArgs arg0)
    {
        hoverInteractable = null;
    }

    //A键释放 关闭锚点菜单
    private void OnRighPrimaryReleased(InputAction.CallbackContext obj)
    {
        // Debug.Log("OnRighPrimaryReleased _ A");

        if (selectAnchorCtr != null)
        {
            selectAnchorCtr.SetUIMenuShow(false);
            selectAnchorCtr = null;
        }
    }

    //A键盘按下 创建锚点 或者打开锚点菜单
    private void OnRighPrimaryPressed(InputAction.CallbackContext obj)
    {
        //Debug.Log("OnRighPrimaryPressed _ A");
        if (anchorPreview.gameObject.activeSelf && IsCreateAnchor) //开启创建锚点功能
        {
            CreateAnchor();
        }
        if (hoverInteractable != null)
        {
            selectAnchorCtr = hoverInteractable.GetComponent<AnchorCTR>();
            if (selectAnchorCtr != null)
            {
                selectAnchorCtr.SetUIMenuShow();
            }
        }
    }

    //右键抓取 打开菜单
    private void OnRightGripPressed(InputAction.CallbackContext callback)
    {
        MenuUI.gameObject.SetActive(true);
    }

    //右键释放 关闭菜单
    private void OnRightGripReleased(InputAction.CallbackContext callback)
    {
        MenuUI.gameObject.SetActive(false);
    }

    //删除锚点
    public void DestroyAnchor(ulong anchorHandle)
    {
        if (anchorList.ContainsKey(anchorHandle))
        {
            Destroy(anchorList[anchorHandle].gameObject);
            anchorList.Remove(anchorHandle);
        }
    }
}
