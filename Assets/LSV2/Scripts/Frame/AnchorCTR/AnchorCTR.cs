using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.XR.PXR;

public class AnchorCTR : MonoBehaviour
{
    public Text idText;
    public GameObject UIMenu;
    public GameObject SaveIcon;

    public Button persistedBtn;
    public Button DestroyBtn;
    public Button unPersistedBtn;

    private ulong anchorHandle;
    private Guid uuid;

    private void Awake()
    {
        persistedBtn.onClick.AddListener(OnClickedPerststedBtn);    
        DestroyBtn.onClick.AddListener(OnClickedDestroy);
        unPersistedBtn.onClick.AddListener(OnClickUnPersistedBtn);
    }

    public void OnInit(ulong anchorHandle, Guid uuid)
    {
        this.anchorHandle = anchorHandle;
        this.uuid = uuid;
        idText.text = this.anchorHandle.ToString();
    }

    public void ShowSaveIcon(bool isShow = true)
    {
        SaveIcon.SetActive(isShow);
    }

    public void SetUIMenuShow(bool isShow = true)
    {
        UIMenu.gameObject.SetActive(isShow);
    }

    private async void OnClickUnPersistedBtn()
    {
        var result = await PXR_MixedReality.UnPersistSpatialAnchorAsync(anchorHandle);
        if (result == PxrResult.SUCCESS)
        {
            PlayerPrefs.DeleteKey(uuid.ToString());

            PXR_MixedReality.DestroyAnchor(anchorHandle);
            SpatialAnchors.Instance.DestroyAnchor(anchorHandle);
        }
    }

    private async void OnClickedPerststedBtn()
    {
        var result = await PXR_MixedReality.PersistSpatialAnchorAsync(anchorHandle);
        if (result == PxrResult.SUCCESS)
        {
            // 如果成功，显示保存图标
            ShowSaveIcon();
        }
    }

    private void OnClickedDestroy()
    {
        var result = PXR_MixedReality.DestroyAnchor(anchorHandle);
        if (result == PxrResult.SUCCESS)
        {
            SpatialAnchors.Instance.DestroyAnchor(anchorHandle);
        }
    }
}
