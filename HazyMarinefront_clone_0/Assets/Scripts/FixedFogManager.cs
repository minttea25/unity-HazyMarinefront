using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FixedFogManager : MonoBehaviour
{
    //public GameObject[] fogPrefabs;
    public GameObject tilePrefab;
    public Map map;
    public GameObject Tiles;

    public FixedFog[,] fixedFogGrid = new FixedFog[MapLayout.mapSize.x, MapLayout.mapSize.y];

    public Tile[,] tileGrid = new Tile[MapLayout.mapSize.x, MapLayout.mapSize.y];


    private void Awake()
    {
    }

    private void Start()
    {
        SpawnTiles();
    }

    /*public void SetFixedFogBlock(List<Vector2Int> exceptCoords)
    {
        for (int i=0; i<fixedFogGrid.GetLength(0); i++)
        {
            for (int j=0; j<fixedFogGrid.GetLength(1); j++)
            {
                if (exceptCoords != null)
                {
                    if (exceptCoords.Contains(new Vector2Int(i, j)))
                    {
                        fixedFogGrid[i, j] = null;
                        //tileGrid[i, j] = null;
                        continue;
                    }
                }
                else
                {
                    // set fog in array
                    fixedFogGrid[i, j] = GetFixedFog();
                    RevealFixedFogBlocks(new Vector2Int(i, j));
                    fixedFogGrid[i, j].transform.parent = map.fogBlocks.transform;
                }
            }
        }

        // set on the map

    }*/

    private void SpawnTiles()
    {
        for (int i = 0; i < fixedFogGrid.GetLength(0); i++)
        {
            for (int j = 0; j < fixedFogGrid.GetLength(1); j++)
            {
                // set fog in array
                tileGrid[i, j] = GetTile();

                float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (i + 0.5f);
                float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (j + 0.5f);

                Vector3 pos = new Vector3(x, map.bottomLeftSquareTransform.transform.position.y + MapLayout.oceanTileInterval, z);

                tileGrid[i, j].transform.position = pos;
                tileGrid[i, j].transform.localScale = new Vector3(MapLayout.areaSize, MapLayout.areaSize* 0.1f, MapLayout.areaSize);


                tileGrid[i, j].transform.parent = Tiles.transform;
            }
        }
    }

    /*private FixedFog GetFixedFog()
    {
        int r = Random.Range(0, fogPrefabs.Length);
        GameObject fogObj = (GameObject)Instantiate(fogPrefabs[r]);
        FixedFog fog = fogObj.GetComponent<FixedFog>();

        return fog;
    }*/

    private Tile GetTile()
    {
        GameObject tileObj = (GameObject)Instantiate(tilePrefab);
        Tile tile = tileObj.GetComponent<Tile>();

        return tile;
    }


    /*private void RevealFixedFogBlocks(Vector2Int coord)
    {
        float x = map.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (coord.x + 0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (coord.y + 0.5f);

        Vector3 pos = new Vector3(x, map.bottomLeftSquareTransform.transform.position.y + MapLayout.oceanFogInterval, z);

        fixedFogGrid[coord.x, coord.y].transform.position = pos;
        fixedFogGrid[coord.x, coord.y].transform.localScale = new Vector3(MapLayout.areaSize, MapLayout.areaSize, MapLayout.areaSize);

        Vector3 pos2 = new Vector3(x, map.bottomLeftSquareTransform.transform.position.y + MapLayout.oceanTileInterval, z);

        tileGrid[coord.x, coord.y].transform.position = pos2;
        tileGrid[coord.x, coord.y].transform.localScale = new Vector3(MapLayout.areaSize, MapLayout.areaSize, MapLayout.areaSize);
    }*/

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

