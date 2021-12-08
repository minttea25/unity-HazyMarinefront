using MLAPI;
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
        shipHealth = shipSizeX * shipSizeY;

        shipType = ShipType.SubShip1;

        abilityCost = MapLayout.subship1AbilityCost;

        visibility = false;

    }

    public override Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map)
    {
        float avgCoordX = (coords[0].x + coords[1].x) / 2f;
        float avgCoordY = coords[0].y; // coords[0].y = coords[1].y

        // 이부분은 배마다 다르게
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (avgCoordX + +0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (avgCoordY + +0.5f);
        // Debug.Log(map.bottomLeftSquareTransform.transform.position.y); = 2

        return new Vector3(x, map.bottomLeftSquareTransform.transform.position.y, z);
    }

    public override List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map)
    {
        // 먼저 좌표 하나뽑고 만약 가능한 좌표면 오른쪽으로 한칸 추가해서 다시 그 칸 가능한 좌표인지 확인

        List<Vector3Int> list = new List<Vector3Int>();

        while (true)
        {
            // 양옆으로 2칸이므로 오른쪽에 항상 한 칸 놓을 수 있도록 x 값은 -1해서 가져오기
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
        //십자 포격
        GameObject.Find("AttackBtnEventObject").GetComponent<AttackBtnEventListner>().SetAttackMode(true);

        if (!GameObject.Find("AttackBtnEventObject").GetComponent<AttackBtnEventListner>().AttackMode)
        {
            Vector2Int curCoord = GameObject.Find("Map(Clone)").GetComponent<Map>().selectedCoord;

            GameObject.Find("NetworkManager").GetComponent<PlayManager>().AttackServerRpc(curCoord.x + 1, curCoord.y);
            GameObject.Find("NetworkManager").GetComponent<PlayManager>().AttackServerRpc(curCoord.x - 1, curCoord.y);
            GameObject.Find("NetworkManager").GetComponent<PlayManager>().AttackServerRpc(curCoord.x, curCoord.y + 1);
            GameObject.Find("NetworkManager").GetComponent<PlayManager>().AttackServerRpc(curCoord.x, curCoord.y - 1);
        }
    }
}
