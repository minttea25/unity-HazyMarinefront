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

    // �ϴ� �Ȱ� Ŭ�� �ø��� ���� �����ϵ��� (���߿� �������� �����ϴ� ������� ���� �ʿ�)
    public void SetAttack()
    {
        return;
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
