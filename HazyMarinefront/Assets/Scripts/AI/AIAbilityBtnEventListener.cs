using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIAbilityBtnEventListener : MonoBehaviour 
{
    public GameObject AlertDialogPrefab;

    public Dropdown dirDropDown;
    public Dropdown teamDropDown;
    public Dropdown shipDropDown;

    public DirectionType dirType { get; private set; }
    public Team team { get; private set; }
    public ShipType shipType { get; private set; }

    public bool MainShipAbilityUsed;

    private void Awake()
    {
        dirType = DirectionType.Front;
        team = Team.ATeam;
        shipType = ShipType.MainShip;

        MainShipAbilityUsed = false;
    }

    public void ActivateAbility()
    {
        int c = MapLayout.GetCostByShipType(shipType);
        // cost 있는지 확인
        if (GameObject.Find("AIMap(Clone)").GetComponent<AIManager>().cost < c)
        {
            // 부족합니다! 경고창 띄우기
            Debug.Log("Cost 부족! 필요 cost: " + c);
            GameObject dialog = Instantiate(
                AlertDialogPrefab
                );
            dialog.GetComponent<AlertDialog>().SetTitle("Cost 부족! - 필요 Cost: " + c);
            return;
        }
        else
        {
            Debug.Log("cost 소모하여 어빌리티 발동: " + c);
        }

        if (MainShipAbilityUsed)
        {
            GameObject dialog = Instantiate(
                AlertDialogPrefab);
            dialog.GetComponent<AlertDialog>().SetTitle("MainShip의 능력은 1번만 발동 가능");
            return;
        }


        ShipSymbol ss = MapLayout.GetSymbolByShiptypeTeam(shipType, team);
        GameObject.Find("AIMap(Clone)").GetComponent<AIManager>().ActivateShipAbility((int)ss);
        //PlayManager.ActivateShipAbilityServerRpc((int)ss);

        GameObject.Find("AIMap(Clone)").GetComponent<AIManager>().cost -= c;
        GameObject.Find("CostText").GetComponent<Text>().text = GameObject.Find("AIMap(Clone)").GetComponent<AIManager>().cost.ToString();

        if (ss == ShipSymbol.A0 || ss == ShipSymbol.B0)
        {
            MainShipAbilityUsed = true;
        }
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