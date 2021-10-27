using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubShip2 : Ship
{

    private void Awake()
    {
        shipSizeX = 1;
        shipSizeY = 2;
        shipHealth = shipSizeX * shipSizeY;

        shipType = ShipType.SubShip2;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        float avgCoordX = coords[0].x; // coords[0].x = coords[1].x
        float avgCoordY = (coords[0].y + coords[1].y) / 2f;

        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + +0.5f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // 먼저 좌표 하나뽑고 만약 가능한 좌표면 위로으로 한칸 추가해서 다시 그 칸 가능한 좌표인지 확인

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // 양옆으로 2칸이므로 위에 항상 한 칸 놓을 수 있도록 y 값은 -1해서 가져오기
            int x = Random.Range(0, map.mapSize.x);
            int y = Random.Range(0, map.mapSize.y - 1);

            bool posible1 = map.CheckIsShipNear(new Vector3Int(x, y, 0));

            if (!posible1)
            {
                continue;
            }

            y += 1;

            bool posible2 = map.CheckIsShipNear(new Vector3Int(x, y, 0));

            if (!posible2)
            {
                continue;
            }

            list.Add(new Vector3Int(x, y - 1, 0));
            list.Add(new Vector3Int(x, y, 0));

            break;
        }

        return list;

    }
}
