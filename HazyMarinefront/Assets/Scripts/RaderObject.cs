using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Connection;
using UnityEngine;
using UnityEngine.UI;
using MLAPI.Messaging;

public class RaderObject : MonoBehaviour
{
    public GameObject radar;

    public GameObject DotPrefab;

    public ShipType shiptype;

    public Vector3 standard;
    public List<ShipOnRadar> nearShips = new List<ShipOnRadar>();

    public List<GameObject> DotList = new List<GameObject>();

    public Team team;

    public Vector3 RadarPos;

    public bool RevealDots;


    public List<ShipOnRadar> nearShipsClient = new List<ShipOnRadar>();
    public Vector3 standardClient;

    private void Start()
    {
        RevealDots = true;
    }

    public void GetData()
    {
        nearShips.Clear();
        nearShipsClient.Clear();

        team = GameObject.Find("EventSystem").GetComponent<ShipControlEventListener>().team;
        
        foreach(var s in GetMap().ShipsInFieldList)
        {
            if (s.team == team && s.shipType == shiptype)
            {
                standard = s.shipCenterPosition;
            }
            else
            {
                nearShips.Add(new ShipOnRadar(s.shipCenterPosition, s.team));
            }

            if (s.team != team && s.shipType == shiptype)
            {
                standardClient = s.shipCenterPosition;
            }
            else
            {
                nearShipsClient.Add(new ShipOnRadar(s.shipCenterPosition, s.team));
            }
        }
    }

    public void ShowRadar()
    {
        DestroyDots();

        DotList.Clear();

        List<ShipOnRadar> l = GetNearShipList();

        foreach (var s in l)
        {
            GameObject g = (GameObject)Instantiate(DotPrefab);
            Vector3 v = GetRelativePos(s.centerPos);

            g.transform.position =
                new Vector3(RadarPos.x + v.x, RadarPos.y + 0.1f, RadarPos.z + v.z);
            g.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            MaterialSetter.ChangeColor(
                g.GetComponent<Renderer>().material,
                (s.team == Team.ATeam) ? MapLayout.aShipDotColor : MapLayout.bShipDotColor
                );

            DotList.Add(g);

            //ShowDotServerRpc(g.transform.position, true);
            //ShowDotClientRpc(g.transform.position, s.team==Team.ATeam);
        }
    }

    private Vector3 GetRelativePosClient(Vector3 pos)
    {
        float x = pos.x - standardClient.x;
        float z = pos.z - standardClient.z;

        return new Vector3(
            x * MapLayout.radarDistanceConst,
            this.transform.position.y + 0.2f,
            z * MapLayout.radarDistanceConst);
    }


    [ClientRpc]
    private void DestroyDotsClientRpc(int type)
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        PlayManager.DestroyDotClientRpc(type);
    }

    [ClientRpc]
    private void ShowDotClientRpc(Vector3 position, bool v)
    {
        //if (NetworkManager.Singleton.IsServer) { return; }

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        PlayManager.SpawnDotClientRpc(position, v, (int)shiptype);
    }

    public void DestroyDots()
    {
        foreach(var d in DotList)
        {
            Destroy(d);
        }
    }

    private Vector3 GetRelativePos(Vector3 pos)
    {
        float x = pos.x - standard.x;
        float z = pos.z - standard.z;

        return new Vector3(
            x * MapLayout.radarDistanceConst,
            this.transform.position.y + 0.2f,
            z * MapLayout.radarDistanceConst);
    }

    public List<ShipOnRadar> GetNearShipList()
    {
        List<ShipOnRadar> t = new List<ShipOnRadar>();

        foreach (var s in nearShips)
        {
            if (Vector3.Distance(s.centerPos, standard) <= MapLayout.radarValidDistance)
            {
                t.Add(new ShipOnRadar(s.centerPos, s.team));
            }
        }

        return t;
    }

    public void RefreshDots()
    {
        GetData();
        ShowRadar();
        showRadarClient();
    }

    private void showRadarClient()
    {
        DestroyDotsClientRpc((int)shiptype);

        List<ShipOnRadar> l = GetNearShipListClient();

        foreach (var s in l)
        {
            Vector3 v = GetRelativePosClient(s.centerPos);

            Vector3 c = new Vector3(
                RadarPos.x + v.x, RadarPos.y + 0.1f, RadarPos.z + v.z
                );
            ShowDotClientRpc(c, s.team == Team.ATeam);
        }

    }

    private List<ShipOnRadar> GetNearShipListClient()
    {
        List<ShipOnRadar> t = new List<ShipOnRadar>();

        foreach (var s in nearShipsClient)
        {
            if (Vector3.Distance(s.centerPos, standardClient) <= MapLayout.radarValidDistance)
            {
                t.Add(new ShipOnRadar(s.centerPos, s.team));
            }
        }

        return t;
    }

    public void SetShiptype(ShipType type)
    {
        shiptype = type;
    }

    public void SetRadarPos(Vector3 pos)
    {
        RadarPos = pos;
    }

    public void SetRevealDots(bool show)
    {
        RevealDots = show;
        if (!show) DestroyDots();
    }

    private Map GetMap()
    {
        ulong localClientId = NetworkManager.Singleton.ServerClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return null;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return null;
        }

        return PlayManager.MapInstance.GetComponent<Map>();
    }
}
