using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.UI;

public class RaderObject : MonoBehaviour
{
    public Rader rader;
    public Text shipText;
    public Button movebtn;
    public  Button abilitybtn;

    public int playerType;
    public DirectionType dirType;
    public ShipType shipType;
    public Team team;

    private int type;

    private void Awake()
    {
    }

    private void Start()
    {
        rader = new Rader();
        this.type = 0;

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("this is server");
            team = Team.ATeam;
        }
        else
        {
            team = Team.BTeam;
        }

        Debug.Log("this type is " + team+ " , and shiptype is  "+shipType);
    }

    public void onMoveBtnClick()
    {
        Debug.Log(team.ToString() + " 's " + shipType.ToString() + " move button clicked");
    }

    public void onAbilityBtnClick()
    {
        Debug.Log(team.ToString() + " 's " + shipType.ToString() + " ability button clicked");
    }

}
