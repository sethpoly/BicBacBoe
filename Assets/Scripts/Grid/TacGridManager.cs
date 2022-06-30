using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pieces;

/// Extends from GridManager and manages additional Tic-Tac-Toe game logic
/// and win condition checking
public class TacGridManager : GridManager
{

    [SerializeField] Piece piecePrefab;

    override public bool CheckWinCondition()
    {
        return false;
    }

    public void PlacePieceAtLocation(Vector2 tileLocation, PieceType type)
    {
        Tile tile = GetTileAtPosition(tileLocation);

        if(tile.associatedPiece == null)
        {
            Piece spawnedPiece = Instantiate(piecePrefab, tile.transform.position, Quaternion.identity);
            spawnedPiece.Init(tile, type);
            tile.SetAssociatedPiece(spawnedPiece);
        } else 
        {
            Debug.Log("Oops, Tile position occupied.");
        }
    }
}
