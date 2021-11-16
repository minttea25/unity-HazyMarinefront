using UnityEngine;

public class FixedFog: MonoBehaviour
{
    // animation 이용

    Vector2Int curCoord;
    public FixedFogManager fixedFogManager;
    public Map map;

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

        //transform.GetComponent<Renderer>().material.color = Color.clear;
        curCoord = new Vector2Int(((int)(transform.localPosition.x + 4.5)), (int)(transform.localPosition.z + 4.5));
        Debug.Log(" coord :" + curCoord);

        if (GameObject.Find("Map(Clone)").GetComponent<Map>().Attack)
        {
            GameObject.Find("FixedFogManager").GetComponent<FixedFogManager>().ClearFog(curCoord);

            GameObject.Find("Map(Clone)").GetComponent<Map>().AttackCoord(curCoord);
            GameObject.Find("Map(Clone)").GetComponent<Map>().Attack = false;
        }
        //fixedFogManager.attack = true;
        //fixedFogManager.atkCoord = curCoord;


        //FixedFogManager.ClearFog(curCoord);
    }
}