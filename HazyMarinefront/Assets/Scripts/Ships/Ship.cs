using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public abstract class Ship : NetworkBehaviour
{
    public List<Vector3Int> oldShipCoords { get; set; }
    public List<Vector3Int> shipCoords { get; set; }
    public Vector3 shipCenterPosition { get; set; }
    public Team team { get; set; }

    public ShipType shipType { get; set; }

    public int shipSizeX { get; protected set; }
    public int shipSizeY { get; protected set; }

    public ShipSymbol Symbol { get; set; }

    public int shipHealth { get; set; }
    public bool isDestroyed { get; set; }

    private bool RevealedToA { get; set; }
    private bool RevealedToB { get; set; }

    // ������ �ʿ� ���
    public List<Vector2Int> availableArea;
    private MaterialSetter materialSetter;
    private MoveShipController controller;

    public NetworkObject Explosion;
    public NetworkObject BigExplosion;

    public abstract Vector3 GetShipCenterPositionFromCoord(List<Vector3Int> coords, Map map);
    public abstract List<Vector3Int> GetPosibleShipSpawnCoordsList(Map map);

    public abstract void ActivateAbility();

    public void Init()
    {
        oldShipCoords = new List<Vector3Int>();
        shipCoords = new List<Vector3Int>();
        availableArea = new List<Vector2Int>();
        materialSetter = GetComponent<MaterialSetter>();
        controller = GetComponent<MoveShipController>();

        Symbol = MapLayout.GetSymbolByShiptypeTeam(this.shipType, team);

        Material m = GetMaterial(team);


        if (m != null)
        {
            // ���� ������ ���� �ӽ� �ڵ� (2���� �޽��� �Ǿ��ִ� �������̱� ������ ��� ����)
            // �������� �θ� ������Ʈ�� mesh renderer�� meterial�� �ٲٴ� ���ιǷ� ���� �Ʒ� �ڵ忡 ���� �ð������� ������ ������� ����
            // ���� �ƿ� �� A, B�� ����� ���� �ٸ� ���������� ����
            SetMaterial(m);
        }
    }

    private Material GetMaterial(Team team)
    {
        Material m = null;
        String m1 = "test/Team1Material";
        String m2 = "test/Team2Material";

        if (team == Team.ATeam)
        {
            m = Resources.Load<Material>(m1);
        }
        else
        {
            m = Resources.Load<Material>(m2);
        }

        if (m != null)
        {
            return m;
        } else
        {
            Debug.Log("Can not find " + m1 + " or " + m2 + ". Check the files in asset folder.");
            return null;
        }
    }

    public void SetMaterial(Material shipMaterial)
    {
        materialSetter.SetMaterial(shipMaterial);
    }

    // ���� �ʵ忡�� �ű��
    public void MoveShipInField(Transform oldTransform, Vector3 desPosition)
    {
        controller.shipTransform = oldTransform;
        controller.desPosition = desPosition;
        controller.moveFlag = true;
        controller.MoveTo(transform, shipCenterPosition);
    }

    // �ش� ��ǥ�� ������ �� �ִ��� ����
    public bool CanMoveTo(Vector2Int coord)
    {
        //TODO: �ӽ÷� true
        return true;
    }

    // ���� ĭ��(ũ��) ��ȯ
    public int GetShipSize()
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
        // �����̰� �� ���� coord�� ���� �߾� position �� ���
        shipCenterPosition = GetShipCenterPositionFromCoord(shipCoords, map);
    }

    internal bool CheckAvailableToMove(DirectionType dirType, int amount, Vector2Int mapSize)
    {
        float x = 0;
        float y = 0;

        // check negative coords or over map size(coord)
        // �ϴ� ������ ���� �����ϱ� �� �浹 ������� ����
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

    public void DamageShip(int index)
    {
        if (shipCoords[index].z == 0)
            shipHealth--;
        if (shipHealth == 0)
        {
            if (!this.isDestroyed)
            {
                this.isDestroyed = true;
                Instantiate(BigExplosion, gameObject.transform.position, Quaternion.identity).Spawn();
                //Destroy(GameObject.Find("BigExplosion(Clone)"), 1f);
                Debug.Log("ship destroyed");
            }
            
        }
        else
        {
            var curCoord = new Vector3((float)(shipCoords[index].x - 4.5), 2f, (float)(shipCoords[index].y - 4.5));
            NetworkObject explosionIst = Instantiate(Explosion, curCoord, Quaternion.identity);
            explosionIst.Spawn();
            //Debug.Log("coord: " + gameObject.transform.position);
            //Destroy(GameObject.Find("SmallExplosion(Clone)"), 1f);
        }
        shipCoords[index] = new Vector3Int(shipCoords[index].x, shipCoords[index].y, shipCoords[index].z + 1);
        //prefab ���� �� ���� ���� (������ Ȥ�� ������/��ź ���ҽ� ������ �����غ���)
        //����Ʈ �߰�?

    }

    public int[] GetDirectionAmount(DirectionType dirType, int amount)
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

    // ��ġ�� ���� �Ǿ��� ��� ���� ����
    public void SetRevealedState(Team ToTeam, bool visible)
    {
        switch (ToTeam) {
            case Team.ATeam:
                RevealedToA = visible;
                break;
            case Team.BTeam:
                RevealedToB = visible;
                break;
            default:
                return;
        }
    }

}
