using System.Collections;
using System.Collections.Generic;
using TileSpriteRender;
using UnityEngine;

public class NormalTile : Tile
{
    public override TileType GetTileType()
    {
        return TileType.Normal;
    }
}
