using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// ���� �ļ� ������ �� �߽� ��ǥ�� ���� ������ ����

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

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + map.areaSize / 2f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + map.areaSize / 2f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector2Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� ���������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector2Int> list = new List<Vector2Int>();

        while(true)
        {
            // �翷���� 2ĭ�̹Ƿ� �����ʿ� �׻� �� ĭ ���� �� �ֵ��� x ���� -1�ؼ� ��������
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
