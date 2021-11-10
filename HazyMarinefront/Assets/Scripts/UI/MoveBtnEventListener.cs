using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBtnEventListener : MonoBehaviour
{
    public Button moveBtn;
    public GameManager manager;

    public DropDownEventListener ddel;
    private const int amount = 1; // 움직임 1칸 고정


    public void MoveShip()
    {
        ShipSymbol s = MapLayout.GetSymbolByShiptypeTeam(ddel.shipType, ddel.team);
        bool exist = manager.map.SetSelectedShip(s);

        if (exist)
        {
            manager.map.MoveShip(ddel.dirType, amount);
        }
        else
        {
            Debug.Log("There is no ship: " + ddel.shipType + " " + ddel.team);
            return;
        }
        
    }
}
