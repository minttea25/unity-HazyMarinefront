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
        // �Ʒ� �ڵ� ���� �ʿ� (ojy ������Ʈ �� �귣ġ���� Ȯ�� �� ������ ��. �ϴ� �Ʒ� �ڵ�� null)
        //Ship s = GameObject.Find("NetworkManager").GetComponent<PlayManager>().MapInstance.GetComponent<Map>().GetSelectedShip();
        int s = 4;


        // �Ʒ� �ڵ�� ���� �Ϸ�
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
        {
            return;
        }

        // cost �ִ��� Ȯ��
        if (TurnManager.cost < s)
        {
            // �����մϴ�! ���â ����
            Debug.Log("Cost ����! �ʿ� cost: " + s);
            GameObject dialog = Instantiate(
                AlertDialogPrefab
                );
            dialog.GetComponent<AlertDialog>().SetTitle("Cost ����! - �ʿ� Cost: " + s);
            return;
        }
        else
        {
            Debug.Log("cost �Ҹ��Ͽ� �����Ƽ �ߵ�: " + s);
        }

        TurnManager.cost -= s;
        GameObject.Find("CostText").GetComponent<Text>().text = TurnManager.cost.ToString();

        // ���� �� �ڵ�� ���������� null ������ ���� x
        //s.ActivateAbility();
    }
}
