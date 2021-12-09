using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FixedFogManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject raderPrefab;
    public GameObject raderObjectPrefab;

    public Map map;
    public GameObject Tiles;
    public GameObject Raders;

    public FixedFog[,] fixedFogGrid = new FixedFog[MapLayout.mapSize.x, MapLayout.mapSize.y];

    public Tile[,] tileGrid = new Tile[MapLayout.mapSize.x, MapLayout.mapSize.y];
    public Rader[,] raderGrid = new Rader[2,2];
    public RaderObject[,] raderObjectGrid = new RaderObject[2, 2];


    private void Awake()
    {
    }

    private void Start()
    {
        SpawnTiles();
        //SpawnRaders();
        SpawnRaderObjects();
    }

    private void SpawnRaderObjects()
    {
        for (int i = 0; i < raderObjectGrid.GetLength(0); i++)
        {
            for (int j = 0; j < raderObjectGrid.GetLength(1); j++)
            {
                raderObjectGrid[i, j] = GetRaderObject();
                switch(i*2+j)
                {
                    case 0:
                        raderObjectGrid[i, j].shipText.text = "MainShip";
                        raderObjectGrid[i, j].shipType = ShipType.MainShip;
                        raderObjectGrid[i, j].shipText.GetComponent<RectTransform>().anchoredPosition = new Vector3(170, 230, 0);
                        raderObjectGrid[i, j].movebtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(120, 50, 0);
                        raderObjectGrid[i, j].abilitybtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(210, 50, 0);
                        break;
                    case 1:
                        raderObjectGrid[i, j].shipText.text = "SubShip1";
                        raderObjectGrid[i, j].shipType = ShipType.SubShip1;
                        raderObjectGrid[i, j].shipText.GetComponent<RectTransform>().anchoredPosition = new Vector3(340, 230, 0);
                        raderObjectGrid[i, j].movebtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(290, 50, 0);
                        raderObjectGrid[i, j].abilitybtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(380, 50, 0);
                        break;
                    case 2:
                        raderObjectGrid[i, j].shipText.text = "SubShip2";
                        raderObjectGrid[i, j].shipType = ShipType.SubShip2;
                        raderObjectGrid[i, j].shipText.GetComponent<RectTransform>().anchoredPosition = new Vector3(170, 15, 0);
                        raderObjectGrid[i, j].movebtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(120, -170, 0);
                        raderObjectGrid[i, j].abilitybtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(210, -170, 0);
                        break;
                    case 3:
                        raderObjectGrid[i, j].shipText.text = "SubShip3";
                        raderObjectGrid[i, j].shipType = ShipType.SubShip3;
                        raderObjectGrid[i, j].shipText.GetComponent<RectTransform>().anchoredPosition = new Vector3(340, 15, 0);
                        raderObjectGrid[i, j].movebtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(290, -170, 0);
                        raderObjectGrid[i, j].abilitybtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(380, -170, 0);
                        break;
                }
                raderObjectGrid[i, j].rader.stopRader();

                float x = map.RaderTransform.transform.position.x + (i * 3.5f);
                float z = map.RaderTransform.transform.position.z + (j * 4.5f);

                Vector3 pos = new Vector3(x, map.RaderTransform.transform.position.y + MapLayout.oceanFogInterval, z);

                raderObjectGrid[i, j].transform.position = pos;
                raderObjectGrid[i, j].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                raderObjectGrid[i, j].transform.parent = Raders.transform;

            }
        }
    }

    private void SpawnRaders()
    {
        for ( int i=0; i<raderGrid.GetLength(0); i++ )
        {
            for ( int j = 0; j< raderGrid.GetLength(1); j++ )
            {
                raderGrid[i, j] = GetRader();
                raderGrid[i, j].stopRader();

                float x = map.RaderTransform.transform.position.x +(i*3.5f);
                float z = map.RaderTransform.transform.position.z + (j *4);

                Vector3 pos = new Vector3(x, map.RaderTransform.transform.position.y + MapLayout.oceanFogInterval, z);

                raderGrid[i, j].transform.position = pos;
                raderGrid[i, j].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                raderGrid[i, j].transform.parent = Raders.transform;

            }
        }
    }

    private void SpawnTiles()
    {
        for (int i = 0; i < fixedFogGrid.GetLength(0); i++)
        {
            for (int j = 0; j < fixedFogGrid.GetLength(1); j++)
            {
                // set fog in array
                tileGrid[i, j] = GetTile();

                //float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (i + 0.5f);
                //float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (j + 0.5f);

                float x = map.topLeftSquareTransform.transform.position.x + MapLayout.areaSize * (i + 0.5f);
                float z = map.topLeftSquareTransform.transform.position.z + MapLayout.areaSize * (j + 0.5f);

                //Vector3 pos = new Vector3(x, map.bottomLeftSquareTransform.transform.position.y + MapLayout.oceanTileInterval, z);
                Vector3 pos = new Vector3(x, map.topLeftSquareTransform.transform.position.y + MapLayout.oceanFogInterval, z);

                tileGrid[i, j].transform.position = pos;
                tileGrid[i, j].transform.localScale = new Vector3(MapLayout.areaSize, MapLayout.areaSize* 0.1f, MapLayout.areaSize);


                tileGrid[i, j].transform.parent = Tiles.transform;
            }
        }
    }

    private RaderObject GetRaderObject()
    {
        GameObject raderObjectObj = (GameObject)Instantiate(raderObjectPrefab);
        RaderObject raderobject = raderObjectObj.GetComponent<RaderObject>();

        return raderobject;
    }

    private Tile GetTile()
    {
        GameObject tileObj = (GameObject)Instantiate(tilePrefab);
        Tile tile = tileObj.GetComponent<Tile>();

        return tile;
    }

    private Rader GetRader()
    {
        GameObject raderObj = (GameObject)Instantiate(raderPrefab);
        Rader rader = raderObj.GetComponent<Rader>();
        //rader.setShipType(temp);

        return rader;
    }

    internal void ClearFog(Vector2Int coords)
    {
        if (fixedFogGrid[coords.x, coords.y] != null)
        {
            Destroy(fixedFogGrid[coords.x, coords.y].gameObject);

            // for test
            Debug.Log(coords + " fog is destroyed.");
        }
        else
        {
            Debug.Log("There is no fog at " + coords);
        }
    }

    // for test (clearFog)
    public void ClearFogTest()
    {
        int x = Random.Range(0, MapLayout.mapSize.x);
        int y = Random.Range(0, MapLayout.mapSize.y);
        ClearFog(new Vector2Int(x, y));
    }
}

