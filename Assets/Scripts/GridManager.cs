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

public class GridManager : MonoBehaviour
{
    public int _width { get { return width; } private set { _width = value; } }
    [SerializeField] private int width;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    public Dictionary<Vector2, Tile> tiles { get; private set; }
    private GridDirection direction = GridDirection.North;
    [SerializeField] private Boolean rotating = false;
    [SerializeField] private float rotateDuration = 0.5f;
    

    void Start()
    {
        GenerateGrid();
    }


    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < width; y++)
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

        cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)width/2 - 0.5f, -10);
        SetupGrid();
    }

    private void SetupGrid() 
    {
        SetHoveredTile(0);

        // Update pivot to be middle tile
        Vector3 centerTilePos = tiles.Values.ToList()[tiles.Count / 2].transform.position;
        transform.position = centerTilePos;

        // Become a daddy to all tiles
        tiles.Values.ToList().ForEach(t => t.transform.SetParent(transform));
    }

    public void RotateGrid()
    {
        if(!rotating) 
        {
            direction = direction.Next();
            StartCoroutine(Rotate90());
            SetHoveredTile();
        }
    }

    IEnumerator Rotate90()
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        while (timeElapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotateDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        rotating = false;
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
    public Tile GetTileFromChoice(int choice) {
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
        return result[choice];
    }

    // Apply hover modifier to specified tile
    public void SetHoveredTile(int nextTile = 0) {
        tiles.Values.ToList().ForEach(t => t.OnHoverExit());
        GetTileFromChoice(nextTile).OnHover();
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
