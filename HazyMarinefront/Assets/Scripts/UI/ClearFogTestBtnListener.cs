using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearFogTestBtnListener : MonoBehaviour
{
    public Button clearFogButton;
    public FixedFogManager ffm;

    public void ClearRandomFog()
    {
        ffm.ClearFogTest();
    }
    
}
