using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIConsole : MonoBehaviour
{
    public InputActionReference menuAction;
    public Canvas canvas;

    void Start()
    {
        if (menuAction != null)
        {
            menuAction.action.Enable();
            menuAction.action.performed += ToggleMenu;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        canvas.enabled = !canvas.enabled;
    }
}
