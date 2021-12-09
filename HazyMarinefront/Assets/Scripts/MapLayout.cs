using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// element ???? ?? ???? ?????? ???? ?? ship???? ???? ???? ????
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
    // ???????? ???????? ?????? ???? ???????? ???????? ????????
    // ???? ?????????? ???? ???? ?????????? public?? ?????? const ???? (temp)

    // ?? ???????? ??????
    public int SIZE_X = 8;
    public int SIZE_Y = 8;

    // ?? ???? ???? position?? ???? ????! ?? ???? ???? ?? ???? ????
    public int AREA_SIZE = 1;

    // ?? ???? ???? ???? (???? n*n ???? ???? ???? ????????)
    public int SPAWN_LEAST_INTERVAL = 3;

    // ?????? ???? ?????? ????
    public float OCEAN_FOG_INTERVAL = 1f;

    public float OCEAN_TILE_INTERVAL = 2f;


    public static Vector2Int mapSize { get; set; }
    public static int areaSize { get; set; }
    public static int spawnLeastInterval { get; set; }
    public static float oceanFogInterval { get; set; }
    public static float oceanTileInterval { get; set; }

    MapLayout()
    {
        mapSize = new Vector2Int(SIZE_X, SIZE_Y);
        areaSize = AREA_SIZE;
        spawnLeastInterval = SPAWN_LEAST_INTERVAL;
        oceanFogInterval = OCEAN_FOG_INTERVAL;
        oceanTileInterval = OCEAN_TILE_INTERVAL;
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
}
