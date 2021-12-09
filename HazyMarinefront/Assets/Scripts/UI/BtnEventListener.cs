using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEventListener : MonoBehaviour
{
    public GameObject BtnControllUiCanvas;

    public Button leaveBtn;
    public Button spawnShipBtn;
    public Button attackBtn;
    public Button endTrunBtn;

    private void Awake()
    {
        BtnControllUiCanvas.SetActive(false);
    }

    internal void isHostConnected(bool v) => throw new NotImplementedException();
}
