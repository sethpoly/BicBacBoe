using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GridDirection
{
    North,
    South,
    East,
    West
}

public enum TileChoice
{
    Left,
    Middle,
    Right
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    public Dictionary<Vector2, Tile> tiles { get; private set; }
    

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTitle = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTitle.name = $"Tile {x},{y}";

                // Alternate color
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTitle.Init(isOffset);

                // Append to dict
                tiles[new Vector2(x, y)] = spawnedTitle;
            }
        }

        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(tiles.TryGetValue(pos, out var tile)) 
        {
            return tile;
        }
        return null;
    }

    public Tile GetTileFromChoice(TileChoice choice) {
        // TODO:
        // Using TileChoice & current GridDirection, 
        // calculate the tile to return to the caller
        return null;
    }
}
