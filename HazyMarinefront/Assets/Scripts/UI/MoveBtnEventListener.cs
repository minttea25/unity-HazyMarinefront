using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBtnEventListener : MonoBehaviour
{
    public Button moveBtn;
    public GameManager manager;
    public Dropdown dirDropDown;

    private const int amount = 1; // ������ 1ĭ ����
    private DirectionType dirType;

    private void Awake()
    {
        // �⺻ ���� (���߿� ����/front/back/right/left�� ���� ���� �κ� ���� ����)
        dirType = DirectionType.Front;
    }

    public void MoveShip()
    {
        manager.map.MoveShip(dirType, amount);
    }

    // �ϴ� �ϵ� �ڵ�...
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
