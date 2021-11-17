using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;

public class PlayManager : NetworkBehaviour
{
    [SerializeField] public NetworkObject[] teamAShipPrefabs;
    [SerializeField] public NetworkObject[] teamBShipPrefabs;

    [SerializeField] public NetworkObject MapPrfab;
    [SerializeField] public NetworkObject FogPrefab;
    [SerializeField] public NetworkObject ExplosionPrefab;
    [SerializeField] public NetworkObject BigExplosionPrefab;
    [SerializeField] public NetworkObject WaterSplashPrefab;

    [SerializeField] public NetworkObject MapInstance { get; private set; }

    //public bool AttackMode { get; set; }

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) { return; }

        if (NetworkManager.Singleton.IsServer)
        {
            SpawnMapServerRpc();

            //SpawnShipRandomCoordServerRpc();

            SpawnFogServerRpc();

        }
        else
        {
            Debug.Log("Client");
        }
    }

    [ServerRpc]
    internal void ClearFogTestServerRpc()
    {
        NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>().fixedFogManager.ClearFogTest();
    }

    [ServerRpc]
    private void SpawnMapServerRpc()
    {
        MapInstance = Instantiate(
            MapPrfab,
            new Vector3(0, 0, 0),
            Quaternion.identity);
        //MapInstance.SpawnWithOwnership(OwnerClientId);
        MapInstance.Spawn();
    }

    [ServerRpc]
    public void SpawnShipRandomCoordServerRpc()
    {
        for (int k = 0; k < teamAShipPrefabs.Length; k++)
        {
            NetworkObject shipInstance = Instantiate(
            teamAShipPrefabs[k],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            //shipInstance.SpawnWithOwnership(OwnerClientId);
            shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.ATeam;
            ship.Init();

            List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(MapInstance.GetComponent<Map>());

            // deep copy
            ship.shipCoords.Clear();
            ship.shipCoords = temp.ConvertAll(o => new Vector3Int(o.x, o.y, o.z));


            MapInstance.GetComponent<Map>().ShipsInFieldList.Add(ship);

            for (int i = 0; i < ship.shipCoords.Count; i++)
            {
                MapInstance.GetComponent<Map>().grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, ship.team);
            }

            ship.shipCenterPosition = ship.GetShipCenterPositionFromCoord(ship.shipCoords, MapInstance.GetComponent<Map>());

            Vector3 pos = ship.shipCenterPosition;
            ship.transform.position = pos;

            ship.transform.parent = MapInstance.GetComponent<Map>().shipHolder.transform;
            ship.transform.localScale = new Vector3(1, 1, 1);
        }

        for (int k = 0; k < teamBShipPrefabs.Length; k++)
        {
            NetworkObject shipInstance = Instantiate(
            teamBShipPrefabs[k],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            //shipInstance.SpawnWithOwnership(OwnerClientId);
            shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.BTeam;
            ship.Init();

            List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(MapInstance.GetComponent<Map>());


            // deep copy
            ship.shipCoords.Clear();
            ship.shipCoords = temp.ConvertAll(o => new Vector3Int(o.x, o.y, o.z));
            MapInstance.GetComponent<Map>().ShipsInFieldList.Add(ship);

            for (int i = 0; i < ship.shipCoords.Count; i++)
            {
                MapInstance.GetComponent<Map>().grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, ship.team);
            }

            ship.shipCenterPosition = ship.GetShipCenterPositionFromCoord(ship.shipCoords, MapInstance.GetComponent<Map>());

            Vector3 pos = ship.shipCenterPosition;
            ship.transform.position = pos;

            ship.transform.parent = MapInstance.GetComponent<Map>().shipHolder.transform;
            ship.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    [ServerRpc]
    private void SpawnFogServerRpc()
    {
        float x = MapInstance.GetComponent<Map>().bottomLeftSquareTransform.transform.position.x;
        float y = MapInstance.GetComponent<Map>().bottomLeftSquareTransform.transform.position.y + MapLayout.oceanFogInterval;
        float z = MapInstance.GetComponent<Map>().bottomLeftSquareTransform.transform.position.z;

        for (int i = 0; i < MapLayout.mapSize.x; i++)
        {
            for (int j = 0; j < MapLayout.mapSize.y; j++)
            {
                float vx = x + MapLayout.areaSize * (i + 0.5f);
                float vz = z + MapLayout.areaSize * (j + 0.5f);
                NetworkObject FogInstance = Instantiate(
                    FogPrefab,
                    new Vector3(vx, y, vz),
                    Quaternion.identity);
                //FogInstance.SpawnWithOwnership(OwnerClientId);
                FogInstance.Spawn();

                FixedFog fog = FogInstance.GetComponent<FixedFog>();
                MapInstance.GetComponent<Map>().fixedFogManager.fixedFogGrid[i, j] = fog;
                fog.transform.parent = MapInstance.GetComponent<Map>().fogBlocks.transform;
            }
        }
    }

    [ServerRpc]
    public void SetMoveShipServerRpc(ShipSymbol s, DirectionType dirType, int amount)
    {
        Debug.Log("SetMove: "+this.GetHashCode());

        bool exist = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>().SetSelectedShip(s);
        if (exist)
        {

            bool moved = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>().MoveShip(dirType, amount);
            Debug.Log("MOVED: " + moved);
        }
    }

    // for only clicking fog
    [ServerRpc]
    public void AttackServerRpc(int x, int y)
    {
        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();

        map.fixedFogManager.ClearFog(new Vector2Int(x, y));

        //map.AttackCoord(new Vector2Int(x, y));
        AttackCoordServerRpc(x, y);
    }

    [ServerRpc]
    public void AttackCoordServerRpc(int x, int y)
    {
        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();

        bool exist = map.SetSelectedShip(map.GetShipSymbolByCoords(new Vector2Int(x, y)));
        if (exist)
        {
            for (int i = 0; i < map.GetSelectedShip().shipCoords.Count; i++)
            {
                if (map.GetSelectedShip().shipCoords[i].x == x && map.GetSelectedShip().shipCoords[i].y == y)
                {
                    //map.GetSelectedShip().DamageShip(i);
                    DamageShipServerRpc(i);
                    Debug.Log("damaged ship : Vector(" + x + ", " + y + ")");
                }
            }
        }
        else
        {
            Vector3 loc = new Vector3((float)(x - 4.5), 1.5f, (float)(y - 4.5));
            Instantiate(WaterSplashPrefab, loc, Quaternion.identity).Spawn();
        }
    }

    [ServerRpc]
    public void DamageShipServerRpc(int index)
    {
        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();
        Ship ship = map.GetSelectedShip();

        if (ship.shipCoords[index].z == 0)
        {
            ship.shipHealth--;
        }
        if (ship.shipHealth == 0)
        {
            if (!ship.isDestroyed)
            {
                ship.isDestroyed = true;
                Instantiate(BigExplosionPrefab, ship.transform.position, Quaternion.identity).Spawn();
                Debug.Log("ship destroyed");

            }
        }
        else
        {
            var curCoord = new Vector3((float)(ship.shipCoords[index].x - 4.5), 2f, (float)(ship.shipCoords[index].y - 4.5));
            Instantiate(ExplosionPrefab, curCoord, Quaternion.identity).Spawn();
        }
        ship.shipCoords[index] = new Vector3Int(ship.shipCoords[index].x, ship.shipCoords[index].y, ship.shipCoords[index].z + 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

}
