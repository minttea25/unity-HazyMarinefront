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

        shipType = ShipType.MainShip;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector2Int> coords, Map map)
    {
        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (coords[0].x + map.areaSize / 2f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (coords[0].y + map.areaSize / 2f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector2Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        while (true)
        {
            int x = Random.Range(0, map.mapSize.x);
            int y = Random.Range(0, map.mapSize.y);

            bool posible = map.CheckIsShipNear(new Vector2Int(x, y));

            if (!posible)
                continue;
            

            list.Add(new Vector2Int(x, y));

            break;
        }

        return list;

    }
}
