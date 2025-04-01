using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CNMatchItem : MonoBehaviour
{
    [Tooltip("触发按钮")]
    [SerializeField] Button button;

    [Tooltip("房间ID")]
    [SerializeField] Text matchIDText;

    public void SetMatchID(string matchID)
    {
        matchIDText.text = matchID;
    }

    public void SetButtonClickListener(UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
