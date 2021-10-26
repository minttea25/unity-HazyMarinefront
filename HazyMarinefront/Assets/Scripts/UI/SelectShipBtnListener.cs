using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectShipBtnListener : MonoBehaviour
{
    public GameObject mapObject;
    public Button[] shipBtns;
    public Text selectedShipText; // UnityEngine.UI.Text

    private Map map;

    // ShipType 클래스를 인자로 받으면 onclick에서 인식을 못함...
    // 일단 임시로 string
    // 나중에 직접 addlistener로 이벤트 추가할 것 (인자 줄이기)

    private void Awake()
    {
        map = mapObject.GetComponent<Map>();
    }

    public void SelectShip(int typeInt)
    {
        ShipType type;

        switch(typeInt)
        {
            case 0: type = ShipType.MainShip; break;
            case 1: type = ShipType.SubShip1; break;
            case 2: type = ShipType.SubShip2; break;
            case 3: type = ShipType.SubShip3; break;
            case 4: type = ShipType.SubShip4; break;
            default: Debug.Log("type error - SelectShipBtnListner");  return;
        }

        switch(type)
        {
            case ShipType.MainShip:
                HighlightOnlyOneBtn(shipBtns[0], typeInt, type);
                break;
            case ShipType.SubShip1:
                HighlightOnlyOneBtn(shipBtns[1], typeInt, type);
                break;
            case ShipType.SubShip2:
                HighlightOnlyOneBtn(shipBtns[2], typeInt, type);
                break;
            case ShipType.SubShip3:
                HighlightOnlyOneBtn(shipBtns[3], typeInt, type);
                break;
            case ShipType.SubShip4:
                HighlightOnlyOneBtn(shipBtns[4], typeInt, type);
                break;
            default:
                Debug.Log("type error - SelectShipBtnListner");
                return;
        }
    }

    private void HighlightOnlyOneBtn(Button btn, int typeInt, ShipType type)
    {
        for (int i=0; i<shipBtns.Length; i++)
        {
            if (i == typeInt)
            {
                ColorBlock cb2 = btn.colors;
                cb2.normalColor = Color.blue;
                btn.colors = cb2;
                continue;
            }
            ColorBlock cb = shipBtns[i].colors;
            cb.normalColor = Color.white;
            shipBtns[i].colors = cb;
        }
        

        ShowSelectedShipText(type);

        bool thereIs = map.SetSelectedShip(type);
        Debug.Log("ThereIs: " + thereIs + " - " + map.GetSelectedShip());
    }

    private void ShowSelectedShipText(ShipType type)
    {
        selectedShipText.text = type.ToString();
    }
}
