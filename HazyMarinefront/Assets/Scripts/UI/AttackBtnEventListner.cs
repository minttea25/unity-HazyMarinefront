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

    // 일단 안개 클릭 시마다 공격 가능하도록 (나중에 서버에서 제어하는 방식으로 변경 필요)
    public void SetAttack()
    {
        return;
        // attack 이후 cancel -> attack으로 다시 바꿔주는 기능 아직 x
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
