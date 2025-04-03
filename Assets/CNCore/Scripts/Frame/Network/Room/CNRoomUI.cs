using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CNRoomUI : MonoBehaviour
{
    public static CNRoomUI singleton;

    [Tooltip("Start Game Button")]
    public Button startGameButton;

    void Start()
    {
        singleton = this;
    }

    /// <summary>
    /// Set the start game button active or inactive.
    /// </summary>
    /// <param name="active"></param>
    public void SetStartGameButtonActive(bool active)
    {
        if (startGameButton != null)
        {
            startGameButton.gameObject.SetActive(active);
        }
    }

    /// <summary>
    /// Called when the start game button is clicked.
    /// </summary>
    public void OnStartGameButtonClick()
    {
        // Start the game
        CNNetworkRoomManagerExt.singleton.LetsGoGame();
    }
}