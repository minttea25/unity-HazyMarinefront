using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : MonoBehaviour
{
    public Button okBtn;
    public Text title;

    public void SetTitle(string titleText)
    {
        title.text = titleText;
    }

    public void CloseDialog()
    {
        Debug.Log("CLOSE DIALOG");
        Destroy(GameObject.Find("AlertDialogUI(Clone)"));
    }
}
