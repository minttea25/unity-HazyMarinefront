using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class DropDownEventListener : MonoBehaviour
{
    public Dropdown dirDropDown;
    public Dropdown teamDropDown;
    public Dropdown shipDropDown;

    public DirectionType dirType { get; private set; }
    public Team team { get; private set; }
    public ShipType shipType { get; private set; }

    private void Awake()
    {
        // 기본 선택 (나중에 선택/front/back/right/left로 할지 선택 부분 뺄지 결정)
        dirType = DirectionType.Front;
        team = Team.ATeam;
        shipType = ShipType.MainShip;
    }

    public void SelectedShipTypeChanged()
    {
        switch (shipDropDown.captionText.text)
        {
            case "MainShip": shipType = ShipType.MainShip; break;
            case "SubShip1": shipType = ShipType.SubShip1; break;
            case "SubShip2": shipType = ShipType.SubShip2; break;
            case "SubShip3": shipType = ShipType.SubShip3; break;
            default: return;
        }
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return;
        }
        PlayManager.MapInstance.GetComponent<Map>().SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(shipType, team));
    }

    public void SelectedDirectionChanged()
    {
        switch (dirDropDown.captionText.text)
        {
            case "Front":
                dirType = DirectionType.Front;
                break;
            case "Back":
                dirType = DirectionType.Back;
                break;
            case "Right":
                dirType = DirectionType.Right;
                break;
            case "Left":
                dirType = DirectionType.Left;
                break;
            default:
                Debug.Log("dir error - MoveBtnEventListener");
                return;
        }
    }

    public void SelectedTeamChanged()
    {
        switch (teamDropDown.captionText.text)
        {
            case "Team A": team = Team.ATeam; break;
            case "Team B": team = Team.BTeam; break;
            default: return;
        }
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return;
        }
        PlayManager.MapInstance.GetComponent<Map>().SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(shipType, team));
    }
}
