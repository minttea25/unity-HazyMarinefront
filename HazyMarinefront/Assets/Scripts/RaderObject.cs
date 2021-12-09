using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.UI;

public class RaderObject : MonoBehaviour
{
    public Rader rader;
    public Text shipText;
    //public Button movebtn;
    //public  Button abilitybtn;

    public int playerType;
    public DirectionType dirType;
    public ShipType shipType;
    public Team team;
    public ShipSymbol symbol;

    public GameObject textPrefab;

    public Text[] textGrids = new Text[7];

    public Canvas shipCanvas;

    private int type;
    public Vector3 standard;

    [SerializeField] public NetworkObject MapInstance { get; private set; }

    public Vector3[,] axis = new Vector3[2,6];

    public void placeShips(List<Vector3> distList)
    {
        int limit = distList.Count;

        switch (shipType)
        {
            case ShipType.MainShip: standard = new Vector3(170, 140, 0); break;
            case ShipType.SubShip1: standard = new Vector3(335, 140, 0); break;
            case ShipType.SubShip2: standard = new Vector3(170, -75, 0); break;
            case ShipType.SubShip3: standard = new Vector3(335, -75, 0); break;
            default: break;
        }

        Debug.Log(shipType + " , " + team + "'s  distList size : " + distList.Count);

        for (int i = 0; i < textGrids.GetLength(0) ; i++)
        {
            
            if ( i< limit )
            {
                textGrids[i].text = "*";
                float x = standard.x + distList[i].x;
                float y = standard.y + distList[i].z;
                float z = 0;
                Debug.Log(shipType + " , " + team + "'s  " + i + "th time : (" + x + " , " + y + " , " + z + ") ");
                textGrids[i].transform.localScale = new Vector3(1, 1, 1);
                textGrids[i].color = Color.blue;
                textGrids[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, z);
            }
            else
            {
                textGrids[i].text = "";
                textGrids[i].transform.localScale = new Vector3(0, 0, 0);
            }

        }

        //for (int i = 0; i < 7; i++)
        //{
        //    GameObject textObj = (GameObject)Instantiate(textPrefab);
        //    Text text = textObj.GetComponent<Text>();
        //    shipsGrid[i] = text;

        //    if (i < limit)
        //    {
        //        float x = standard.x + distList[i].x;
        //        float y = standard.z + distList[i].y;
        //        float z = 0;

        //        Debug.Log(shipType + " , " + team + "'s  " + i + "th time : (" + x + " , " + y + " , " + z + ") ");

        //        //shipsGrid[i].text = "*";

        //        //shipsGrid[i].color = Color.blue;
        //        //shipsGrid[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, z);
        //        //shipsGrid[i].transform.localScale = new Vector3(1, 1, 1);
        //        //shipsGrid[i].transform.parent = shipCanvas.transform;
        //    }
        //    else
        //    {
        //        shipsGrid[i].text = "";
        //    }
        //}


        //axis[0, 0] = new Vector3(0, 70, 0);
        //axis[0, 1] = new Vector3(-35, 35, 0);
        //axis[0, 2] = new Vector3(0, 35, 0);
        //axis[0, 3] = new Vector3(35, 35, 0);
        //axis[0, 4] = new Vector3(-70, 0, 0);
        //axis[0, 5] = new Vector3(-35, 0, 0);
        //axis[1, 0] = new Vector3(35, 0, 0);
        //axis[1, 1] = new Vector3(70, 0, 0);
        //axis[1, 2] = new Vector3(-35, -35, 0);
        //axis[1, 3] = new Vector3(0, -35, 0);
        //axis[1, 4] = new Vector3(35, -35, 0);
        //axis[1, 5] = new Vector3(0, -70, 0);



        //for (int i = 0; i < shipsGrid.GetLength(0); i++)
        //{
        //    for (int j = 0; j < shipsGrid.GetLength(1); j++)
        //    {
        //        GameObject textObj = (GameObject)Instantiate(textPrefab);
        //        Text text = textObj.GetComponent<Text>();

        //        shipsGrid[i, j] = text;
        //        float x = axis[i, j].x + standard.x;
        //        float y = axis[i, j].y + standard.y;
        //        Debug.Log(x + " , " + y);
        //        shipsGrid[i, j].text = "*";
        //        shipsGrid[i, j].color = Color.blue;
        //        shipsGrid[i, j].transform.parent = shipCanvas.transform;
        //        shipsGrid[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector3(axis[i, j].x + standard.x, axis[i, j].y + standard.y, 0);
        //        shipsGrid[i, j].transform.localScale = new Vector3(1, 1, 1);

        //    }
        //}
    }

    public void setRaderShips()
    {
        List<Ship> shipList = MapInstance.GetComponent<Map>().ShipsInFieldList;
        for ( int i=0; i<shipList.Count; i++ )
        {
            Ship ship = shipList[i];

            symbol = MapLayout.GetSymbolByShiptypeTeam(this.shipType, this.team);

            if ( symbol == ship.Symbol )
            {
                Vector3 myCenter = ship.shipCenterPosition;
                Debug.Log(symbol.ToString() + " 's center : " + myCenter.x + " , " + myCenter.y + " , " + myCenter.z);
            }
        }
    }

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

        //placeShips();

        for( int i=0; i<textGrids.GetLength(0);i++)
        {
            GameObject textObj = (GameObject)Instantiate(textPrefab);
            Text text = textObj.GetComponent<Text>();

            textGrids[i] = text;
            textGrids[i].text = "";
            
            textGrids[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            textGrids[i].transform.localScale = new Vector3(0, 0, 0);

            textGrids[i].transform.parent = shipCanvas.transform;

        }

    }

    public void onMoveBtnClick()
    {
        Debug.Log(team.ToString() + " 's " + shipType.ToString() + " move button clicked");
    }

    public void onAbilityBtnClick()
    {
        Debug.Log(team.ToString() + " 's " + shipType.ToString() + " ability button clicked");
    }

    public void setRader()
    {

    }
}
