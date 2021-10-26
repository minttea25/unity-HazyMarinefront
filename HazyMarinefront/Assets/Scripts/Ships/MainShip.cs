using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MainShip : Ship
{
    private void Awake()
    {
        shipSizeX = 1;
        shipSizeY = 1;
        shipHealth = shipSizeX * shipSizeY;


        shipType = ShipType.MainShip;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        //Debug.Log(map.bottomLeftSquareTransform.transform.position);
        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (coords[0].x + 0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (coords[0].y + 0.5f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            int x = Random.Range(0, map.mapSize.x);
            int y = Random.Range(0, map.mapSize.y);

            bool posible = map.CheckIsShipNear(new Vector3Int(x, y, 0));

            if (!posible)
                continue;


            list.Add(new Vector3Int(x, y, 0));

            break;
        }

        return list;

    }
}
