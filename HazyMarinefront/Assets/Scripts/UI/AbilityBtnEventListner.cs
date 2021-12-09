using MLAPI;
using MLAPI.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBtnEventListner : MonoBehaviour
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
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
        {
            return;
        }

        int c = MapLayout.GetCostByShipType(shipType);
        // cost �ִ��� Ȯ��
        if (TurnManager.cost < c)
        {
            // �����մϴ�! ���â ����
            Debug.Log("Cost ����! �ʿ� cost: " + c);
            GameObject dialog = Instantiate(
                AlertDialogPrefab
                );
            dialog.GetComponent<AlertDialog>().SetTitle("Cost ����! - �ʿ� Cost: " + c);
            return;
        }
        else
        {
            Debug.Log("cost �Ҹ��Ͽ� �����Ƽ �ߵ�: " + c);
        }

        if (MainShipAbilityUsed)
        {
            GameObject dialog = Instantiate(
                AlertDialogPrefab);
            dialog.GetComponent<AlertDialog>().SetTitle("MainShip�� �ɷ��� 1���� �ߵ� ����");
            return;
        }


        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        ShipSymbol ss = MapLayout.GetSymbolByShiptypeTeam(shipType, team);
        PlayManager.ActivateShipAbilityServerRpc((int)ss);

        TurnManager.cost -= c;
        GameObject.Find("CostText").GetComponent<Text>().text = TurnManager.cost.ToString();

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
