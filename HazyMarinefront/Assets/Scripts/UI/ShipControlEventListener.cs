using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;


public class ShipControlEventListener : MonoBehaviour
{
    public GameObject MoveControllUICanvas;
    public GameObject RadarUICanvas;

    public Button MainShipBtn;
    public Button SubShip1Btn;
    public Button SubShip2Btn;
    public Button SubShip3Btn;
    public Button SubShip4Btn; // default로 invisible

    public GameObject MainShipBtnBackground;
    public GameObject SubShip1BtnBackground;
    public GameObject SubShip2BtnBackground;
    public GameObject SubShip3BtnBackground;
    public GameObject SubShip4BtnBackground; // default로 invisible

    public Text selectedShipText;

    public Dropdown dirDropDown;

    public ShipType nowSelected;
    public Team team;
    public DirectionType direction;

    private const int amount = 1; // 움직임 1칸 고정

    public Button attackBtn;
    public Text attackText;

    public bool AttackMode;
    public bool CrossAttackMode;

    public GameObject EndTurnBtn;

    public bool MainShipAbilityUsed;
    public GameObject AlertDialogPrefab;
    public Text CostText;

    // Start is called before the first frame update
    void Start()
    {
        direction = DirectionType.Front;
        nowSelected = ShipType.MainShip; // default
        SubShip4BtnBackground.SetActive(false);
        MainShipAbilityUsed = false;

        
    }

    public void SetTeam(Team t)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("This is server.");
            team = t;
        }
        else
        {
            Debug.Log("This is client");
            team = t;
        }
    }

    // about move
    public void MoveShip()
    {
        TurnManager tm = GetTurnManager();

        if (tm == null) { return; }

        if (tm.hasMoved)
        {
            // 한턴에 한번만 움직일 수 있음
            GameObject dialog = Instantiate(
                AlertDialogPrefab);
            dialog.GetComponent<AlertDialog>().SetTitle("한 턴에 한번만 이동 가능합니다.");
            return;

        }

        ShipSymbol s = MapLayout.GetSymbolByShiptypeTeam(nowSelected, team);

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        PlayManager.SetMoveShipServerRpc(s, direction, amount);

        tm.hasMoved = true;
    }

    // about and attack
    public void SetAttack()
    {
        if (AttackMode)
        {
            attackText.text = "ATTACK";
            AttackMode = false;
        }
        else
        {
            attackText.text = "CANCEL";
            AttackMode = true;
        }
    }

    public void SetAttackMode(bool atk)
    {
        if (atk)
        {
            attackText.text = "CANCEL";
            AttackMode = true;
        }
        else
        {
            attackText.text = "ATTACK";
            AttackMode = false;
        }
    }
    public void SetCrossAttackMode(bool atk)
    {
        if (atk)
        {
            CrossAttackMode = true;
        }
        else
        {
            CrossAttackMode = false;
        }
    }

    // about ability
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

        int c = MapLayout.GetCostByShipType(nowSelected);
        // cost 있는지 확인
        if (TurnManager.cost < c)
        {
            // 부족합니다! 경고창 띄우기
            Debug.Log("Cost 부족! 필요 cost: " + c);
            GameObject dialog = Instantiate(
                AlertDialogPrefab
                );
            dialog.GetComponent<AlertDialog>().SetTitle("Cost 부족! - 필요 Cost: " + c);
            return;
        }

        if (MainShipAbilityUsed)
        {
            GameObject dialog = Instantiate(
                AlertDialogPrefab);
            dialog.GetComponent<AlertDialog>().SetTitle("MainShip의 능력은 1번만 발동 가능");
            return;
        }


        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        ShipSymbol ss = MapLayout.GetSymbolByShiptypeTeam(nowSelected, team);
        PlayManager.ActivateShipAbilityServerRpc((int)ss);

        TurnManager.cost -= c;
        //GameObject.Find("CostText").GetComponent<Text>().text = TurnManager.cost.ToString();
        CostText.text = TurnManager.cost.ToString();

        if (nowSelected == ShipType.MainShip)
        {
            MainShipAbilityUsed = true;
        }
    }

    // about direction
    public void SelectedDirectionChanged()
    {
        switch (dirDropDown.captionText.text)
        {
            case "Front":
                direction = DirectionType.Front;
                break;
            case "Back":
                direction = DirectionType.Back;
                break;
            case "Right":
                direction = DirectionType.Right;
                break;
            case "Left":
                direction = DirectionType.Left;
                break;
            default:
                Debug.Log("dir error - MoveBtnEventListener");
                return;
        }
    }

    // about endturn
    public void EndTurn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GetTurnManager()?.SetGameState(3);
            SetActiveMoveCanvas(false);
        }
        else
        {
            GetTurnManager()?.SetGameState(2);
            SetActiveMoveCanvas(false);
        }
    }

    private TurnManager GetTurnManager()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return null;
        }

        if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
        {
            return null;
        }

        return TurnManager;
    }


    // UI visible
    public void SetActiveMoveCanvas(bool act)
    {
        MoveControllUICanvas.SetActive(act);
    }
    public void SetActiveRadarCanvas(bool act)
    {
        RadarUICanvas.SetActive(act);
    }

    // radar UI
    public void SetActiveShipRadarUI(ShipType type, bool act)
    {
        switch(type)
        {
            case ShipType.MainShip:
                MainShipBtnBackground.SetActive(act);
                break;
            case ShipType.SubShip1:
                SubShip1BtnBackground.SetActive(act);
                break;
            case ShipType.SubShip2:
                SubShip2BtnBackground.SetActive(act);
                break;
            case ShipType.SubShip3:
                SubShip3BtnBackground.SetActive(act);
                break;
            case ShipType.SubShip4:
                SubShip4BtnBackground.SetActive(act);
                break;
        }

        if (!act)
        {
            SetPosibleSelectedShip(type);
        }
    }

    public void SetPosibleSelectedShip(ShipType destryedShiptype)
    {
        if (MainShipBtnBackground.activeSelf == true && (destryedShiptype != ShipType.MainShip))
        {
            MainShipSelected();
        }
        else if (SubShip1BtnBackground.activeSelf == true && (destryedShiptype != ShipType.SubShip1))
        {
            Subship1Selected();
        }
        else if (SubShip2BtnBackground.activeSelf == true && (destryedShiptype != ShipType.SubShip2))
        {
            Subship2Selected();
        }
        else if (SubShip3BtnBackground.activeSelf == true && (destryedShiptype != ShipType.SubShip3))
        {
            Subship3Selected();
        }
        else if (SubShip4BtnBackground.activeSelf == true && (destryedShiptype != ShipType.SubShip4))
        {
            Subship4Selected();
        }
        else
        {
            Debug.Log("There is no ship!");
        }
    }

    // about select ship
    public void MainShipSelected()
    {
        Debug.Log("MainShipSelected");
        nowSelected = ShipType.MainShip;
        HighLightBackground(nowSelected);
        selectedShipText.text = "MainShip";
    }

    public void Subship1Selected()
    {
        Debug.Log("SubShip1Selected");
        nowSelected = ShipType.SubShip1;
        HighLightBackground(nowSelected);
        selectedShipText.text = "SubShip1";
    }

    public void Subship2Selected()
    {
        Debug.Log("SubShip2Selected");
        nowSelected = ShipType.SubShip2;
        HighLightBackground(nowSelected);
        selectedShipText.text = "SubShip2";
    }


    public void Subship3Selected()
    {
        Debug.Log("SubShip3Selected");
        nowSelected = ShipType.SubShip3;
        HighLightBackground(nowSelected);
        selectedShipText.text = "SubShip3";
    }

    public void Subship4Selected()
    {
        Debug.Log("SubShip4Selected");
        nowSelected = ShipType.SubShip4;
        HighLightBackground(nowSelected);
        selectedShipText.text = "SubShip4";
    }

    public void HighLightBackground(ShipType type)
    {
        MainShipBtnBackground.GetComponent<Image>().color = MapLayout.unselectedShipBackgroundColor;
        SubShip1BtnBackground.GetComponent<Image>().color = MapLayout.unselectedShipBackgroundColor;
        SubShip2BtnBackground.GetComponent<Image>().color = MapLayout.unselectedShipBackgroundColor;
        SubShip3BtnBackground.GetComponent<Image>().color = MapLayout.unselectedShipBackgroundColor;
        SubShip4BtnBackground.GetComponent<Image>().color = MapLayout.unselectedShipBackgroundColor;

        switch (type)
        {
            case ShipType.MainShip:
                MainShipBtnBackground.GetComponent<Image>().color = MapLayout.selectedShipBackgroundColor;
                break;
            case ShipType.SubShip1:
                SubShip1BtnBackground.GetComponent<Image>().color = MapLayout.selectedShipBackgroundColor;
                break;
            case ShipType.SubShip2:
                SubShip2BtnBackground.GetComponent<Image>().color = MapLayout.selectedShipBackgroundColor;
                break;
            case ShipType.SubShip3:
                SubShip3BtnBackground.GetComponent<Image>().color = MapLayout.selectedShipBackgroundColor;
                break;
            case ShipType.SubShip4:
                SubShip4BtnBackground.GetComponent<Image>().color = MapLayout.selectedShipBackgroundColor;
                break;
        }
    }



    // Update is called once per frame
    void Update()
    {
        // sweep
    }
}
