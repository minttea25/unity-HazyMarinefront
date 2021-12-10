using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarEvent : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject RadarObjectPrefab;

    public GameObject[] RadarInstances = new GameObject[MapLayout.numberOfTypesOfShips];
    public GameObject[] RadarTransforms = new GameObject[MapLayout.numberOfTypesOfShips];
    
    void Start()
    {
        for (int i=0; i<RadarInstances.Length; i++)
        {
            RadarInstances[i] = (GameObject)Instantiate(
                RadarObjectPrefab);
            RadarInstances[i].transform.localPosition = RadarTransforms[i].transform.position;
            RadarInstances[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            RadarInstances[i].GetComponent<RaderObject>().SetShiptype((ShipType)i);
            RadarInstances[i].GetComponent<RaderObject>().SetRadarPos(RadarTransforms[i].transform.position);
        }

        RadarInstances[4].SetActive(false);
    }

    public void ActivateRadars()
    {
        foreach(var r in RadarInstances)
        {
            if (!r.GetComponent<RaderObject>().RevealDots)
            {
                continue;
            }

            r.GetComponent<RaderObject>().RefreshDots();
        }
    }

    public Vector3 GetPosFromShiptype(ShipType type)
    {
        return type switch
        {
            ShipType.MainShip => RadarTransforms[0].transform.position,
            ShipType.SubShip1 => RadarTransforms[1].transform.position,
            ShipType.SubShip2 => RadarTransforms[2].transform.position,
            ShipType.SubShip3 => RadarTransforms[3].transform.position,
            ShipType.SubShip4 => RadarTransforms[4].transform.position,
            _ => new Vector3(-1, -1, -1),
        };
    }

    public void DestroyRadar(ShipType type)
    {
        RadarInstances[(int)type].GetComponent<RaderObject>().SetRevealDots(false);
        Destroy(RadarInstances[(int)type]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
