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

    public Transform XRInteractionSetup;

    private void Awake()
    {
        tagAnchorPoint = Hint.position;
        tagAnchorRot = Hint.eulerAngles;
        activateAction.action.started += ActionStarted;
    }

    private void ActionStarted(InputAction.CallbackContext obj)
    {
        Root.SetPositionAndRotation(Hint.position, Hint.rotation);
        placeAnchor = true;
        Hint.gameObject.SetActive(false);

        if (XRInteractionSetup.parent != Root)
        {
            XRInteractionSetup.SetParent(Root);
        }
    }

    public void Update()
    {
        if (!placeAnchor)
        {
            tagAnchorPoint = HandControlPoint.position;
            tagAnchorPoint.y = 0;

            tagAnchorRot.y = HandControlPoint.eulerAngles.y;

            Hint.SetPositionAndRotation(tagAnchorPoint, Quaternion.Euler(tagAnchorRot));
        }
    }
}
