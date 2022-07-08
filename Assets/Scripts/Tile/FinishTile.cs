using System.Collections;
using System.Collections.Generic;
using TileSpriteRender;
using UnityEngine;

public class FinishTile : Tile
{
    public override TileType GetTileType()
    {
        return TileType.Finish;
    }
}
