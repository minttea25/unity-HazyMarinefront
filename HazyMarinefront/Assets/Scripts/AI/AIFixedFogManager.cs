using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFixedFogManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public AIMap aimap;
    public GameObject Tiles;

    public FixedFog[,] fixedFogGrid = new FixedFog[MapLayout.mapSize.x, MapLayout.mapSize.y];

    public Tile[,] tileGrid = new Tile[MapLayout.mapSize.x, MapLayout.mapSize.y];

    public AITile[,] AItileGrid = new AITile[MapLayout.mapSize.x, MapLayout.mapSize.y];

    private void Awake()
    {
    }

    private void Start()
    {
          SpawnAITiles();
    }


    private void SpawnAITiles()
    {
        for (int i = 0; i < fixedFogGrid.GetLength(0); i++)
        {
            for (int j = 0; j < fixedFogGrid.GetLength(1); j++)
            {
                // set fog in array
                AItileGrid[i, j] = GetAITile();

                float x = aimap.bottomLeftSquareTransform.transform.position.x + MapLayout.areaSize * (i + 0.5f);
                float z = aimap.bottomLeftSquareTransform.transform.position.z + MapLayout.areaSize * (j + 0.5f);

                Vector3 pos = new Vector3(x, aimap.bottomLeftSquareTransform.transform.position.y + MapLayout.oceanTileInterval, z);

                tileGrid[i, j].transform.position = pos;
                tileGrid[i, j].transform.localScale = new Vector3(MapLayout.areaSize, MapLayout.areaSize * 0.1f, MapLayout.areaSize);


                tileGrid[i, j].transform.parent = Tiles.transform;
            }
        }
    }

    private AITile GetAITile()
    {
        GameObject tileObj = (GameObject)Instantiate(tilePrefab);
        AITile tile = tileObj.GetComponent<AITile>();

        return tile;
    }

    internal void ClearFog(Vector2Int coords)
    {
        if (coords.x < 0 || coords.x >= MapLayout.mapSize.x
            || coords.y < 0 || coords.y >= MapLayout.mapSize.y)
        {
            return;
        }

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
