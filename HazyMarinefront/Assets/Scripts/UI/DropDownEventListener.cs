using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        // �⺻ ���� (���߿� ����/front/back/right/left�� ���� ���� �κ� ���� ����)
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
    }
}
