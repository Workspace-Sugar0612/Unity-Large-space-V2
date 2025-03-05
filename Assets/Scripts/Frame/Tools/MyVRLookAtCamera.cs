using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVRLookAtCamera : MonoBehaviour
{
    public enum LookAtMethod
    {
        Rotation,
        Forward,
        LookAt,
        None
    }

    private Transform _mainCameraTrans;
    public LookAtMethod lookAtMethod = LookAtMethod.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_mainCameraTrans != null)
        {
            if (lookAtMethod == LookAtMethod.Rotation) { this.transform.rotation = Quaternion.LookRotation(this.transform.position - _mainCameraTrans.position); }
            else if (lookAtMethod == LookAtMethod.Forward) { this.transform.forward = _mainCameraTrans.forward; }
            else if (lookAtMethod == LookAtMethod.LookAt) { this.transform.LookAt(_mainCameraTrans); }
            else { }
        }
        else
            _mainCameraTrans = Camera.main.transform;
    }
}
