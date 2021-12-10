using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIMoveBtnEventListener : MonoBehaviour
{
    public GameObject MoveControllUICanvas;

    public Button moveBtn;
    public Dropdown dirDropDown;
    public Dropdown teamDropDown;
    public Dropdown shipDropDown;

    private const int amount = 1; // 움직임 1칸 고정

    public DirectionType dirType { get; private set; }
    public Team team { get; private set; }
    public ShipType shipType { get; private set; }

    private void Awake()
    {
        dirType = DirectionType.Front;
        team = Team.ATeam;
        shipType = ShipType.MainShip;
    }


    public void MoveShip()
    {
        ShipSymbol s = MapLayout.GetSymbolByShiptypeTeam(shipType, team);

        GameObject.Find("AIMap(Clone)").GetComponent<AIManager>().SetMoveShip(s, dirType, amount);
        //PlayManager.SetMoveShipServerRpc(s, dirType, amount);
    }

    public void SelectedShipTypeChanged()
    {
        switch (shipDropDown.captionText.text)
        {
            case "MainShip": shipType = ShipType.MainShip; break;
            case "SubShip1": shipType = ShipType.SubShip1; break;
            case "SubShip2": shipType = ShipType.SubShip2; break;
            case "SubShip3": shipType = ShipType.SubShip3; break;
            case "SubShip4": shipType = ShipType.SubShip4; break;
            default: return;
        }
        GameObject.Find("AIMap(Clone)").GetComponent<AIMap>().SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(shipType, team));
        //PlayManager.MapInstance.GetComponent<Map>().SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(shipType, team));
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
        
        GameObject.Find("AIMap(Clone)").GetComponent<AIMap>().SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(shipType, team));
        //PlayManager.MapInstance.GetComponent<Map>().SetSelectedShip(MapLayout.GetSymbolByShiptypeTeam(shipType, team));
    }

    public void SetActiveMoveCanvas(bool act)
    {
        MoveControllUICanvas.SetActive(act);
    }
}
