using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using TMPro;

public class WinLoseBtnEventListener : MonoBehaviour
{
    public GameObject WinLoseCanvas;

    public Text WinLoseText;

    public void Awake()
    {
        WinLoseCanvas.SetActive(false);
    }

    public void ChangeWinLoseText(string t)
    {
        WinLoseText.text = t;
    }

    public void SetActiveWinLoseCanvas(bool show)
    {
        WinLoseCanvas.SetActive(show);
    }

    public void OKButtonLeave()
    {
        GetComponent<HostClientNetworkManager>().Leave();

        SetActiveWinLoseCanvas(false);
    }
}
