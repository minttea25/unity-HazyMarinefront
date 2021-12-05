using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class Tile : MonoBehaviour
{
    Vector2Int curCoord;

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
        if (!GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().AttackMode)
        {
            Debug.Log("Not AttackMode");
            return;
        }

        curCoord = new Vector2Int(((int)(transform.localPosition.x + 4.5)), (int)(transform.localPosition.z + 4.5));
        Debug.Log(" coord :" + curCoord);

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }
        // RPC method parameter does not support serialization: UnityEngine.Vector2Int
        // -> int �� 2�� ���
        PlayManager.AttackServerRpc(curCoord.x, curCoord.y);

        // added (ojy)
        GameObject.Find("Map(Clone)").GetComponent<Map>().selectedCoord = curCoord;

        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);

        GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetAttackMode(false);
    }

}
