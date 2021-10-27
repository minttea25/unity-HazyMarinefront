using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int SHIP_COUNT = 4;

    public GameObject[] teamAShipPrefabs;
    public GameObject[] teamBShipPrefabs;

    public Map map;

    // Start is called before the first frame update
    void Start()
    {
        if (SHIP_COUNT != teamAShipPrefabs.Length || SHIP_COUNT != teamBShipPrefabs.Length)
        {
            Debug.Log("SHIP_COUNT != shipRefabs.Length");
            return;
        }

        for (int i=0; i<SHIP_COUNT; i++)
        {
            map.SpawnShipRandomCoord(teamAShipPrefabs[i], Team.ATeam);
            map.SpawnShipRandomCoord(teamBShipPrefabs[i], Team.BTeam);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // temp
        if (Input.GetButtonDown("Fire1"))
        {
            // UI ���� (����, �̵� ĭ ��)


            //map.MoveShip(dirType, amount);
        }
            
    }
}
