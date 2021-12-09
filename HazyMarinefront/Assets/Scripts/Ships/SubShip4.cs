using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class SubShip4 : Ship
{
    private void Awake()
    {
        shipSizeX = 1;
        shipSizeY = 1;
        shipHealth = shipSizeX * shipSizeY;

        shipType = ShipType.SubShip4;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {

        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (coords[0].x + 0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (coords[0].y + 0.5f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            int x = Random.Range(0, MapLayout.mapSize.x);
            int y = Random.Range(0, MapLayout.mapSize.y);

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
                continue;

            list.Add(new Vector3Int(x, y, 0));

            break;
        }

        return list;
    }

    public override void ActivateAbility()
    {
        //none
    }
}
