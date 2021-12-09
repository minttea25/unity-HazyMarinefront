using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;
using Random = UnityEngine.Random;


public class MainShip : Ship
{

    private void Awake()
    {
        shipSizeX = 1;
        shipSizeY = 1;
        shipHealth = shipSizeX * shipSizeY;

        shipType = ShipType.MainShip;

        abilityCost = MapLayout.mainshipAbilityCost;

        visibility = false;
    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (coords[0].x + 0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (coords[0].y + 0.5f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            int x = Random.Range(0, MapLayout.mapSize.x);
            int y = Random.Range(0, MapLayout.mapSize.y);

            if (map.CheckIsShipNear(new Vector3Int(x, y, 0)))
                continue;

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

        //아군 소환
        if (this.team == Team.ATeam)
        {
            PlayManager.createShipServerRpc(4, true);
        }
        else
        {
            PlayManager.createShipServerRpc(4, false);
        }
    }
}
