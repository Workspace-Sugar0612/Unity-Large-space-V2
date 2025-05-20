using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraController : MonoBehaviour
{
    public List<CameraItem> cameraList = new List<CameraItem>();

    public bool isTeacher = false;

    static SwitchCameraController instance;

    public static SwitchCameraController Get()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<SwitchCameraController>();
            if (instance == null)
            {
                GameObject obj = new GameObject("SwitchCameraController");
                instance = obj.AddComponent<SwitchCameraController>();
            }
        }
        return instance;
    }

    void Awake()
    {
     
    }

    void Start()
    {
        if (isTeacher)
        {
            SwitchToCamera(CameraTag.Manager);
        }
        else
        {
            SwitchToCamera(CameraTag.Player);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Log.cinput("red", "key down alpha1");
            SwitchToCamera(CameraTag.Manager);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Log.cinput("red", "key down alpha2");
            SwitchToCamera(CameraTag.Player);
        }
    }

    public void SwitchToCamera(CameraTag tag)
    {
        foreach (CameraItem item in cameraList)
        {
            if (tag == item.cameraTag)
            {
                item.gameObject.SetActive(true);
                item.tag = "MainCamera";
            }
            else
            {
                item.gameObject.SetActive(false);
                item.tag = "Untagged";
            }
        }
    }
}
