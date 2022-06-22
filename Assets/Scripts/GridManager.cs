using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class GridDirectionAttribute : Attribute
{
    public bool sortByX { get; }
    public SortDirection sortDirection { get; }
    public AggregateIndex aggregateIndex { get; }

    public GridDirectionAttribute(
        bool _sortByX,
        SortDirection _sortDirection,
        AggregateIndex _aggregateIndex
        )
    {
        sortByX = _sortByX;
        sortDirection = _sortDirection;
        aggregateIndex = _aggregateIndex;
    }
}

public enum SortDirection
{
    Ascending,
    Descending
}

public enum AggregateIndex
{
    First,
    Last
}

enum GridDirection
{
    [GridDirectionAttribute(_sortByX: true, _sortDirection: SortDirection.Ascending, _aggregateIndex: AggregateIndex.Last)]
    North,

    [GridDirectionAttribute(_sortByX: false, _sortDirection: SortDirection.Descending, _aggregateIndex: AggregateIndex.Last)]
    East,

    [GridDirectionAttribute(_sortByX: true, _sortDirection: SortDirection.Descending, _aggregateIndex: AggregateIndex.First)]
    South,

    [GridDirectionAttribute(_sortByX: false, _sortDirection: SortDirection.Ascending, _aggregateIndex: AggregateIndex.First)]
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
    private GridDirection direction = GridDirection.North;
    

    void Start()
    {
        GenerateGrid();
        SetupGrid();
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
                Vector2 location = new Vector2(x,y);
                spawnedTitle.Init(isOffset, location);

                // Append to dict
                tiles[location] = spawnedTitle;
            }
        }

        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f, -10);
    }

    private void SetupGrid() 
    {
        SetHoveredTile(TileChoice.Left);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(tiles.TryGetValue(pos, out var tile)) 
        {
            return tile;
        }
        return null;
    }

    // Get tile reference from user's constrained choice
    public Tile GetTileFromChoice(TileChoice choice) {
        var tilesList = new List<Tile>(tiles.Values);
        var result = new List<Tile>();

        // Retrieve list of relevant possible tiles
        result = new List<Tile>(tilesList.FindAll(
            delegate(Tile t)
            {
                float location = direction.GetAttributeOfType<GridDirectionAttribute>().sortByX ? t._location.y : t._location.x;
                int aggregateId = direction.GetAttributeOfType<GridDirectionAttribute>().aggregateIndex == AggregateIndex.First
                    ? 0 : width - 1;
                return location == aggregateId;
            }
        ));

        // Sort the tiles by unique order
        result = OrderTilesByDirection(result, direction);
        result.ForEach(i => Debug.Log("Tile result -> " + i));   
        return result[(int)choice];
    }

    // Apply hover modifier to specified tile
    public void SetHoveredTile(TileChoice choice) {
        tiles.Values.ToList().ForEach(t => t.OnHoverExit());
        GetTileFromChoice(choice).OnHover();
    }

    // Order the list of tiles against current grid direction
    private List<Tile> OrderTilesByDirection(List<Tile> tiles, GridDirection dir)
    {
        bool sortByX = direction.GetAttributeOfType<GridDirectionAttribute>().sortByX;
        SortDirection sortOrder = direction.GetAttributeOfType<GridDirectionAttribute>().sortDirection;

        // Sort by x value of tile
        if(sortByX)
        {
            if(sortOrder == SortDirection.Ascending)
                return tiles.OrderBy(t => t._location.x).ToList();
            else
                return tiles.OrderByDescending(t => t._location.x).ToList();
        }

        // Sort by y value of tile
        if(sortOrder == SortDirection.Ascending)
            return tiles.OrderBy(t => t._location.y).ToList();
        else
            return tiles.OrderByDescending(t => t._location.y).ToList();
    }
}
