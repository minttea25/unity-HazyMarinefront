using UnityEngine;
using MLAPI;
using MLAPI.Connection;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using Random = UnityEngine.Random;
using System;

public class AIPlayer : NetworkBehaviour
{
    [SerializeField] private Renderer playerTeamRenderer;
    [SerializeField] private Color[] teamColors;

    [SerializeField] private NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    private NetworkVariable<Team> team = new NetworkVariable<Team>();

    [ServerRpc]
    public void SetTeamServerRpc(Team t)
    {
        // only 2 team (A and B) - make sure team index is valid
        if (t != Team.ATeam && t != Team.BTeam) { return; }

        // Update the team NetworkVariable
        team.Value = t;
    }

    private void OnEnable()
    {
        team.OnValueChanged += OnTeamChanged;
    }

    private void OnDisable()
    {
        team.OnValueChanged -= OnTeamChanged;
    }

    private void OnTeamChanged(Team previousTeam, Team newTeam)
    {
        // Only clients need to update the renderer
        if (!IsClient) { return; }

        // update the player color from team
        playerTeamRenderer.material.color = teamColors[(int)newTeam];
    }

    private void Start()
    {
        if (IsOwner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetTeamServerRpc(Team.ATeam);
                transform.position = new Vector3(6, 3, 0);
                Position.Value = new Vector3(6, 3, 0);
            }
            else
            {
                SetTeamServerRpc(Team.BTeam);
                SetPositionServerRpc(new Vector3(6, 3, -2));
            }
        }

        playerTeamRenderer.material.color = teamColors[(int)team.Value];
    }

    [ServerRpc]
    private void SetPositionServerRpc(Vector3 pos)
    {
        Position.Value = pos;
    }

    private void Update()
    {
        transform.position = Position.Value;
    }

    void turn()
    {

    }

    void moveShip()
    {
        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();

        Ship aiShip;
        DirectionType dirType;

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        while (true)
        {
            ShipType num = (ShipType)Random.Range(0, map.teamBShipPrefabs.Length);
            aiShip = map.GetShipBySymbol(MapLayout.GetSymbolByShiptypeTeam(num, Team.BTeam));
            if (aiShip == null)
                continue;

            break;
        }

        while (true)
        {
            bool collision = false;
            dirType = (DirectionType)Random.Range(0, 3);
            int[] axisValue = aiShip.GetDirectionAmount(dirType, 1);
            int xAxis = axisValue[0];
            int yAxis = axisValue[1];

            for (int i = 0; i < aiShip.shipCoords.Count; i++)
            {
                if (map.grid[aiShip.shipCoords[i].x + xAxis, aiShip.shipCoords[i].y + yAxis] != ShipSymbol.NoShip &&
                    map.grid[aiShip.shipCoords[i].x + xAxis, aiShip.shipCoords[i].y + yAxis] != ShipSymbol.NM &&
                    map.grid[aiShip.shipCoords[i].x + xAxis, aiShip.shipCoords[i].y + yAxis] != aiShip.Symbol)
                    collision = true;
            }
            if (collision)
                continue;
            break;
        }
        PlayManager.SetMoveShipServerRpc(aiShip.Symbol, dirType, 1);
    }

    void atkShip()
    {
        bool atk = false;

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }


        Map map = NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<PlayManager>().MapInstance.GetComponent<Map>();

        for (int i = 0; i < MapLayout.mapSize.x && !atk; i++)
        {
            for (int j = 0; j < MapLayout.mapSize.y; j++)
            {
                if (map.fixedFogManager.fixedFogGrid[i, j] == null && map.grid[i, j] != ShipSymbol.NoShip && map.grid[i, j] != ShipSymbol.NM)
                {
                    if (MapLayout.GetTeamByShipSymbol(map.grid[i, j]) == Team.ATeam)
                    {
                        PlayManager.AttackServerRpc(i, j);
                        atk = true;
                        break;
                    }
                }
            }
        }

        while (!atk)
        {
            int x = Random.Range(0, MapLayout.mapSize.x);
            int y = Random.Range(0, MapLayout.mapSize.y);

            if (map.fixedFogManager.fixedFogGrid[x, y] != null || MapLayout.GetTeamByShipSymbol(map.grid[x, y]) == Team.BTeam)
                continue;

            PlayManager.AttackServerRpc(x, y);

            break;
        }

    }

    void ability()
    {

    }
}
