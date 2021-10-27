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
        shipHealth = shipSizeX * shipSizeY;

        shipType = ShipType.SubShip1;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        float avgCoordX = (coords[0].x + coords[1].x) / 2f;
        float avgCoordY = coords[0].y; // coords[0].y = coords[1].y

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + +0.5f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� ���������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // �翷���� 2ĭ�̹Ƿ� �����ʿ� �׻� �� ĭ ���� �� �ֵ��� x ���� -1�ؼ� ��������
            int x = Random.Range(0, map.mapSize.x - 1);
            int y = Random.Range(0, map.mapSize.y);

            bool posible1 = map.CheckIsShipNear(new Vector3Int(x, y, 0));

            if (!posible1)
            {
                continue;
            }

            x += 1;

            bool posible2 = map.CheckIsShipNear(new Vector3Int(x, y, 0));

            if (!posible2)
            {
                continue;
            }

            list.Add(new Vector3Int(x - 1, y, 0));
            list.Add(new Vector3Int(x, y, 0));

            break;
        }

        return list;

    }
}
