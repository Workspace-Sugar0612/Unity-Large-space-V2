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
    private InputActionReference rightGrip; //ץȡ��
    [SerializeField]
    private InputActionReference rPrimaryButton_A; //A��

    [SerializeField]
    private GameObject anchorPreview; //ê��Ԥ��Ч��
    [SerializeField]
    private AnchorCTR anchorPrefab; //ê��Ԥ����
    [SerializeField]
    private XRRayInteractor interactor; //�ֱ�����
    private XRBaseInteractable hoverInteractable; //�ֱ����߶�׼������
    private AnchorCTR selectAnchorCtr; //ѡ�в�����ê��
    private Dictionary<ulong, AnchorCTR> anchorList = new Dictionary<ulong, AnchorCTR>();//����ê����㼯��
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

        PXR_Manager.SpatialAnchorDataUpdated += SpatialAnchorDataUpdated; // ע��ê�����ݸ����¼�
    }

    private void OnDisable()
    {

        rightGrip.action.started -= OnRightGripPressed;
        rightGrip.action.canceled -= OnRightGripReleased;

        rPrimaryButton_A.action.started -= OnRighPrimaryPressed;
        rPrimaryButton_A.action.canceled -= OnRighPrimaryReleased;

        interactor.hoverEntered.RemoveListener(HoverEntered);
        interactor.hoverExited.RemoveListener(HoverExited);

        PXR_Manager.SpatialAnchorDataUpdated -= SpatialAnchorDataUpdated; // ע��ê�����ݸ����¼�

    }

    private void FixedUpdate()
    {
        HandleSpatialDrift();
    }

    //����У׼�������
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
                    // ����ɹ������µ�ǰ�����λ�ú���ת
                    anchorObj.transform.position = position;
                    anchorObj.transform.rotation = rotation;
                }
            }
        }
    }

    //�������ê��
    private void OnClickLoadAnchorBtn()
    {
        PressedLoadAllAnchors();
    }

    //�������ê��
    private void OnClickCreateAnchorBtn()
    {
        if (!IsCreateAnchor)//��������
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

    //���ê��
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

    // ê�����ݸ����¼�
    private void SpatialAnchorDataUpdated()
    {
        PressedLoadAllAnchors(); // ��������ê��
    }

    private async void StartSpatialAnchorProvider()
    {
        var result = await PXR_MixedReality.StartSenseDataProvider(PxrSenseDataProviderType.SpatialAnchor); // �����ռ�ê���֪�����ṩ��

        if (result == PxrResult.SUCCESS) // �ɹ�����
        {
            PressedLoadAllAnchors();
        }
        Debug.Log("StartSpatialAnchorProvider " + result.ToString());
    }

    //����ê��
    private void CreateAnchor()
    {
        CreateSpatialAnchor(anchorPreview.transform);
    }

    //�����������л�ê��
    private async void PressedLoadAllAnchors()
    {
        var result = await PXR_MixedReality.QuerySpatialAnchorAsync(); // ��ѯ���пռ�ê��
        if (result.result == PxrResult.SUCCESS) // �ɹ���ѯ
        {
            foreach (var key in result.anchorHandleList) // ����ê����
            {
                if (!anchorList.ContainsKey(key)) // ���ê���б��в����ڸ�ê��
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

    // �첽�����ռ�ê��
    private async void CreateSpatialAnchor(Transform _transform)
    {
        var result = await PXR_MixedReality.CreateSpatialAnchorAsync(_transform.position, _transform.rotation); // ����ê��

        if (result.result == PxrResult.SUCCESS) // �ɹ�����
        {
            Debug.Log("����ê��ɹ� uuID " + result.uuid.ToString() + " anchorHandle " + result.anchorHandle.ToString());

            AnchorCTR anchorObject = Instantiate(anchorPrefab);
            anchorObject.OnInit(result.anchorHandle, result.uuid);
            anchorObject.transform.rotation = _transform.rotation;
            anchorObject.transform.position = _transform.position;
            anchorList.Add(result.anchorHandle, anchorObject);
        }
    }

    //���߽���
    private void HoverEntered(HoverEnterEventArgs arg0)
    {
        hoverInteractable = (XRBaseInteractable)arg0.interactableObject;
    }

    //�����˳�
    private void HoverExited(HoverExitEventArgs arg0)
    {
        hoverInteractable = null;
    }

    //A���ͷ� �ر�ê��˵�
    private void OnRighPrimaryReleased(InputAction.CallbackContext obj)
    {
        // Debug.Log("OnRighPrimaryReleased _ A");

        if (selectAnchorCtr != null)
        {
            selectAnchorCtr.SetUIMenuShow(false);
            selectAnchorCtr = null;
        }
    }

    //A���̰��� ����ê�� ���ߴ�ê��˵�
    private void OnRighPrimaryPressed(InputAction.CallbackContext obj)
    {
        //Debug.Log("OnRighPrimaryPressed _ A");
        if (anchorPreview.gameObject.activeSelf && IsCreateAnchor) //��������ê�㹦��
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

    //�Ҽ�ץȡ �򿪲˵�
    private void OnRightGripPressed(InputAction.CallbackContext callback)
    {
        MenuUI.gameObject.SetActive(true);
    }

    //�Ҽ��ͷ� �رղ˵�
    private void OnRightGripReleased(InputAction.CallbackContext callback)
    {
        MenuUI.gameObject.SetActive(false);
    }

    //ɾ��ê��
    public void DestroyAnchor(ulong anchorHandle)
    {
        if (anchorList.ContainsKey(anchorHandle))
        {
            Destroy(anchorList[anchorHandle].gameObject);
            anchorList.Remove(anchorHandle);
        }
    }
}
