using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    public List<Vector3Int> oldShipCoords { get; set; }
    public List<Vector3Int> shipCoords { get; set; }
    public Vector3 shipCenterPosition { get; set; }
    public Team team { get; set; }

    public ShipType shipType { get; set; }

    public bool isMainShip { get; set; }

    public bool visibility { get; set; }

    public int shipSizeX { get; protected set; }
    public int shipSizeY { get; protected set; }

    public int shipHealth { get; set; }
    public bool isDestroyed { get; set; }

    // 로직에 필요 요소
    public List<Vector2Int> availableArea;
    private MaterialSetter materialSetter;
    private MoveShipController controller;

    public abstract Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map);
    public abstract List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map);


    public void Init()
    {
        oldShipCoords = new List<Vector3Int>();
        shipCoords = new List<Vector3Int>();
        availableArea = new List<Vector2Int>();
        materialSetter = GetComponent<MaterialSetter>();
        controller = GetComponent<MoveShipController>();

        if (shipType == ShipType.MainShip)
            isMainShip = true;
        visibility = isMainShip;
    }

    public void SetMaterial(Material shipMaterial)
    {
        materialSetter.SetMaterial(shipMaterial);
    }

    // 실제 필드에서 옮기기
    public void MoveShipInField()
    {
        controller.MoveTo(transform, shipCenterPosition);
    }

    // 해당 좌표로 움직일 수 있는지 여부
    public bool CanMoveTo(Vector2Int coord)
    {
        //TODO: 임시로 true
        return true;
    }

    // 배의 칸수(크기) 반환
    public int getShipSize()
    {
        return shipSizeX * shipSizeY;
    }

    public void MoveShipInCoord(DirectionType dirType, int amount, Map map)
    {
        Debug.Log("MoveShipInCoord");

        int[] axisValue = GetDirectionAmount(dirType, amount);
        int xAxis = axisValue[0];
        int yAxis = axisValue[1];

        Debug.Log(xAxis + ", " + yAxis);



        List<Vector3Int> newCoords = new List<Vector3Int>();
        for (int i = 0; i < shipCoords.Count; i++)
        {
            newCoords.Add(new Vector3Int(shipCoords[i].x + xAxis, shipCoords[i].y + yAxis, shipCoords[i].z));
            Debug.Log(newCoords[i]);
        }

        ChangeOldNewCoords(newCoords);

        map.UpdateShipOnGrid(oldShipCoords, shipCoords, this);
    }

    public void MoveShipInPosition(Map map)
    {
        // 움직이고 난 후의 coord에 대한 중앙 position 값 얻기
        shipCenterPosition = GetShipCenterPositionFromCoord(shipCoords, map);
    }

    internal bool checkAvailableToMove(DirectionType dirType, int amount, Vector2Int mapSize)
    {
        float x = 0;
        float y = 0;

        // check negative coords or over map size(coord)
        // 일단 움직일 수는 있으니까 배 충돌 고려하지 않음
        switch (dirType)
        {
            case DirectionType.Front:
                y += amount;
                break;
            case DirectionType.Back:
                y -= amount;
                break;
            case DirectionType.Left:
                x -= amount;
                break;
            case DirectionType.Right:
                x += amount;
                break;
            default:
                return false;
        }

        foreach (var coords in shipCoords)
        {
            float xx = coords.x + x;
            float yy = coords.y + y;

            if (xx < 0 || xx > mapSize.x - 1)
            {
                return false;
            }

            if (yy < 0 || yy > mapSize.y - 1)
            {
                return false;
            }
        }

        return true;
    }

    public void DamageShip(int index, Map map)
    {
        if (shipCoords[index].z == 0)
            shipHealth--;
        if (shipHealth == 0)
            this.isDestroyed = true;
        shipCoords[index] = new Vector3Int(shipCoords[index].x, shipCoords[index].y, shipCoords[index].z + 1);
        //prefab 으로 배 색상 변경 (붉은색 혹은 검은색/피탄 리소스 있으면 적용해보기)
        //이펙트 추가?

    }

    private int[] GetDirectionAmount(DirectionType dirType, int amount)
    {
        int xAxis = 0;
        int yAxis = 0;
        switch (dirType)
        {
            case DirectionType.Right:
                xAxis = amount;
                break;
            case DirectionType.Left:
                xAxis = amount * -1;
                break;
            case DirectionType.Front:
                yAxis = amount;
                break;
            case DirectionType.Back:
                yAxis = amount * -1;
                break;
        }
        int[] a = { xAxis, yAxis };

        return a;
    }

    // util list를 target에 복사
    private void ChangeOldNewCoords(List<Vector3Int> newCoords)
    {


        oldShipCoords.Clear();
        for (int i = 0; i < shipCoords.Count; i++)
        {
            oldShipCoords.Add(new Vector3Int(shipCoords[i].x, shipCoords[i].y, shipCoords[i].z));
        }

        shipCoords.Clear();
        for (int i = 0; i < newCoords.Count; i++)
        {
            shipCoords.Add(new Vector3Int(newCoords[i].x, newCoords[i].y, newCoords[i].z));
        }
    }
}
