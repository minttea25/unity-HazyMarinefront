using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// element 추가 후 버튼 이벤트 처리 및 ship에서 코드 수정 필요
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
    // 가능하면 설정관련 파일은 따로 저장하여 불러오는 방식으로
    // 일단 에디터에서 직접 수정 가능하도록 public을 붙이고 const 제외 (temp)

    // 맵 상대좌표 사이즈
    public int SIZE_X = 10;
    public int SIZE_Y = 10;

    // 이 값은 실제 position에 대한 값임! 맵 크기 조절 시 변경 필요
    public int AREA_SIZE = 1;

    // 배 초기 스폰 조건 (주변 n*n 으로 다른 배가 없어야함)
    public int SPAWN_LEAST_INTERVAL = 3;

    // 안개와 바다 사이의 거리
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
