using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Linq;
using TileSpriteRender;

public class TextGridManager : GridManager
{
    public TextMapping[] mappingData;
    private Vector2 currentPosition = new Vector2(0, 0);

    void Awake()
    {
        // Get main camera
        cam = Camera.main.transform;
    }

    void Start()
    {
        shakeBehaviour = GetComponent<ShakeBehaviour>();
    }

    private int GetMapWidth(TextAsset level)
    {
        string[] rows = Regex.Split(level.text, "\r\n|\r|\n");
        string longest = rows.OrderByDescending( s => s.Length ).First();
        return longest.Length;
    }

    private int GetMapHeight(TextAsset level)
    {
       string[] rows = Regex.Split(level.text, "\r\n|\r|\n");
       return rows.Length;
    }

    override public void GenerateGrid(TextAsset level)
    {
        tiles = new Dictionary<Vector2, Tile>();
        string[] rows = Regex.Split(level.text, "\r\n|\r|\n");
        width = GetMapWidth(level);
        int height = GetMapHeight(level);

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

        // TODO: remove, temporary testing
        tiles.Last().Value.MakeFinish();

        // Re-align camera & set new grid pivot point
        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
        SetupGrid();
    }

    private void SetupGrid() 
    {
        SetHoveredTile(tiles.Keys.ToList().First());

        // Update pivot to be middle tile
        transform.position = GetMeanCenter(tiles.Values.Select(t => t.transform).ToList());

        // Become a daddy to all tiles
        tiles.Values.ToList().ForEach(t => t.transform.SetParent(transform));

        // Notify delegate
        //onSetupCompleted();
    }

    /// <summary>
    /// Get center pos of this GameObject by using the list 
    /// of child obj transform to determine it
    /// </summary>
    private Vector3 GetMeanCenter(List<Transform> transforms)
    {
        Vector3 sumVector = Vector3.zero;
        foreach(Transform obj in transforms)
        {
            sumVector += obj.position;
        }
        return sumVector / transforms.Count();
    }

    public override void IllegalAction()
    {
        shakeBehaviour.ShakeForTime(timeInSeconds: .15f, () => {
            Debug.Log("Shaking finished!");
        });
    }
}
