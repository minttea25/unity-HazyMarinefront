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


    public Vector2Int mapSize { get; set; }

    // 배치 가능 주변 탐색범위 (n*n)
    private int spawnLeastInterval { set; get; }



    public float areaSize { get; set; }

    // map info
    private Ship[,] grid;
    private Ship selectedShip;

    //private MoveShipController controller;


    private void Start()
    {
        mapSize = new Vector2Int(mapLayout.mapSize.x, mapLayout.mapSize.y);
        grid = new Ship[mapSize.x, mapSize.y];
        spawnLeastInterval = mapLayout.spawnLeastInterval;
        areaSize = mapLayout.areaSize;
        //controller = GetComponent<MoveShipController>();
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

        for (int i = 0; i < ship.shipCoords.Count; i++)
        {
            grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = ship;
            Debug.Log(prefab.name + "(relative pos): " + ship.shipCoords[i]);
        }

        ship.shipCenterPosition = ship.GetShipCenterPositionFromCoord(ship.shipCoords, this);

        Vector3 pos = ship.shipCenterPosition;
        Debug.Log(prefab.name + " (real pos): " + pos);
        ship.transform.position = pos;

        // selectedShip = grid[ship.shipCoords[0].x, ship.shipCoords[0].y];

        ship.transform.parent = shipHolder.transform;
        // new code
        ship.transform.localScale = new Vector3(1, 1, 1);
    }


    // 지정 좌표에 실제로 배가 있는지 확인 후 반환(조건 수정 필요)
    private Ship GetShipOnArea(Vector2Int coord)
    {
        //TODO: 조건 수정 필요

        if (grid[coord.x, coord.y] != null)
            return grid[coord.x, coord.y];
        else
            return null;
    }

    public void MoveShip(DirectionType dirType, int amount)
    {
        if (selectedShip == null)
            return;

        bool canMove = selectedShip.checkAvailableToMove(dirType, amount, mapLayout.mapSize);

        // unavailable to move 
        if (!canMove)
        {
            Debug.Log(selectedShip.name + " can't go there!");
            return;
        }


        //controller.shipTransform = selectedShip.transform;
        selectedShip.MoveShipInCoord(dirType, amount, this);
        selectedShip.MoveShipInPosition(this);
        //controller.desPosition = selectedShip.shipCenterPosition;
        //controller.moveNow = true;
        
        selectedShip.MoveShipInField();
    }

    public void UpdateShipOnGrid(List<Vector3Int> oldCoords, List<Vector3Int> newCoords, Ship ship)
    {
        // 기존 grid의 ship null 값으로 변경
        for (int i = 0; i < oldCoords.Count; i++)
        {
            grid[oldCoords[i].x, oldCoords[i].y] = null;
        }

        for (int i = 0; i < newCoords.Count; i++)
        {
            grid[newCoords[i].x, newCoords[i].y] = ship;
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
                    if (grid[x, y] != null)
                        return false;
                }
            }
        }

        return true;
    }

    public bool SetSelectedShip(ShipType type, Team team)
    {
        // grid 탐색 - 비효율...
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == null)
                    continue;
                if (grid[i, j].shipType == type && grid[i, j].team == team)
                {
                    selectedShip = grid[i, j];
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
}
