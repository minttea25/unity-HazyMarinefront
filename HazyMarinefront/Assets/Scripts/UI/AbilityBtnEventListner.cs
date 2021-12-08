using MLAPI;
using MLAPI.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBtnEventListner : MonoBehaviour
{
    public GameObject AlertDialogPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateAbility()
    {
        // 아래 코드 수정 필요 (ojy 업데이트 된 브랜치에서 확인 후 가져올 것. 일단 아래 코드는 null)
        //Ship s = GameObject.Find("NetworkManager").GetComponent<PlayManager>().MapInstance.GetComponent<Map>().GetSelectedShip();
        int s = 4;


        // 아래 코드는 검증 완료
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
        {
            return;
        }

        // cost 있는지 확인
        if (TurnManager.cost < s)
        {
            // 부족합니다! 경고창 띄우기
            Debug.Log("Cost 부족! 필요 cost: " + s);
            GameObject dialog = Instantiate(
                AlertDialogPrefab
                );
            dialog.GetComponent<AlertDialog>().SetTitle("Cost 부족! - 필요 Cost: " + s);
            return;
        }
        else
        {
            Debug.Log("cost 소모하여 어빌리티 발동: " + s);
        }

        TurnManager.cost -= s;
        GameObject.Find("CostText").GetComponent<Text>().text = TurnManager.cost.ToString();

        // 가장 위 코드와 마찬가지로 null 값으로 실행 x
        //s.ActivateAbility();
    }
}
