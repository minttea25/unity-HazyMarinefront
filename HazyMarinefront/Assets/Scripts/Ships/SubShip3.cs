using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubShip3 : Ship
{
    private void Awake()
    {
        shipSizeX = 1;
        shipSizeY = 3;

        shipType = ShipType.SubShip3;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector2Int> coords, Map map)
    {
        float avgCoordX = coords[0].x; //coords[0].x = coords[1].x = coords[2].x
        float avgCoordY = (coords[0].y + coords[coords.Count-1].y) / 2f;

        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + map.areaSize / 2f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + map.areaSize / 2f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector2Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // 먼저 좌표 하나뽑고 만약 가능한 좌표면 위로으로 두칸 추가해서 다시 그 칸 가능한 좌표인지 확인

        List<Vector2Int> list = new List<Vector2Int>();

        while (true)
        {
            // 양옆으로 2칸이므로 위에 항상 두 칸 놓을 수 있도록 y 값은 -2해서 가져오기
            int x = Random.Range(0, map.mapSize.x);
            int y = Random.Range(0, map.mapSize.y - 2);

            bool posible1 = map.CheckIsShipNear(new Vector2Int(x, y));

            if (!posible1)
            {
                continue;
            }

            y += 1;

            bool posible2 = map.CheckIsShipNear(new Vector2Int(x, y));

            if (!posible2)
            {
                continue;
            }

            y += 1;

            bool posible3 = map.CheckIsShipNear(new Vector2Int(x, y));

            if (!posible3)
            {
                continue;
            }

            list.Add(new Vector2Int(x, y - 2));
            list.Add(new Vector2Int(x, y - 1));
            list.Add(new Vector2Int(x, y));

            break;
        }

        return list;
    }
}
