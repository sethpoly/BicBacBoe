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
    [SerializeField] private Sprite _playerSprite;
    [SerializeField] private Sprite _cpuSprite;
    [SerializeField] private Tile associatedTile;
    [SerializeField] private PieceType type;

    public void Init(Tile _associatedTile, PieceType _type)
    {
        associatedTile = _associatedTile;
        type = _type;
        transform.SetParent(associatedTile.transform);

        SetSprite(type);
    }

    private void SetSprite(PieceType pieceType)
    {
        switch(pieceType)
        {
            case PieceType.Exe:
                _renderer.sprite = _playerSprite;
                break;
            case PieceType.Oh:
                _renderer.sprite = _cpuSprite;
                break;
        }
    }

}
