using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FixedFogManager : MonoBehaviour
{
    public MapLayout mapLayout;
    public GameObject[] fogPrefabs;
    public Map map;

    FixedFog[,] fixedFogGrid;
    

    private void Awake()
    {
        if (fogPrefabs.Length == 0)
        {
            Debug.Log("There are no fog prefabs.");
        }
        Debug.Log("SDFSDF");
        fixedFogGrid = new FixedFog[mapLayout.mapSize.x, mapLayout.mapSize.y];
    }

    public void SetFixedFogBlock(List<Vector2Int> exceptCoords)
    {
        Debug.Log(fixedFogGrid);
        for (int i=0; i<fixedFogGrid.GetLength(0); i++)
        {
            for (int j=0; j<fixedFogGrid.GetLength(1); j++)
            {
                if (exceptCoords != null)
                {
                    if (exceptCoords.Contains(new Vector2Int(i, j)))
                    {
                        fixedFogGrid[i, j] = null;
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

    }

    private FixedFog GetFixedFog()
    {
        int r = Random.Range(0, fogPrefabs.Length);
        GameObject fogObj = (GameObject)Instantiate(fogPrefabs[r]);
        FixedFog fog = fogObj.GetComponent<FixedFog>();

        return fog;
    }

    private void RevealFixedFogBlocks(Vector2Int coord)
    {
        float x = map.bottomLeftSquareTransform.transform.position.x + map.areaSize * (coord.x + 0.5f);
        float z = map.bottomLeftSquareTransform.transform.position.z + map.areaSize * (coord.y + 0.5f);

        Vector3 pos = new Vector3(x, map.bottomLeftSquareTransform.transform.position.y + 10, z);

        fixedFogGrid[coord.x, coord.y].transform.position = pos;
        fixedFogGrid[coord.x, coord.y].transform.localScale = new Vector3(mapLayout.areaSize, mapLayout.areaSize, mapLayout.areaSize);
    }

    private void ClearFog(Vector2Int coords)
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
        int x = Random.Range(0, mapLayout.mapSize.x);
        int y = Random.Range(0, mapLayout.mapSize.y);
        ClearFog(new Vector2Int(x, y));
    }
}

