using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class Tile : MonoBehaviour
{
    Vector2Int curCoord;

    // Start is called before the first frame update
    void Start()
    {
        //transform.GetComponent<Renderer>().material.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        Debug.Log(" fog color :" + transform.GetComponent<Renderer>().material.color);
        //Debug.Log(" coord :" + transform.localPosition);
        transform.GetComponent<Renderer>().material.color = new Color(0, 0.5f, 1, 0.5f);
    }

    private void OnMouseExit()
    {
        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
    }

    void OnMouseDown()
    {
        //transform.GetComponent<Renderer>().material.color = Color.clear;
        curCoord = new Vector2Int(((int)(transform.localPosition.x + 4.5)), (int)(transform.localPosition.z + 4.5));
        Debug.Log(" coord :" + curCoord);

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
        PlayManager.AttackServerRpc(curCoord.x, curCoord.y);



        //GameObject.Find("FixedFogManager").GetComponent<FixedFogManager>().ClearFog(curCoord);

        //GameObject.Find("Map(Clone)").GetComponent<Map>().AttackCoord(curCoord);

        //GameObject.Find("Map(Clone)").GetComponent<Map>().Attack = false;
        //GameObject.Find("AttackBtn").GetComponent<Button>().GetComponentInChildren<Text>().text = "ATTACK";

        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);





        /*if (GameObject.Find("Map(Clone)").GetComponent<Map>().Attack)
        {
            GameObject.Find("FixedFogManager").GetComponent<FixedFogManager>().ClearFog(curCoord);

            GameObject.Find("Map(Clone)").GetComponent<Map>().AttackCoord(curCoord);
            GameObject.Find("Map(Clone)").GetComponent<Map>().Attack = false;
            GameObject.Find("AttackBtn").GetComponent<Button>().GetComponentInChildren<Text>().text = "ATTACK";

            transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
        }*/



        //fixedFogManager.attack = true;
        //fixedFogManager.atkCoord = curCoord;


        //FixedFogManager.ClearFog(curCoord);
    }

}
