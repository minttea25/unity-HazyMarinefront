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

public enum ShipSymbol
{
    NoShip = 0, // no ship

    A0 = 1, // Mainship of A
    A1 = 2,
    A2 = 3,
    A3 = 4,
    A4 = 5,

    B0 = 11, // Mainship of B
    B1 = 12,
    B2 = 13,
    B3 = 14,
    B4 = 15,

    NM = 10 // Naval Mine
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

    public float OCEAN_TILE_INTERVAL = 2f;

    // 시작 cost 값
    public int START_COST = 0;

    // 턴 시작시 마다 얻는 cost 값
    public int TURN_COST = 2;

    // ship ability cost
    public int MAINSHIP_ABILITY_COST = 3;
    public int SUBSHIP1_ABILITY_COST = 3;
    public int SUBSHIP2_ABILITY_COST = 4;
    public int SUBSHIP3_ABILITY_COST = 5;

    


    // do not modify!
    public float SPAWNED_SHIP_ALPHA_VALUE = 1.0f;
    public static float spawnedShipAlphaValue { get; set; }

    public float SHIP_REVEALED_ALPHA_VALUE = 1.0f;
    public static float shipRevealedAlphaValue { get; set; }
    public string SHIP_UPPDER_COMPONENT_NAME = "Cube1";
    public static string shipUpperComponentName { get; set; }
    public string SHIP_DOWN_COMPONENT_NAME = "Cube2";
    public static string shipDownComponentName { get; set; }

    public string A_MAINSHIP_NAME_CLIENT = "TeamA_MainShip(Clone)";
    public string A_SUBSHIP1_NAME_CLIENT = "TeamA_SubShip1(Clone)";
    public string A_SUBSHIP2_NAME_CLIENT = "TeamA_SubShip2(Clone)";
    public string A_SUBSHIP3_NAME_CLIENT = "TeamA_SubShip3(Clone)";

    public string B_MAINSHIP_NAME_CLIENT = "TeamB_MainShip(Clone)";
    public string B_SUBSHIP1_NAME_CLIENT = "TeamB_SubShip1(Clone)";
    public string B_SUBSHIP2_NAME_CLIENT = "TeamB_SubShip2(Clone)";
    public string B_SUBSHIP3_NAME_CLIENT = "TeamB_SubShip3(Clone)";

    public static string aMainshipNameClient { get; set; }
    public static string aSubship1NameClient { get; set; }
    public static string aSubship2NameClient { get; set; }
    public static string aSubship3NameClient { get; set; }
    public static string bMainshipNameClient { get; set; }
    public static string bSubship1NameClient { get; set; }
    public static string bSubship2NameClient { get; set; }
    public static string bSubship3NameClient { get; set; }


    public static Vector2Int mapSize { get; private set; }
    public static int areaSize { get; private set; }
    public static int spawnLeastInterval { get; private set; }
    public static float oceanFogInterval { get; private set; }
    public static float oceanTileInterval { get; private set; }
    public static int startCost { get; private set; }
    public static int turnCost { get; private set; }

    public static int mainshipAbilityCost { get; private set; }
    public static int subship1AbilityCost { get; private set; }
    public static int subship2AbilityCost { get; private set; }
    public static int subship3AbilityCost { get; private set; }

    MapLayout()
    {
        mapSize = new Vector2Int(SIZE_X, SIZE_Y);
        areaSize = AREA_SIZE;
        spawnLeastInterval = SPAWN_LEAST_INTERVAL;
        oceanFogInterval = OCEAN_FOG_INTERVAL;
        oceanTileInterval = OCEAN_TILE_INTERVAL;
        startCost = START_COST;
        turnCost = TURN_COST;

        mainshipAbilityCost = MAINSHIP_ABILITY_COST;
        subship1AbilityCost = SUBSHIP1_ABILITY_COST;
        subship2AbilityCost = SUBSHIP2_ABILITY_COST;
        subship3AbilityCost = SUBSHIP3_ABILITY_COST;

        spawnedShipAlphaValue = SPAWNED_SHIP_ALPHA_VALUE;
        shipRevealedAlphaValue = SHIP_REVEALED_ALPHA_VALUE;
        shipUpperComponentName = SHIP_UPPDER_COMPONENT_NAME;
        shipDownComponentName = SHIP_DOWN_COMPONENT_NAME;

        aMainshipNameClient = A_MAINSHIP_NAME_CLIENT;
        aSubship1NameClient = A_SUBSHIP1_NAME_CLIENT;
        aSubship2NameClient = A_SUBSHIP2_NAME_CLIENT;
        aSubship3NameClient = A_SUBSHIP3_NAME_CLIENT;

        bMainshipNameClient = B_MAINSHIP_NAME_CLIENT;
        bSubship1NameClient = B_SUBSHIP1_NAME_CLIENT;
        bSubship2NameClient = B_SUBSHIP2_NAME_CLIENT;
        bSubship3NameClient = B_SUBSHIP3_NAME_CLIENT;
    }

    public static Team GetTeamByShipSymbol(ShipSymbol s)
    {
        if (s == ShipSymbol.NoShip)
        {
            return Team.ATeam;
        }

        if (s == ShipSymbol.A0 || s == ShipSymbol.A1 || s == ShipSymbol.A2 || s == ShipSymbol.A3)
        {
            return Team.ATeam;
        }
        else
        {
            return Team.BTeam;
        }
    }

    public static ShipSymbol GetSymbolByShiptypeTeam(ShipType type, Team team)
    {
        ShipSymbol s = ShipSymbol.NoShip;

        switch (team)
        {
            case Team.ATeam:
                switch (type)
                {
                    case ShipType.MainShip:
                        s = ShipSymbol.A0;
                        break;
                    case ShipType.SubShip1:
                        s = ShipSymbol.A1;
                        break;
                    case ShipType.SubShip2:
                        s = ShipSymbol.A2;
                        break;
                    case ShipType.SubShip3:
                        s = ShipSymbol.A3;
                        break;
                    case ShipType.SubShip4:
                        s = ShipSymbol.A4;
                        break;
                }
                break;

            case Team.BTeam:
                switch (type)
                {
                    case ShipType.MainShip:
                        s = ShipSymbol.B0;
                        break;
                    case ShipType.SubShip1:
                        s = ShipSymbol.B1;
                        break;
                    case ShipType.SubShip2:
                        s = ShipSymbol.B2;
                        break;
                    case ShipType.SubShip3:
                        s = ShipSymbol.B3;
                        break;
                    case ShipType.SubShip4:
                        s = ShipSymbol.B4;
                        break;
                }
                break;
        }

        return s;
    }

    public static int GetCostByShipType(ShipType type)
    {
        switch (type)
        {
            case ShipType.MainShip:
                return MapLayout.mainshipAbilityCost;
            case ShipType.SubShip1:
                return MapLayout.subship1AbilityCost;
            case ShipType.SubShip2:
                return MapLayout.subship2AbilityCost;
            case ShipType.SubShip3:
                return MapLayout.subship3AbilityCost;
            default:
                return -1;
        }
    }
}
