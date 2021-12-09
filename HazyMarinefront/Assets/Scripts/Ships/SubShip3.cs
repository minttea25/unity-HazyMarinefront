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

        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override Vector3 GetAIShipCenterPositionFromCoord(List<Vector3Int> coords, AIMap map)
    {
        float avgCoordX = coords[0].x; //coords[0].x = coords[1].x = coords[2].x
        float avgCoordY = (coords[0].y + coords[coords.Count - 1].y) / 2f;

        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // 먼저 좌표 하나뽑고 만약 가능한 좌표면 위로으로 두칸 추가해서 다시 그 칸 가능한 좌표인지 확인

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // 양옆으로 2칸이므로 위에 항상 두 칸 놓을 수 있도록 y 값은 -2해서 가져오기
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
        // 먼저 좌표 하나뽑고 만약 가능한 좌표면 위로으로 두칸 추가해서 다시 그 칸 가능한 좌표인지 확인

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // 양옆으로 2칸이므로 위에 항상 두 칸 놓을 수 있도록 y 값은 -2해서 가져오기
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

        //자가 수리
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
