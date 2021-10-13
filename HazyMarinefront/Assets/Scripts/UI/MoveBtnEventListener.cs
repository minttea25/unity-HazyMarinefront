using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBtnEventListener : MonoBehaviour
{
    public Button moveBtn;
    public GameManager manager;
    public Dropdown dirDropDown;

    private const int amount = 1; // 움직임 1칸 고정
    private DirectionType dirType;

    private void Awake()
    {
        // 기본 선택 (나중에 선택/front/back/right/left로 할지 선택 부분 뺄지 결정)
        dirType = DirectionType.Front;
    }

    public void MoveShip()
    {
        manager.map.MoveShip(dirType, amount);
    }

    // 일단 하드 코딩...
    public void SelectedDirectionChanged()
    {
        switch(dirDropDown.captionText.text)
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
    
}
