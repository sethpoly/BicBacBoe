using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject highlight;

    public Vector2 _location;

    public void Init(bool isOffset, Vector2 location)
    {
        _renderer.color = isOffset ? offsetColor : baseColor;
        _location = location;
    }

    public void OnHover()
    {
        highlight.SetActive(true);
    }

    public void OnHoverExit()
    {
        highlight.SetActive(false);
    }
}

namespace TileSpriteRender 
{
    enum TileOrientation
    {
        NorthLeftCorner,
        NorthEdge,
        NorthRightCorner,

        EastLeftCorner,
        EastEdge,
        EastRightCorner,

        SouthLeftCorner,
        SouthEdge,
        SouthRightCorner,

        WestLeftCorner,
        WestEdge,
        WestRightCorner,
    }
}
