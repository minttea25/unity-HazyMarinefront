using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int SHIP_COUNT = 4;

    public GameObject[] shipPrefabs;

    public Map map;

    // Start is called before the first frame update
    void Start()
    {
        if (SHIP_COUNT != shipPrefabs.Length)
        {
            Debug.Log("SHIP_COUNT != shipRefabs.Length");
            return;
        }

        for (int i=0; i<SHIP_COUNT; i++)
        {
            map.SpawnShipRandomCoord(shipPrefabs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // temp
        if (Input.GetButtonDown("Fire1"))
        {
            // UI ¶ç¿ì±â (¹æÇâ, ÀÌµ¿ Ä­ ¼ö)


            //map.MoveShip(dirType, amount);
        }
            
    }
}
