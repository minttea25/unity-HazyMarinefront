using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class SubShip3 : Ship
{
    private void Awake()
    {
        shipSizeX = 1;
        shipSizeY = 3;
        shipHealth = shipSizeX * shipSizeY;

        shipType = ShipType.SubShip3;

        abilityCost = MapLayout.subship3AbilityCost;

        visibility = false;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        float avgCoordX = coords[0].x; //coords[0].x = coords[1].x = coords[2].x
        float avgCoordY = (coords[0].y + coords[coords.Count - 1].y) / 2f;

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override Vector3 GetAIShipCenterPositionFromCoord(List<Vector3Int> coords, AIMap map)
    {
        float avgCoordX = coords[0].x; //coords[0].x = coords[1].x = coords[2].x
        float avgCoordY = (coords[0].y + coords[coords.Count - 1].y) / 2f;

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� �������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // �翷���� 2ĭ�̹Ƿ� ���� �׻� �� ĭ ���� �� �ֵ��� y ���� -2�ؼ� ��������
            int x = Random.Range(0, MapLayout.mapSize.x);
            int y = Random.Range(0, MapLayout.mapSize.y - 2);

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            y += 1;

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            y += 1;

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            list.Add(new Vector3Int(x, y - 2, 0));
            list.Add(new Vector3Int(x, y - 1, 0));
            list.Add(new Vector3Int(x, y, 0));

            break;
        }

        return list;
    }

    public override List<Vector3Int> GetPosibleAIShipSpawnCoordsList(AIMap map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� �������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // �翷���� 2ĭ�̹Ƿ� ���� �׻� �� ĭ ���� �� �ֵ��� y ���� -2�ؼ� ��������
            int x = Random.Range(0, MapLayout.mapSize.x);
            int y = Random.Range(0, MapLayout.mapSize.y - 2);

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            y += 1;

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            y += 1;

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            list.Add(new Vector3Int(x, y - 2, 0));
            list.Add(new Vector3Int(x, y - 1, 0));
            list.Add(new Vector3Int(x, y, 0));

            break;
        }

        return list;
    }

    public override void ActivateAbility()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return;
        }

        //�ڰ� ����
        for (int i = 0; i < this.shipSizeY; i++)
        {
            if (this.shipCoords[i].z != 0)
            {
                //this.shipCoords[i].z = 0;
                PlayManager.MapInstance.GetComponent<Map>().GetSelectedShip().shipCoords[i] = new Vector3Int(this.shipCoords[i].x, this.shipCoords[i].y, 0);
                this.shipHealth++;
                break;
            }
        }
    }
}
