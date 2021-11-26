using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class FixedFog: NetworkBehaviour
{
    // animation 이용

    //Vector2Int curCoord;
    //public Map map;

    private void Start()
    {
        //fixedFogManager = GetComponent<FixedFogManager>();

    }

    private void OnMouseEnter()
    {
        //Debug.Log(" fog color :" + transform.GetComponent<Renderer>().material.color);
        //Debug.Log(" coord :" + transform.localPosition);
        //transform.GetComponent<Renderer>().material.color = Color.;
    }

    private void OnMouseExit()
    {

    }

    void OnMouseDown()
    {
        return;
/*
        //transform.GetComponent<Renderer>().material.color = Color.clear;
        curCoord = new Vector2Int(((int)(transform.localPosition.x + 4.5)), (int)(transform.localPosition.z + 4.5));
        Debug.Log("clicked coord :" + curCoord);

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return;
        }

        // RPC method parameter does not support serialization: UnityEngine.Vector2Int
        // -> int 값 2개 사용
        PlayManager.AttackServerRpc(curCoord.x, curCoord.y);*/
    }
}