using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Canvas lobbyCanvas;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void Host()
    {
        inputField.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
        lobbyCanvas.gameObject.SetActive(false);

        Player.instance.HostGame();
    }

    public void HostSuccess(bool success)
    {
        if (success)
        {
            lobbyCanvas.gameObject.SetActive(true);
        }
        else
        {
            inputField.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
        }
    }

    public void Join()
    {
        inputField.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
    }

    public void JoinSuccess(bool success)
    {
        if (success)
        {

        }
        else
        {
            inputField.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
        }
    }
}
