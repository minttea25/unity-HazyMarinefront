using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System;

public class Player : NetworkBehaviour
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
                transform.position = new Vector3( -6.5f, 3, -3);
                Position.Value = new Vector3( -6.5f, 3, -3);
            }
            else
            {
                SetTeamServerRpc(Team.BTeam);
                SetPositionServerRpc(new Vector3( 0 , 3, -3));
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
}
