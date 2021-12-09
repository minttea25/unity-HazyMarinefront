using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMap : MonoBehaviour
{
    public GameObject[] teamAShipPrefabs;
    public GameObject[] teamBShipPrefabs;

    public GameObject shipHolder;

    // 기준 좌표
    public Transform bottomLeftSquareTransform;

    public GameObject fogBlocks;
    public GameObject ExplosionPrefab;
    public GameObject BigExplosionPrefab;
    public GameObject waterSplash;

    public FixedFogManager fixedFogManager;

    public Vector2Int mapSize = new Vector2Int(MapLayout.mapSize.x, MapLayout.mapSize.y);

    // map info
    private Ship selectedShip;
    public Vector2Int selectedCoord;

    [SerializeField] public ShipSymbol[,] grid = new ShipSymbol[MapLayout.mapSize.x, MapLayout.mapSize.y];
    [SerializeField] public List<Ship> ShipsInFieldList = new List<Ship>();

    private void Awake()
    {
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

    private Ship GetShipOnArea(Vector2Int Coord)
    {
        if (grid[Coord.x, Coord.y] != ShipSymbol.NoShip)
        {
            return GetShipBySymbol(grid[Coord.x, Coord.y]);
        }
        else
        {
            return null;
        }

        // ojy code
        //return selectedShip;
    }

    // 이동에 성공 시 true 반환
    public bool MoveShip(DirectionType dirType, int amount)
    {
        if (selectedShip == null || selectedShip.isDestroyed)
            return false;

        bool canMove = selectedShip.CheckAvailableToMove(dirType, amount, MapLayout.mapSize);
        bool collision = false;
        bool mine = false;
        int minehit = -1;
        // unavailable to move 
        if (!canMove)
        {
            Debug.Log(selectedShip.name + " can't go there!");
            return false;
        }

        int[] axisValue = selectedShip.GetDirectionAmount(dirType, amount);
        int xAxis = axisValue[0];
        int yAxis = axisValue[1];

        for (int i = 0; i < selectedShip.shipCoords.Count; i++)
        {
            Debug.Log("count: " + i + "/" + grid[selectedShip.shipCoords[i].x + xAxis, selectedShip.shipCoords[i].y + yAxis]);
            //여러개 동시에 충돌하는 경우 보완 필요 
            if (grid[selectedShip.shipCoords[i].x + xAxis, selectedShip.shipCoords[i].y + yAxis] != ShipSymbol.NoShip &&
                grid[selectedShip.shipCoords[i].x + xAxis, selectedShip.shipCoords[i].y + yAxis] != ShipSymbol.NM &&
                grid[selectedShip.shipCoords[i].x + xAxis, selectedShip.shipCoords[i].y + yAxis] != selectedShip.Symbol)
            {
                Debug.Log("충돌");

                GetComponent<AIManager>().DamageShip(i);
                
                //PlayManager.DamageShipServerRpc(i);

                var loc = new Vector2Int(selectedShip.shipCoords[i].x + xAxis, selectedShip.shipCoords[i].y + yAxis);

                GetComponent<AIManager>().AttackCoord(loc.x, loc.y);

                //PlayManager.AttackCoordServerRpc(loc.x, loc.y);

                collision = true;
            }
            else if (grid[selectedShip.shipCoords[i].x + xAxis, selectedShip.shipCoords[i].y + yAxis] == ShipSymbol.NM)
            {
                minehit = i;
                mine = true;
            }
        }

        if (collision)
            return false;

        Transform oldTransform = selectedShip.transform;

        selectedShip.MoveAIShipInCoord(dirType, amount, this);
        selectedShip.MoveAIShipInPosition(this);
        selectedShip.MoveShipInField(oldTransform, selectedShip.shipCenterPosition); ;

        if (mine && minehit != -1)
        {
            
            GetComponent<AIManager>().DamageShip(minehit);
            //PlayManager.DamageShipServerRpc(minehit);
        }

        return true;
    }

    public void UpdateShipOnGrid(List<Vector3Int> oldCoords, List<Vector3Int> newCoords, Ship ship)
    {
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

                // 배열 bound 확인
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
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == ShipSymbol.NoShip)
                    continue;
                if (grid[i, j] == s)
                {
                    Ship t = GetShipBySymbol(s);
                    if (t == null)
                    {
                        continue;
                    }
                    else
                    {
                        selectedShip = t;
                        Debug.Log(selectedShip + " is selected");
                        return true;
                    }
                }
            }
        }

        return false;
    }

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

    public ShipSymbol GetShipSymbolByCoords(Vector2Int coord)
    {
        return grid[coord.x, coord.y];
    }


    public bool IsThereLeftShip(Team team)
    {
        bool flag = false;

        foreach (var s in ShipsInFieldList)
        {
            if (s.team == team)
            {
                flag = true;
                break;
            }
        }

        return flag;
    }

    public bool RemoveShipInList(ShipSymbol s)
    {
        foreach (var ship in ShipsInFieldList)
        {
            if (ship.Symbol == s)
            {
                ShipsInFieldList.Remove(ship);
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        //Debug.Log("SELECT: " + selectedShip.Symbol);
    }
}
