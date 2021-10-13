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

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (avgCoordX + map.areaSize / 2f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (avgCoordY + map.areaSize / 2f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector2Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� �������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector2Int> list = new List<Vector2Int>();

        while (true)
        {
            // �翷���� 2ĭ�̹Ƿ� ���� �׻� �� ĭ ���� �� �ֵ��� y ���� -2�ؼ� ��������
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
