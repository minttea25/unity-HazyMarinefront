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

        // playmanager의 crossAtk이 true이면 AttackMode가 false여도 공격 가능하도록 변경 
        // 이유: subship1이 서버에 존재하기 때문에 subship1에서 클라이언트의 AttackMode 값을 변경할 수 없음
        // (방법1: subship1 -> PlayManager에서 clientrpc로 값 변경, 방법 2: 여기서 제어...(x))

        // bound 확인 로직 수정 하였음
        PlayManager.AttackServerRpc(curCoord.x, curCoord.y);
        if (GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().CrossAttackMode)
        {
            if (curCoord.x + 1 < MapLayout.mapSize.x)
                PlayManager.AttackServerRpc(curCoord.x + 1, curCoord.y);
            if (curCoord.x - 1 > 0)
                PlayManager.AttackServerRpc(curCoord.x - 1, curCoord.y);
            if (curCoord.y + 1 < MapLayout.mapSize.y)
                PlayManager.AttackServerRpc(curCoord.x, curCoord.y + 1);
            if (curCoord.y - 1 > 0)
                PlayManager.AttackServerRpc(curCoord.x, curCoord.y - 1);
            Debug.Log("십자공격 " + GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().CrossAttackMode);
            GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetCrossAttackMode(false);
        }

        // ???
        GameObject.Find("Map(Clone)").GetComponent<Map>().selectedCoord = curCoord;

        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);

        GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetAttackMode(false);
    }

}
