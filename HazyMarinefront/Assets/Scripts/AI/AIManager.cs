using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public GameObject AlertDialogPrefab;

    [SerializeField] public GameObject[] teamAShipPrefabs;
    [SerializeField] public GameObject[] teamBShipPrefabs;

    [SerializeField] public GameObject MapPrfab;
    [SerializeField] public GameObject FogPrefab;
    [SerializeField] public GameObject ExplosionPrefab;
    [SerializeField] public GameObject BigExplosionPrefab;
    [SerializeField] public GameObject WaterSplashPrefab;

    [SerializeField] public GameObject MapInstance { get; private set; }

    [SerializeField] private Dictionary<ShipSymbol, GameObject> SymbolNetworkInstance = new Dictionary<ShipSymbol, GameObject>();

    [SerializeField] public bool AMainShipVisibility;
    [SerializeField] public bool ASubShip1Visibility;
    [SerializeField] public bool ASubShip2Visibility;
    [SerializeField] public bool ASubShip3Visibility;
                                                    
    [SerializeField] public bool BMainShipVisibility;
    [SerializeField] public bool BSubShip1Visibility;
    [SerializeField] public bool BSubShip2Visibility;
    [SerializeField] public bool BSubShip3Visibility;
                            
    [SerializeField] public bool IsShipSpawned;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (!IsOwner) { return; }

        //if (NetworkManager.Singleton.IsServer)
        //{
            SpawnMap();
            SpawnFog();
        //}
        //else
        //{
            //Debug.Log("Client");
        //}
    }

    internal void ClearFogTest()
    {
        GetComponent<PlayManager>().MapInstance.GetComponent<AIMap>().fixedFogManager.ClearFogTest();
    }

    private void SpawnMap()
    {
        MapInstance = Instantiate(
            MapPrfab,
            new Vector3(0, 0, 0),
            Quaternion.identity);
        //MapInstance.Spawn();
    }

    public void SpawnShipRandomCoord()
    {
        for (int k = 0; k < teamAShipPrefabs.Length - 1; k++)
        {
            GameObject shipInstance = Instantiate(
            teamAShipPrefabs[k],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            //shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.ATeam;
            ship.visibility = false;
            ship.Init();

            List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(MapInstance.GetComponent<Map>());

            // deep copy
            ship.shipCoords.Clear();
            ship.shipCoords = temp.ConvertAll(o => new Vector3Int(o.x, o.y, o.z));


            MapInstance.GetComponent<AIMap>().ShipsInFieldList.Add(ship);

            for (int i = 0; i < ship.shipCoords.Count; i++)
            {
                MapInstance.GetComponent<AIMap>().grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, ship.team);
            }

            ship.shipCenterPosition = ship.GetAIShipCenterPositionFromCoord(ship.shipCoords, MapInstance.GetComponent<AIMap>());

            Vector3 pos = ship.shipCenterPosition;
            ship.transform.position = pos;

            ship.transform.parent = MapInstance.GetComponent<AIMap>().shipHolder.transform;
            ship.transform.localScale = new Vector3(1, 1, 1);

            shipInstance.tag = ship.Symbol.ToString();

            SymbolNetworkInstance.Add(ship.Symbol, shipInstance);

            ChangeAlphaValueShip(ship.Symbol, MapLayout.spawnedShipAlphaValue);
        }

        for (int k = 0; k < teamBShipPrefabs.Length - 1; k++)
        {
            GameObject shipInstance = Instantiate(
            teamBShipPrefabs[k],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            //shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.BTeam;
            ship.visibility = false;
            ship.Init();

            List<Vector3Int> temp = ship.GetPosibleAIShipSpawnCoordsList(MapInstance.GetComponent<AIMap>());


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

        IsShipSpawned = true;
    }

    //[ClientRpc]
    //public void SetAttackModeClientRpc(bool mode)
    //{
    //    if (NetworkManager.Singleton.IsServer) { return; }

    //    GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetAttackMode(true);
    //}

    //[ClientRpc]
    //public void SetCrossAttackModeClientRpc(bool mode)
    //{
    //    if (NetworkManager.Singleton.IsServer) { return; }

    //    GameObject.Find("EventSystem").GetComponent<AttackBtnEventListner>().SetCrossAttackMode(true);
    //}



    public void createShip(int num, bool shipType)
    {
        if (shipType)
        {
            GameObject shipInstance = Instantiate(
            teamAShipPrefabs[num],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            //shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.ATeam;
            ship.Init();

            List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(MapInstance.GetComponent<Map>());


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
        else
        {
            GameObject shipInstance = Instantiate(
            teamBShipPrefabs[num],
            new Vector3(0, 0, 0),
            Quaternion.identity);
            //shipInstance.Spawn();

            Ship ship = shipInstance.GetComponent<Ship>();
            ship.team = Team.BTeam;
            ship.Init();

            List<Vector3Int> temp = ship.GetPosibleShipSpawnCoordsList(MapInstance.GetComponent<Map>());

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
    }

    private void SpawnFog()
    {
        float x = MapInstance.GetComponent<AIMap>().bottomLeftSquareTransform.transform.position.x;
        float y = MapInstance.GetComponent<AIMap>().bottomLeftSquareTransform.transform.position.y + MapLayout.oceanFogInterval;
        float z = MapInstance.GetComponent<AIMap>().bottomLeftSquareTransform.transform.position.z;

        for (int i = 0; i < MapLayout.mapSize.x; i++)
        {
            for (int j = 0; j < MapLayout.mapSize.y; j++)
            {
                float vx = x + MapLayout.areaSize * (i + 0.5f);
                float vz = z + MapLayout.areaSize * (j + 0.5f);
                GameObject FogInstance = Instantiate(
                    FogPrefab,
                    new Vector3(vx, y, vz),
                    Quaternion.identity);
                //FogInstance.Spawn();

                FixedFog fog = FogInstance.GetComponent<FixedFog>();
                MapInstance.GetComponent<AIMap>().fixedFogManager.fixedFogGrid[i, j] = fog;
                fog.transform.parent = MapInstance.GetComponent<AIMap>().fogBlocks.transform;
            }
        }
    }

    public void SetMoveShip(ShipSymbol s, DirectionType dirType, int amount)
    {
        Debug.Log("SetMove: " + this.GetHashCode());

        bool exist = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>().SetSelectedShip(s);
        if (exist)
        {

            bool moved = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>().MoveShip(dirType, amount);
            Debug.Log("MOVED: " + moved);
        }
    }

    // for only clicking fog
    public void Attack(int x, int y)
    {
        AIMap map = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>();

        map.fixedFogManager.ClearFog(new Vector2Int(x, y));
        AttackCoord(x, y);
    }

    public void AttackCoord(int x, int y)
    {
        AIMap map = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>();

        // ojy added
        Ship curShip = map.GetSelectedShip();

        bool exist = map.SetSelectedShip(map.GetShipSymbolByCoords(new Vector2Int(x, y)));
        if (exist)
        {
            for (int i = 0; i < map.GetSelectedShip().shipCoords.Count; i++)
            {
                if (map.GetSelectedShip().shipCoords[i].x == x && map.GetSelectedShip().shipCoords[i].y == y)
                {
                    DamageShip(i);
                    Debug.Log("damaged ship : Vector(" + x + ", " + y + ")");
                }
            }
            // ojy added
            map.SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(curShip.shipType, curShip.team));
        }
        else
        {
            Vector3 loc = new Vector3((float)(x - 4.5), 1.5f, (float)(y - 4.5));
            Instantiate(WaterSplashPrefab, loc, Quaternion.identity);
        }
    }

    public void DamageShip(int index)
    {
        AIMap map = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>();
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
                Instantiate(BigExplosionPrefab, ship.transform.position, Quaternion.identity);
                Debug.Log("ship destroyed");

                // ojy added
                for (int i = 0; i < ship.shipCoords.Count; i++)
                {
                    map.grid[ship.shipCoords[i].x, ship.shipCoords[i].y] = ShipSymbol.NoShip;
                }


                // list 俊辑 颇鲍等 硅 昏力
                bool removed = map.RemoveShipInList(ship.Symbol);
                if (removed)
                {
                    Debug.Log(ship.Symbol + " is removed in list");
                }
                else
                {
                    Debug.Log("Remove ship in list error...");
                }

                //if (NetworkManager.Singleton.IsServer)
                //{
                    Destroy(GameObject.FindGameObjectWithTag(ship.Symbol.ToString()));
                //}
                //else
                //{
                    //SymbolNetworkInstance.TryGetValue(ship.Symbol, out NetworkObject obj);
                    //obj.Despawn();
                //}
            }
        }
        else
        {
            var curCoord = new Vector3((float)(ship.shipCoords[index].x - 4.5), 2f, (float)(ship.shipCoords[index].y - 4.5));
            Instantiate(ExplosionPrefab, curCoord, Quaternion.identity);

            // reveal ship
            if (!ship.visibility)
            {
                ShipSymbol ss = MapLayout.GetSymbolByShiptypeTeam(ship.shipType, ship.team);

                ChangeValueVisiblity(ship.Symbol, true);

                ship.visibility = true;

                Debug.Log("REVEAL SHIP: " + ss);
            }

        }
        ship.shipCoords[index] = new Vector3Int(ship.shipCoords[index].x, ship.shipCoords[index].y, ship.shipCoords[index].z + 1);

        CheckGameOver();
    }

    public void ActivateShipAbility(int symbol)
    {
        AIMap map = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>();

        map.SetSelectedShip((ShipSymbol)symbol);
        Ship s = map.GetSelectedShip();

        if (s == null) { Debug.Log("Selected ship is null at AbilityBtnEventListner"); return; }

        s.ActivateAbility();
    }

    public void CheckGameOver()
    {
        AIMap map = GetComponent<AIManager>().MapInstance.GetComponent<AIMap>();

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

    // 0: 公铰何
    // 1: ATeam 铰府
    // 2: BTeam 铰府
    public void WinLoseEvent(int winLose)
    {
        if (winLose < 0 || winLose > 2)
        {
            Debug.Log("argument error in WinLoseEventServerRpc");
            return;
        }
        else
        {
            // who win?
            GetComponent<TurnManager>().SetWinLose(winLose);
            //TurnManager.SetWinLose(winLose);

            // GameOver Turn
            GetComponent<TurnManager>().SetGameState(4);
            //TurnManager.SetGameState(4);
        }
    }

    private void ChangeAlphaValueShipInServer(ShipSymbol ss, float alphaValue)
    {

        //playManager.SymbolNetworkInstance.TryGetValue(ss, out NetworkObject shipInstance);
        GetComponent<AIManager>().SymbolNetworkInstance.TryGetValue(ss, out GameObject shipInstance);

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
        //AMainShipVisibility.OnValueChanged += AMainChanged;
        //ASubShip1Visibility.OnValueChanged += ASub1Changed;
        //ASubShip2Visibility.OnValueChanged += ASub2Changed;
        //ASubShip3Visibility.OnValueChanged += ASub3Changed;

        //BMainShipVisibility.OnValueChanged += BMainChanged;
        //BSubShip1Visibility.OnValueChanged += BSub1Changed;
        //BSubShip2Visibility.OnValueChanged += BSub2Changed;
        //BSubShip3Visibility.OnValueChanged += BSub3Changed;

        //IsShipSpawned.OnValueChanged += IsShipSpawnedValueChanged;

    }

    private void IsShipSpawnedValueChanged(bool previousValue, bool newValue)
    {
        if (!newValue) { return; }

        //if (NetworkManager.Singleton.IsServer) { return; }

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
        //if (NetworkManager.Singleton.IsServer)
        //{
            //ChangeAlphaValueShipInServer(ss, alphaValue);
        //}
        //else
        //{
            ChangeAlphaValueShipInClient(ss, alphaValue);
        //}
    }

    private void ChangeAlphaValueShipInClient(ShipSymbol ss, float alphaValue)
    {
        //if (NetworkManager.Singleton.IsServer) { return; }

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
        //AMainShipVisibility.OnValueChanged -= AMainChanged;
        //ASubShip1Visibility.OnValueChanged -= ASub1Changed;
        //ASubShip2Visibility.OnValueChanged -= ASub2Changed;
        //ASubShip3Visibility.OnValueChanged -= ASub3Changed;

        //BMainShipVisibility.OnValueChanged -= BMainChanged;
        //BSubShip1Visibility.OnValueChanged -= BSub1Changed;
        //BSubShip2Visibility.OnValueChanged -= BSub2Changed;
        //BSubShip3Visibility.OnValueChanged -= BSub3Changed;

        //IsShipSpawned.OnValueChanged -= IsShipSpawnedValueChanged;
    }

    private void ChangeValueVisiblity(ShipSymbol ss, bool value)
    {
        switch (ss)
        {
            case ShipSymbol.A0:
                //AMainShipVisibility.Value = value;
                break;
            case ShipSymbol.A1:
                ASubShip1Visibility = value;
                break;
            case ShipSymbol.A2:
                ASubShip2Visibility = value;
                break;
            case ShipSymbol.A3:
                ASubShip3Visibility = value;
                break;
            case ShipSymbol.B0:
                //BMainShipVisibility.Value = value;
                break;
            case ShipSymbol.B1:
                BSubShip1Visibility = value;
                break;
            case ShipSymbol.B2:
                BSubShip2Visibility = value;
                break;
            case ShipSymbol.B3:
                BSubShip3Visibility = value;
                break;

        }
    }
}
