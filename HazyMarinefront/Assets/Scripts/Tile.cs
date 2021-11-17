using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    Vector2Int curCoord;
    public FixedFogManager fixedFogManager;
    public Map map;

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

        if (GameObject.Find("Map(Clone)").GetComponent<Map>().Attack)
        {
            GameObject.Find("FixedFogManager").GetComponent<FixedFogManager>().ClearFog(curCoord);

            GameObject.Find("Map(Clone)").GetComponent<Map>().AttackCoord(curCoord);
            GameObject.Find("Map(Clone)").GetComponent<Map>().Attack = false;
            GameObject.Find("AttackBtn").GetComponent<Button>().GetComponentInChildren<Text>().text = "ATTACK";

            transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
        }
        //fixedFogManager.attack = true;
        //fixedFogManager.atkCoord = curCoord;


        //FixedFogManager.ClearFog(curCoord);
    }

}
