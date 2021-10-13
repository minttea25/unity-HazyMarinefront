using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// 아직 파손 상태의 배 중심 좌표에 대한 구현은 없음

public class SubShip1 : Ship
{
    private void Awake()
    {
        shipSizeX = 2;
        shipSizeY = 1;

        shipType = ShipType.SubShip1;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector2Int> coords, Map map)
    {
        float avgCoordX = (coords[0].x + coords[1].x) / 2f;
        float avgCoordY = coords[0].y; // coords[0].y = coords[1].y

        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + map.areaSize / 2f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + map.areaSize / 2f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector2Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // 먼저 좌표 하나뽑고 만약 가능한 좌표면 오른쪽으로 한칸 추가해서 다시 그 칸 가능한 좌표인지 확인

        List<Vector2Int> list = new List<Vector2Int>();

        while(true)
        {
            // 양옆으로 2칸이므로 오른쪽에 항상 한 칸 놓을 수 있도록 x 값은 -1해서 가져오기
            int x = Random.Range(0, map.mapSize.x - 1);
            int y = Random.Range(0, map.mapSize.y);

            bool posible1 = map.CheckIsShipNear(new Vector2Int(x, y));

            if (!posible1)
            {
                continue;
            }

            x += 1;

            bool posible2 = map.CheckIsShipNear(new Vector2Int(x, y));

            if (!posible2)
            {
                continue;
            }

            list.Add(new Vector2Int(x - 1, y));
            list.Add(new Vector2Int(x, y));

            break;
        }

        return list;
        
    }
}
