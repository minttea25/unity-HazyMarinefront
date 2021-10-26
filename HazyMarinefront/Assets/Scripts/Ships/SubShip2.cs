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

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + +0.5f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� �������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // �翷���� 2ĭ�̹Ƿ� ���� �׻� �� ĭ ���� �� �ֵ��� y ���� -1�ؼ� ��������
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
