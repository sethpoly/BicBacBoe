using System;
using UnityEngine;
using TileSpriteRender;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject highlight;


    [SerializeField] private Sprite finishSprite;
    [SerializeField] private Sprite edgeSprite;
    [SerializeField] private Sprite cornerSprite;
    [SerializeField] private Sprite centerSprite;

    public Vector2 _location { get; private set; }
    [SerializeField] private TileOrientation startingOrientation;
    [SerializeField] public Piece associatedPiece { get; private set; }

    public void Init(bool isOffset, Vector2 location, TileOrientation tileOrientation)
    {
        _renderer.color = isOffset ? offsetColor : baseColor;
        _location = location;
        startingOrientation = tileOrientation;

        SetSprite(startingOrientation);
    }

    /// All inheriting Tiles must specify a tile type
    public abstract TileType GetTileType();

    private void SetSprite(TileOrientation tileOrientation)
    {
        switch(tileOrientation)
        {
            case TileOrientation.NorthLeftCorner:
            case TileOrientation.EastLeftCorner:
            case TileOrientation.SouthLeftCorner:
            case TileOrientation.WestLeftCorner:
            case TileOrientation.NorthRightCorner:
            case TileOrientation.EastRightCorner:
            case TileOrientation.SouthRightCorner:
            case TileOrientation.WestRightCorner:
            _renderer.sprite = cornerSprite;
             break;
            case TileOrientation.Center:
            _renderer.sprite = centerSprite;
            break;
            default:
            _renderer.sprite = edgeSprite;
            break;
        }

        transform.Rotate(0, 0, tileOrientation.GetAttributeOfType<TileSpriteRotation>().rotation);
    }

    // TODO: Remove - testing only
    public void MakeFinish()
    {
        _renderer.sprite = finishSprite;
        //tileType = TileType.Finish;
    }

    public void OnHover()
    {
        highlight.SetActive(true);
    }

    public void OnHoverExit()
    {
        highlight.SetActive(false);
    }

    public void SetAssociatedPiece(Piece piece)
    {
        associatedPiece = piece;
    }
}

namespace TileSpriteRender 
{
    // Types of tiles for each level
    public enum TileType
    {
        Normal,
        Finish
    }

    public enum TileOrientation
    {

        [TileSpriteRotation(0)] 
        NorthLeftCorner,

        [TileSpriteRotation(0)]
        NorthEdge,

        [TileSpriteRotation(270)]
        NorthRightCorner,

        [TileSpriteRotation(270)]
        EastLeftCorner,

        [TileSpriteRotation(270)]
        EastEdge,

        [TileSpriteRotation(180)]
        EastRightCorner,

        [TileSpriteRotation(0)]
        Center,

        [TileSpriteRotation(90)]
        SouthLeftCorner,

        [TileSpriteRotation(180)]
        SouthEdge,

        [TileSpriteRotation(90)]
        SouthRightCorner,

        [TileSpriteRotation(90)]
        WestLeftCorner,

        [TileSpriteRotation(90)]
        WestEdge,

        [TileSpriteRotation(180)]
        WestRightCorner,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TileSpriteRotation : Attribute
    {
        public float rotation { get; }

        public TileSpriteRotation(float _rotation)
        {
            rotation = _rotation;
        }
    }
}


