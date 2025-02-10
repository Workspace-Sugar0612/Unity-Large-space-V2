using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnchorControl : MonoBehaviour
{
    public Transform Root;
    public Transform Hint;
    public Transform HandControlPoint;

    private Vector3 tagAnchorPoint = Vector3.zero;
    private Vector3 tagAnchorRot = Vector3.zero;
    
    public InputActionProperty activateAction;
    private bool placeAnchor = false;
    private VRNetworkLauncher _vrNetLaucher;
    public Transform XRInteractionSetup;

    private void Awake()
    {
        tagAnchorPoint = Hint.position;
        tagAnchorRot = Hint.eulerAngles;
        activateAction.action.started += ActionStarted;

        if (_vrNetLaucher == null)
            _vrNetLaucher = (VRNetworkLauncher)FindObjectOfType(typeof(VRNetworkLauncher));
    }

    private void ActionStarted(InputAction.CallbackContext obj)
    {
        Root.SetPositionAndRotation(new Vector3(Hint.position.x, 0.0f, Hint.position.z), Hint.rotation);
        placeAnchor = true;
        Hint.gameObject.SetActive(false);

        if (XRInteractionSetup.parent != Root)
            XRInteractionSetup.SetParent(Root);

        StartCoroutine(_vrNetLaucher.Waiter());
    }

    public void Update()
    {
        if (!placeAnchor)
        {
            tagAnchorPoint = HandControlPoint.position;
            //tagAnchorPoint.y = 0;

            tagAnchorRot.y = HandControlPoint.eulerAngles.y;

            Hint.SetPositionAndRotation(tagAnchorPoint, Quaternion.Euler(tagAnchorRot));
        }
    }
}
