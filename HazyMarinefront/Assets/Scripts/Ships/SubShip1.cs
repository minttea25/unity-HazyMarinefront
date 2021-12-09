using MLAPI;
using MLAPI.Connection;
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

        abilityCost = MapLayout.subship1AbilityCost;

        visibility = false;

    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        float avgCoordX = (coords[0].x + coords[1].x) / 2f;
        float avgCoordY = coords[0].y; // coords[0].y = coords[1].y

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override Vector3 GetAIShipCenterPositionFromCoord(List<Vector3Int> coords, AIMap map)
    {
        float avgCoordX = (coords[0].x + coords[1].x) / 2f;
        float avgCoordY = coords[0].y; // coords[0].y = coords[1].y

        // �̺κ��� �踶�� �ٸ���
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);
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
            int x = Random.Range(0, MapLayout.mapSize.x - 1);
            int y = Random.Range(0, MapLayout.mapSize.y);


            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            x += 1;


            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            list.Add(new Vector3Int(x - 1, y, 0));
            list.Add(new Vector3Int(x, y, 0));

            break;
        }
        return list;
    }

    public override List<Vector3Int> GetPosibleAIShipSpawnCoordsList(AIMap map)
    {
        // ���� ��ǥ �ϳ��̰� ���� ������ ��ǥ�� ���������� ��ĭ �߰��ؼ� �ٽ� �� ĭ ������ ��ǥ���� Ȯ��

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // �翷���� 2ĭ�̹Ƿ� �����ʿ� �׻� �� ĭ ���� �� �ֵ��� x ���� -1�ؼ� ��������
            int x = Random.Range(0, MapLayout.mapSize.x - 1);
            int y = Random.Range(0, MapLayout.mapSize.y);


            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            x += 1;


            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
            {
                continue;
            }

            list.Add(new Vector3Int(x - 1, y, 0));
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
        //���� ����
        if (this.team == Team.ATeam)
        {
            GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetAttackMode(true);
            GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetCrossAttackMode(true);

        }
        else
        {
            PlayManager.SetAttackModeClientRpc(true);
            PlayManager.SetCrossAttackModeClientRpc(true);
        }
    }
}
