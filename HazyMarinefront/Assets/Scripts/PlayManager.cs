using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using MLAPI.NetworkVariable;
using System;

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

    [SerializeField] private Dictionary<ShipSymbol, NetworkObject> SymbolNetworkInstance = new Dictionary<ShipSymbol, NetworkObject>();

    [SerializeField] public NetworkVariableBool AMainShipVisibility = new NetworkVariableBool(false);
    [SerializeField] public NetworkVariableBool ASubShip1Visibility = new NetworkVariableBool(false);
    [SerializeField] public NetworkVariableBool ASubShip2Visibility = new NetworkVariableBool(false);
    [SerializeField] public NetworkVariableBool ASubShip3Visibility = new NetworkVariableBool(false);

    [SerializeField] public NetworkVariableBool BMainShipVisibility = new NetworkVariableBool(false);
    [SerializeField] public NetworkVariableBool BSubShip1Visibility = new NetworkVariableBool(false);
    [SerializeField] public NetworkVariableBool BSubShip2Visibility = new NetworkVariableBool(false);
    [SerializeField] public NetworkVariableBool BSubShip3Visibility = new NetworkVariableBool(false);

    [SerializeField] public NetworkVariableBool IsShipSpawned = new NetworkVariableBool(false);

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
            shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.ATeam;
            ship.visibility = false;
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

            shipInstance.tag = ship.Symbol.ToString();

            SymbolNetworkInstance.Add(ship.Symbol, shipInstance);

            ChangeAlphaValueShip(ship.Symbol, MapLayout.spawnedShipAlphaValue);
        }

        for (int k = 0; k < teamBShipPrefabs.Length; k++)
        {
            NetworkObject shipInstance = Instantiate(
            teamBShipPrefabs[k],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.BTeam;
            ship.visibility = false;
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

            shipInstance.tag = ship.Symbol.ToString();

            SymbolNetworkInstance.Add(ship.Symbol, shipInstance);

            ChangeAlphaValueShip(ship.Symbol, MapLayout.spawnedShipAlphaValue);
        }

        IsShipSpawned.Value = true;
    }

    public Ship createShip(int num, bool shipType)
    {
        if (shipType)
        {
            NetworkObject shipInstance = Instantiate(
            teamAShipPrefabs[num],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.ATeam;
            ship.Init();

            return ship;
        }
        else
        {
            NetworkObject shipInstance = Instantiate(
            teamBShipPrefabs[num],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.BTeam;
            ship.Init();

            return ship;
        }
    }

    public void placeShip(Ship ship, List<Vector3Int> coords)
    {
        ship.shipCoords.Clear();
        ship.shipCoords = coords.ConvertAll(o => new Vector3Int(o.x, o.y, o.z));


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
        Debug.Log("SetMove: " + this.GetHashCode());

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
        AttackCoordServerRpc(x, y);
    }

    [ServerRpc]
    public void AttackCoordServerRpc(int x, int y)
    {
        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();

        bool exist = map.SetSelectedShip(map.GetShipSymbolByCoords(new Vector2Int(x, y)));
        if (exist)
        { 
            // ERROR
            // 아래 구문에서 동일한 지점 공격시 nullpoint exception 발생!!
            // 충돌 시에도 같은 현상 발생

            for (int i = 0; i < map.GetSelectedShip().shipCoords.Count; i++)
            {
                if (map.GetSelectedShip().shipCoords[i].x == x && map.GetSelectedShip().shipCoords[i].y == y)
                {
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

                // list 에서 파괴된 배 삭제
                bool removed = map.RemoveShipInList(ship.Symbol);
                if (removed)
                {
                    Debug.Log(ship.Symbol + " is removed in list");
                }
                else
                {
                    Debug.Log("Remove ship in list error...");
                }

                if (NetworkManager.Singleton.IsServer)
                {
                    Destroy(GameObject.FindGameObjectWithTag(ship.Symbol.ToString()));
                }
                else
                {
                    SymbolNetworkInstance.TryGetValue(ship.Symbol, out NetworkObject obj);
                    obj.Despawn();
                }
            }
        }
        else
        {
            var curCoord = new Vector3((float)(ship.shipCoords[index].x - 4.5), 2f, (float)(ship.shipCoords[index].y - 4.5));
            Instantiate(ExplosionPrefab, curCoord, Quaternion.identity).Spawn();

            // reveal ship
            if (!ship.visibility)
            {
                ShipSymbol ss = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, ship.team);
                Debug.Log("ss = " + ss);

                ChangeValueVisiblity(ship.Symbol, true);

                ship.visibility = true;

                Debug.Log("REVEAL SHIP: " + ss);
            }

        }
        ship.shipCoords[index] = new Vector3Int(ship.shipCoords[index].x, ship.shipCoords[index].y, ship.shipCoords[index].z + 1);

        CheckGameOver();
    }

    public void CheckGameOver()
    {
        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();

        bool ATeamShips = map.IsThereLeftShip(Team.ATeam);
        bool BTeamShips = map.IsThereLeftShip(Team.BTeam);

        if (ATeamShips && !BTeamShips)
        {
            WinLoseEvent(1);
        }
        else if (!ATeamShips && BTeamShips)
        {
            WinLoseEvent(2);
        }
        else if (map.ShipsInFieldList.Count == 0)
        {
            WinLoseEvent(0);
        }
    }

    // 0: 무승부
    // 1: ATeam 승리
    // 2: BTeam 승리
    public void WinLoseEvent(int winLose) 
    {
        if (winLose < 0 || winLose > 2)
        {
            Debug.Log("argument error in WinLoseEventServerRpc");
            return;
        }
        else
        {
            ulong localClientId = NetworkManager.Singleton.LocalClientId;

            if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
            {
                return;
            }

            if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
            {
                return;
            }

            // who win?
            TurnManager.SetWinLose(winLose);

            // GameOver Turn
            TurnManager.SetGameState(4);
        }
    }

    [ServerRpc]
    private void ChangeAlphaValueShipInServerServerRpc(ShipSymbol ss, float alphaValue)
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var playManager))
        {
            return;
        }

        playManager.SymbolNetworkInstance.TryGetValue(ss, out NetworkObject shipInstance);

        if (shipInstance == null)
        {
            Debug.Log("NULL VALUE instance - ERROR");
            return;
        }

        MaterialSetter.ChangeAlpha(shipInstance.transform.Find(MapLayout.shipUpperComponentName).GetComponent<Renderer>().material, alphaValue);
        MaterialSetter.ChangeAlpha(shipInstance.transform.Find(MapLayout.shipDownComponentName).GetComponent<Renderer>().material, alphaValue);

    }

    private void OnEnable()
    {
        AMainShipVisibility.OnValueChanged += AMainChanged;
        ASubShip1Visibility.OnValueChanged += ASub1Changed;
        ASubShip2Visibility.OnValueChanged += ASub2Changed;
        ASubShip3Visibility.OnValueChanged += ASub3Changed;

        BMainShipVisibility.OnValueChanged += BMainChanged;
        BSubShip1Visibility.OnValueChanged += BSub1Changed;
        BSubShip2Visibility.OnValueChanged += BSub2Changed;
        BSubShip3Visibility.OnValueChanged += BSub3Changed;

        IsShipSpawned.OnValueChanged += IsShipSpawnedValueChanged;

    }

    private void IsShipSpawnedValueChanged(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        if (NetworkManager.Singleton.IsServer) { return; }

        ChangeAlphaValueShip(ShipSymbol.A0, MapLayout.spawnedShipAlphaValue);
        ChangeAlphaValueShip(ShipSymbol.A1, MapLayout.spawnedShipAlphaValue);
        ChangeAlphaValueShip(ShipSymbol.A2, MapLayout.spawnedShipAlphaValue);
        ChangeAlphaValueShip(ShipSymbol.A3, MapLayout.spawnedShipAlphaValue);

        ChangeAlphaValueShip(ShipSymbol.B0, MapLayout.spawnedShipAlphaValue);
        ChangeAlphaValueShip(ShipSymbol.B1, MapLayout.spawnedShipAlphaValue);
        ChangeAlphaValueShip(ShipSymbol.B2, MapLayout.spawnedShipAlphaValue);
        ChangeAlphaValueShip(ShipSymbol.B3, MapLayout.spawnedShipAlphaValue);
    }

    private void BSub3Changed(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.B3, MapLayout.shipRevealedAlphaValue);
    }

    private void BSub2Changed(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.B2, MapLayout.shipRevealedAlphaValue);
    }

    private void BSub1Changed(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.B1, MapLayout.shipRevealedAlphaValue);
    }

    private void BMainChanged(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.B0, MapLayout.shipRevealedAlphaValue);
    }

    private void ASub3Changed(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.A3, MapLayout.shipRevealedAlphaValue);
    }

    private void ASub2Changed(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.A2, MapLayout.shipRevealedAlphaValue);
    }

    private void ASub1Changed(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.A1, MapLayout.shipRevealedAlphaValue);
    }

    private void AMainChanged(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }
        ChangeAlphaValueShip(ShipSymbol.A0, MapLayout.shipRevealedAlphaValue);
    }

    private void ChangeAlphaValueShip(ShipSymbol ss, float alphaValue)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            ChangeAlphaValueShipInServerServerRpc(ss, alphaValue);
        }
        else
        {
            ChangeAlphaValueShipInClient(ss, alphaValue);
        }
    }

    private void ChangeAlphaValueShipInClient(ShipSymbol ss, float alphaValue)
    {
        if (NetworkManager.Singleton.IsServer) { return; }

        string name = "";
        switch (ss)
        {
            case ShipSymbol.A0:
                name = MapLayout.aMainshipNameClient;
                break;
            case ShipSymbol.A1:
                name = MapLayout.aSubship1NameClient;
                break;
            case ShipSymbol.A2:
                name = MapLayout.aSubship2NameClient;
                break;
            case ShipSymbol.A3:
                name = MapLayout.aSubship3NameClient;
                break;
            case ShipSymbol.B0:
                name = MapLayout.bMainshipNameClient;
                break;
            case ShipSymbol.B1:
                name = MapLayout.bSubship1NameClient;
                break;
            case ShipSymbol.B2:
                name = MapLayout.bSubship2NameClient;
                break;
            case ShipSymbol.B3:
                name = MapLayout.bSubship3NameClient;
                break;
        }

        MaterialSetter.ChangeAlpha(GameObject.Find(name).transform.Find(MapLayout.shipUpperComponentName).GetComponent<Renderer>().material, alphaValue);
        MaterialSetter.ChangeAlpha(GameObject.Find(name).transform.Find(MapLayout.shipDownComponentName).GetComponent<Renderer>().material, alphaValue);

    }

    private void OnDisable()
    {
        AMainShipVisibility.OnValueChanged -= AMainChanged;
        ASubShip1Visibility.OnValueChanged -= ASub1Changed;
        ASubShip2Visibility.OnValueChanged -= ASub2Changed;
        ASubShip3Visibility.OnValueChanged -= ASub3Changed;

        BMainShipVisibility.OnValueChanged -= BMainChanged;
        BSubShip1Visibility.OnValueChanged -= BSub1Changed;
        BSubShip2Visibility.OnValueChanged -= BSub2Changed;
        BSubShip3Visibility.OnValueChanged -= BSub3Changed;

        IsShipSpawned.OnValueChanged -= IsShipSpawnedValueChanged;
    }

    private void ChangeValueVisiblity(ShipSymbol ss, bool value)
    {
        switch (ss)
        {
            case ShipSymbol.A0:
                //AMainShipVisibility.Value = value;
                break;
            case ShipSymbol.A1:
                ASubShip1Visibility.Value = value;
                break;
            case ShipSymbol.A2:
                ASubShip2Visibility.Value = value;
                break;
            case ShipSymbol.A3:
                ASubShip3Visibility.Value = value;
                break;
            case ShipSymbol.B0:
                //BMainShipVisibility.Value = value;
                break;
            case ShipSymbol.B1:
                BSubShip1Visibility.Value = value;
                break;
            case ShipSymbol.B2:
                BSubShip2Visibility.Value = value;
                break;
            case ShipSymbol.B3:
                BSubShip3Visibility.Value = value;
                break;

        }
    }
}
