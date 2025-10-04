using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;   // ancho del campo
    public int height = 10;  // alto del campo
    public float tileSize = 1f; // separación entre tiles
    public GameObject soilTilePrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(soilTilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile_{x}_{z}";
            }
        }
    }
}
