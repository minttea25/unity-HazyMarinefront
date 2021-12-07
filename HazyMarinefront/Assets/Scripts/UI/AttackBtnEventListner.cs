using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class AttackBtnEventListner : MonoBehaviour
{

    public Button attackBtn;
    public Text attackText;

    public bool AttackMode;

    public void SetAttack()
    {
        if (AttackMode)
        {
            attackText.text = "ATTACK";
            AttackMode = false;
        }
        else
        {
            attackText.text = "CANCEL";
            AttackMode = true;
        }
    }

    public void SetAttackMode(bool atk)
    {
        if (atk)
        {
            attackText.text = "CANCEL";
            AttackMode = true;
        }
        else
        {
            attackText.text = "ATTACK";
            AttackMode = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AttackMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
