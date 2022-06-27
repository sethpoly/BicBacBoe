using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pieces;

namespace Pieces {
    public enum PieceType
    {
        Exe,
        Oh
    }
}

public class Piece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Tile associatedTile;
    [SerializeField] private PieceType type;

    public void Init(Tile _associatedTile, PieceType _type)
    {
        associatedTile = _associatedTile;
        type = _type;
        transform.SetParent(associatedTile.transform);
    }

}
