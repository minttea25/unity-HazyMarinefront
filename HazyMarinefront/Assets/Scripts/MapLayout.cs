using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// element �߰� �� ��ư �̺�Ʈ ó�� �� ship���� �ڵ� ���� �ʿ�
//Dictionary<ShipType, string> shipTypeToString = new Dictionary<ShipType, string>();
//Dictionary<DirectionType, string> dirTypeToString = new Dictionary<DirectionType, string>();

public enum Team
{
    ATeam = 0, BTeam = 1
}

public enum ShipType
{
    MainShip = 0, 
    SubShip1 = 1, 
    SubShip2 = 2, 
    SubShip3 = 3, 
    SubShip4 = 4
}

public enum DirectionType
{
    Right, Left, Front, Back
}

public class MapLayout : MonoBehaviour
{
    // �����ϸ� �������� ������ ���� �����Ͽ� �ҷ����� �������

    const int SIZE_X = 10;
    const int SIZE_Y = 10;

    const int AREA_SIZE = 1;

    const int SPAWN_LEAST_INTERVAL = 3;

    public Vector2Int mapSize { get; set; }
    public int areaSize { get; set; }
    public int spawnLeastInterval { get; set; }

    private void Awake()
    {
        mapSize = new Vector2Int(SIZE_X, SIZE_Y);
        areaSize = AREA_SIZE;
        spawnLeastInterval = SPAWN_LEAST_INTERVAL;
    }
}
