using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using TileSpriteRender;
using GridAttributes;

namespace GridAttributes 
{
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

    public enum GridDirection
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
}

public abstract class GridManager : MonoBehaviour, IShakeable
{
    public int _width { get { return width; } private set { _width = value; } }
    [SerializeField] public int width;
    [SerializeField] public Tile tilePrefab;
    public Transform cam;
    public Dictionary<Vector2, Tile> tiles { get; set; }
    public GridDirection _direction { get { return direction; } private set { _direction = value; } }
    [SerializeField] private GridDirection direction = GridDirection.North;
    [SerializeField] private Boolean rotating = false;
    [SerializeField] private float rotateDuration = 0.5f;
    public ShakeBehaviour shakeBehaviour { get; set; }

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        shakeBehaviour = GetComponent<ShakeBehaviour>();
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

    public void RotateGrid(bool isClockwise)
    {
        if(!rotating) 
        {
            float degrees = isClockwise ? -90 : 90;
            direction = isClockwise ? direction.Next() : direction.Previous();
            StartCoroutine(Rotate90(degrees));
        }
    }

    IEnumerator Rotate90(float degrees)
    {
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, degrees);
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


    // Apply hover modifier to specified tile
    public void SetHoveredTile(Vector2 nextTile) {
        tiles.Values.ToList().ForEach(t => t.OnHoverExit());
        GetTileAtPosition(nextTile).OnHover();

        // TODO: Check level end?
        CheckWinCondition(GetTileAtPosition(nextTile));
    }

    /// <summary>
    /// Get subset of <c>Tile</c> against the specified grid direction
    /// - tile count returned = _width
    /// </summary>
    public List<Tile> GetTileSubset(GridDirection direction, Vector2 currentTile) 
    {
        var tilesList = new List<Tile>(tiles.Values);
        var result = new List<Tile>();

        // Retrieve list of relevant possible tiles
        result = new List<Tile>(tilesList.FindAll(
            delegate(Tile t)
            {
                if(direction.GetAttributeOfType<GridDirectionAttribute>().sortByX)
                    return t._location.y == currentTile.y;
                return t._location.x == currentTile.x;
            }
        ));

        // Sort the tiles by unique order
        return OrderTilesByDirection(result, direction);
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

    public abstract void GenerateGrid(TextAsset text);
    public abstract void IllegalAction();
    public abstract void CheckWinCondition(Tile currentTile);
}
