using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public MapLayout mapLayout;
    public GameObject shipHolder;

    // 기준 좌표
    public Transform bottomLeftSquareTransform;

    public GameObject fogBlocks;
    public FixedFogManager fixedFogManager;


    public Vector2Int mapSize { get; private set; }

    // 배치 가능 주변 탐색범위 (n*n)
    private int spawnLeastInterval { set; get; }

    public float areaSize { get; private set; }

    // map info
    //private Ship[,] grid;
    private Ship selectedShip;

    private ShipSymbol[,] grid { get; set; }
    private List<Ship> ShipsInFieldList { get; set; }

    private MoveShipController controller;

    private void Start()
    {
        mapSize = new Vector2Int(mapLayout.mapSize.x, mapLayout.mapSize.y);
        //grid = new Ship[mapSize.x, mapSize.y];
        grid = new ShipSymbol[mapSize.x, mapSize.y]; // new
        ShipsInFieldList = new List<Ship>(); // new

        spawnLeastInterval = mapLayout.spawnLeastInterval;
        areaSize = mapLayout.areaSize;
        controller = GetComponent<MoveShipController>();

        fixedFogManager.SetFixedFogBlock(null);
    }

    public Ship GetSelectedShip()
    {
        return selectedShip;
    }

    public void SpawnShipRandomCoord(GameObject prefab, Team team)
    {
        // 좌표 정하기

        GameObject shipObj = (GameObject)Instantiate(prefab);
        Ship ship = shipObj.GetComponent<Ship>();
        // Debug.Log(ship.name + ": " + ship);
        ship.team = team;
        ship.Init();

        //List<Vector2Int> temp = ship.GetPosibleShipSpawnCoordsList(this);
        List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(this);

        // deep copy
        ship.shipCoords.Clear();
        //ship.shipCoords = temp.ConvertAll(o => new Vector2Int(o.x, o.y));
        ship.shipCoords = temp.ConvertAll(o => new Vector3Int(o.x, o.y, o.z));

        // new
        ShipsInFieldList.Add(ship);

        for (int i = 0; i < ship.shipCoords.Count; i++)
        {
            //new
            grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, team);

            //grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = ship;
            Debug.Log(prefab.name + "(relative pos): " + ship.shipCoords[i]);
        }

        ship.shipCenterPosition = ship.GetShipCenterPositionFromCoord(ship.shipCoords, this);

        Vector3 pos = ship.shipCenterPosition;
        Debug.Log(prefab.name + " (real pos): " + pos);
        ship.transform.position = pos;

        ship.transform.parent = shipHolder.transform;
        // new code
        ship.transform.localScale = new Vector3(1, 1, 1);
    }

    private Ship GetShipOnArea(Vector2Int Coord)
    {
        //TODO: 조건 수정 필요

        if (grid[Coord.x, Coord.y] != ShipSymbol.NoShip)
        {
            return GetShipBySymbol(grid[Coord.x, Coord.y]);
            //return grid[Coord.x, Coord.y]
        }
        else
        {
            return null;
        }
    }

    public void MoveShip(DirectionType dirType, int amount)
    {
        if (selectedShip == null)
            return;

        bool canMove = selectedShip.CheckAvailableToMove(dirType, amount, mapLayout.mapSize);

        // unavailable to move 
        if (!canMove)
        {
            Debug.Log(selectedShip.name + " can't go there!");
            return;
        }


        Transform oldTransform = selectedShip.transform;
        
        selectedShip.MoveShipInCoord(dirType, amount, this);
        selectedShip.MoveShipInPosition(this);
        selectedShip.MoveShipInField(oldTransform, selectedShip.shipCenterPosition); ;
    }

    public void UpdateShipOnGrid(List<Vector3Int> oldCoords, List<Vector3Int> newCoords, Ship ship)
    {
        // 기존 grid의 ship null 값으로 변경
        for (int i = 0; i < oldCoords.Count; i++)
        {
            grid[oldCoords[i].x, oldCoords[i].y] = ShipSymbol.NoShip;
        }

        for (int i = 0; i < newCoords.Count; i++)
        {
            grid[newCoords[i].x, newCoords[i].y] = ship.Symbol;
        }
    }

    public bool CheckIsShipNear(Vector3Int coord)
    {
        int moveX = -1;
        int moveY = -1;

        for (int i = 0; i < spawnLeastInterval; i++)
        {
            for (int j = 0; j < spawnLeastInterval; j++)
            {
                int x = coord.x + moveX + i;
                int y = coord.y + moveY + j;

                // 배열 bound 확인
                if (x < 0 || y < 0 || x > mapSize.x - 1 || y > mapSize.y - 1)
                {
                    continue;
                }
                else
                {
                    if (grid[x, y] != ShipSymbol.NoShip)
                        return false;
                }
            }
        }

        return true;
    }

    public bool SetSelectedShip(ShipSymbol s)
    {
        // grid 탐색 - 비효율...
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == ShipSymbol.NoShip)
                    continue;
                if (grid[i, j] == s)
                {
                    selectedShip = GetShipBySymbol(s);
                    Debug.Log(selectedShip + " is selected");
                    return true;
                }
            }
        }

        return false;
    }

    public void AttackCoord(Vector2Int coord)
    {
        selectedShip = GetShipOnArea(coord);
        if (selectedShip != null)
        {
            for (int i = 0; i < selectedShip.shipCoords.Count; i++)
            {
                if (selectedShip.shipCoords[i].x == coord.x && selectedShip.shipCoords[i].y == coord.y)
                    selectedShip.DamageShip(i, this);
            }
        }

    }

    /*@param
     */
    private Ship GetShipBySymbol(ShipSymbol s)
    {
        for (int i = 0; i < ShipsInFieldList.Count; i++)
        {
            if (ShipsInFieldList[i].Symbol == s)
            {
                return ShipsInFieldList[i];
            }
        }
        Debug.Log("There is no Ship: " + s);
        return null;
    }
}
