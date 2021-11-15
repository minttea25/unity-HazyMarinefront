using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using Random = UnityEngine.Random;

public class Map : NetworkBehaviour
{
    public NetworkObject[] teamAShipPrefabs;
    public NetworkObject[] teamBShipPrefabs;
    /// <summary>
    /// /////////////
    /// </summary>
    public GameObject shipHolder;

    // ���� ��ǥ
    public Transform bottomLeftSquareTransform;

    public GameObject fogBlocks;
    public FixedFogManager fixedFogManager;

    public Vector2Int mapSize = new Vector2Int(MapLayout.mapSize.x, MapLayout.mapSize.y);

    // map info
    private Ship selectedShip;

    [SerializeField] public ShipSymbol[,] grid = new ShipSymbol[MapLayout.mapSize.x, MapLayout.mapSize.y];
    [SerializeField] public List<Ship> ShipsInFieldList = new List<Ship>();

    private void Awake()
    {
        //fixedFogManager.SetFixedFogBlock(null);
        SetShipSymbolDefault();

    }
    private void SetShipSymbolDefault()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = ShipSymbol.NoShip;
            }
        }
    }

    public Ship GetSelectedShip()
    {
        return selectedShip;
    }

    public void SpawnShipRandomCoord(GameObject prefab, Team team)
    {
        // ��ǥ ���ϱ�

        GameObject shipObj = (GameObject)Instantiate(prefab);
        Ship ship = shipObj.GetComponent<Ship>();
        // Debug.Log(ship.name + ": " + ship);
        ship.team = team;
        ship.Init();

        List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(this);

        // deep copy
        ship.shipCoords.Clear();
        ship.shipCoords = temp.ConvertAll(o => new Vector3Int(o.x, o.y, o.z));

        // new
        ShipsInFieldList.Add(ship);

        for (int i = 0; i < ship.shipCoords.Count; i++)
        {
            //new
            Debug.Log(ship.shipCoords.Count);
            grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, team);

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
        //TODO: ���� ���� �ʿ�

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

    // �̵��� ���� �� true ��ȯ
    public bool MoveShip(DirectionType dirType, int amount)
    {
        if (selectedShip == null)
            return false;

        bool canMove = selectedShip.CheckAvailableToMove(dirType, amount, MapLayout.mapSize);

        // unavailable to move 
        if (!canMove)
        {
            Debug.Log(selectedShip.name + " can't go there!");
            return false;
        }


        Transform oldTransform = selectedShip.transform;
        
        selectedShip.MoveShipInCoord(dirType, amount, this);
        selectedShip.MoveShipInPosition(this);
        selectedShip.MoveShipInField(oldTransform, selectedShip.shipCenterPosition); ;

        return true;
    }

    public void UpdateShipOnGrid(List<Vector3Int> oldCoords, List<Vector3Int> newCoords, Ship ship)
    {
        // ���� grid�� ship null ������ ����
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

        for (int i = 0; i < MapLayout.spawnLeastInterval; i++)
        {
            for (int j = 0; j < MapLayout.spawnLeastInterval; j++)
            {
                int x = coord.x + moveX + i;
                int y = coord.y + moveY + j;

                // �迭 bound Ȯ��
                if (x < 0 || y < 0 || x > MapLayout.mapSize.x - 1 || y > MapLayout.mapSize.y - 1)
                {
                    continue;
                }
                else
                {
                    if (grid[x, y] != ShipSymbol.NoShip)
                    {
                        return true;
                    }

                }
            }
        }

        return false;
    }

    public bool SetSelectedShip(ShipSymbol s)
    {
        // grid Ž�� - ��ȿ��...
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
    public Ship GetShipBySymbol(ShipSymbol s)
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
