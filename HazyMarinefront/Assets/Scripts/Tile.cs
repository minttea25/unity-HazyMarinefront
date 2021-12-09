using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class Tile : MonoBehaviour
{
    Vector2Int curCoord;
    public GameObject AlertDialogPrefab;

    private void OnMouseEnter()
    {
        transform.GetComponent<Renderer>().material.color = new Color(0, 0.5f, 1, 0.5f);
    }

    private void OnMouseExit()
    {
        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
    }

    void OnMouseDown()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return;
        }

        if (TurnManager.hasAttacked && !GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().CrossAttackMode)
        {
            GameObject dialog = Instantiate(
                AlertDialogPrefab);
            dialog.GetComponent<AlertDialog>().SetTitle("�� �Ͽ� �ѹ��� ���� �����մϴ�.");

            GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().SetAttackMode(false);

            return;
        }


        if (!GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().AttackMode)
        {
            Debug.Log("Not AttackMode");
            return;
        }

        curCoord = new Vector2Int(((int)(transform.localPosition.x + 4.5)), (int)(transform.localPosition.z + 4.5));
        Debug.Log(" coord :" + curCoord);


        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        // playmanager�� crossAtk�� true�̸� AttackMode�� false���� ���� �����ϵ��� ���� 
        // ����: subship1�� ������ �����ϱ� ������ subship1���� Ŭ���̾�Ʈ�� AttackMode ���� ������ �� ����
        // (���1: subship1 -> PlayManager���� clientrpc�� �� ����, ��� 2: ���⼭ ����...(x))

        // bound Ȯ�� ���� ���� �Ͽ���
        PlayManager.AttackServerRpc(curCoord.x, curCoord.y);
        if (GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().CrossAttackMode)
        {
            if (curCoord.x + 1 < MapLayout.mapSize.x)
                PlayManager.AttackServerRpc(curCoord.x + 1, curCoord.y);
            if (curCoord.x - 1 >= 0)
                PlayManager.AttackServerRpc(curCoord.x - 1, curCoord.y);
            if (curCoord.y + 1 < MapLayout.mapSize.y)
                PlayManager.AttackServerRpc(curCoord.x, curCoord.y + 1);
            if (curCoord.y - 1 >= 0)
                PlayManager.AttackServerRpc(curCoord.x, curCoord.y - 1);
            Debug.Log("���ڰ��� " + GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().CrossAttackMode);
            GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().SetCrossAttackMode(false);

            // ũ�ν� ������ �ѹ� ���ݿ� ���� x
        }
        else
        {
            TurnManager.hasAttacked = true;
        }

        // ???
        GameObject.Find("Map(Clone)").GetComponent<Map>().selectedCoord = curCoord;

        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);

        GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().SetAttackMode(false);

    }

}
