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
    // �ϴ� �����Ϳ��� ���� ���� �����ϵ��� public�� ���̰� const ���� (temp)

    // �� �����ǥ ������
    public int SIZE_X = 10;
    public int SIZE_Y = 10;

    // �� ���� ���� position�� ���� ����! �� ũ�� ���� �� ���� �ʿ�
    public int AREA_SIZE = 1;

    // �� �ʱ� ���� ���� (�ֺ� n*n ���� �ٸ� �谡 �������)
    public int SPAWN_LEAST_INTERVAL = 3;

    // �Ȱ��� �ٴ� ������ �Ÿ�
    public float OCEAN_FOG_INTERVAL = 1f;


    public Vector2Int mapSize { get; set; }
    public int areaSize { get; set; }
    public int spawnLeastInterval { get; set; }
    public float oceanFogInterval { get; set; }

    private void Awake()
    {
        mapSize = new Vector2Int(SIZE_X, SIZE_Y);
        areaSize = AREA_SIZE;
        spawnLeastInterval = SPAWN_LEAST_INTERVAL;
        oceanFogInterval = OCEAN_FOG_INTERVAL;
    }
}
