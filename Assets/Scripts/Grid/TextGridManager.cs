using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Linq;
using TileSpriteRender;
using GridAttributes;

public class TextGridManager : GridManager
{
    public TextMapping[] mappingData;
    public TextAsset mapText;
    private Vector2 currentPosition = new Vector2(0, 0);
    private int height;

    void Start()
    {
        shakeBehaviour = GetComponent<ShakeBehaviour>();
        width = GetMapWidth();
        height = GetMapHeight();
        GenerateGrid();
    }

    private int GetMapWidth()
    {
        string[] rows = Regex.Split(mapText.text, "\r\n|\r|\n");
        string longest = rows.OrderByDescending( s => s.Length ).First();
        return longest.Length;
    }

    private int GetMapHeight()
    {
       string[] rows = Regex.Split(mapText.text, "\r\n|\r|\n");
       return rows.Length;
    }

    override public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        string[] rows = Regex.Split(mapText.text, "\r\n|\r|\n");

        foreach(string row in rows.Reverse())
        {
            foreach(char c in row)
            {
                // Use 'x' has empty space
                if(c != 'x') 
                {
                    // Generate tile
                    var spawnedTile = Instantiate(tilePrefab, currentPosition, Quaternion.identity);
                    spawnedTile.Init(false, currentPosition, TileOrientation.Center);
                
                    foreach(TextMapping tm in mappingData)
                    {
                        if(c == tm.character)
                        {
                            Instantiate(tm.prefab, currentPosition, Quaternion.identity, spawnedTile.transform);
                            // TODO: Make prefab a child of tile, use interface for 
                            // ->
                        }
                    }
                    tiles[currentPosition] = spawnedTile;
                }
                currentPosition = new Vector2(++currentPosition.x, currentPosition.y);
            }
            currentPosition = new Vector2(0, ++currentPosition.y);
        }

        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
        SetupGrid();
    }

    private void SetupGrid() 
    {
        SetHoveredTile(tiles.Keys.ToList().First());

        // Update pivot to be middle tile
        Vector3 centerTilePos = tiles.Values.ToList()[tiles.Count / 2].transform.position;
        transform.position = centerTilePos;

        // Become a daddy to all tiles
        tiles.Values.ToList().ForEach(t => t.transform.SetParent(transform));
    }

    public override void IllegalAction()
    {
        shakeBehaviour.ShakeForTime(timeInSeconds: .15f, () => {
            Debug.Log("Shaking finished!");
        });
    }
}
