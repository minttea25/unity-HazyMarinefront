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

    // �ϴ� �Ȱ� Ŭ�� �ø��� ���� �����ϵ��� (���߿� �������� �����ϴ� ������� ���� �ʿ�)
    public void SetAttack()
    {
        Debug.Log(AttackMode);

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
        // attack ���� cancel -> attack���� �ٽ� �ٲ��ִ� ��� ���� x
        /*if (!GameObject.Find("Map(Clone)").GetComponent<Map>().Attack)
        {
            GameObject.Find("Map(Clone)").GetComponent<Map>().Attack = true;
            attackText.text = "CANCEL";
        }
        else
        {
            GameObject.Find("Map(Clone)").GetComponent<Map>().Attack = false;
            attackText.text = "ATTACK";
        }*/
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
