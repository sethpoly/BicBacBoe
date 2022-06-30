using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pieces;

/// Extends from GridManager and manages additional Tic-Tac-Toe game logic
/// and win condition checking
public class TacGridManager : GridManager
{

    [SerializeField] Piece piecePrefab;

    override public bool CheckWinCondition(Vector2 lastPlacedPiecePos)
    {
        var col = 0;
        var row = 0;
        var diag = 0;
        var rdiag = 0;
        var winner = false;


        for(int i = 0; i < _width; i++)
        {
            Piece nextColPiece = tiles[new Vector2(lastPlacedPiecePos.x, i)].associatedPiece;
            if(nextColPiece != null) {
                if(nextColPiece.type == PieceType.Exe) {
                    col++;
                }
            }

            Piece nextRowPiece = tiles[new Vector2(i, lastPlacedPiecePos.y)].associatedPiece;
            if(nextRowPiece != null) {
                if(nextRowPiece.type == PieceType.Exe) {
                    row++;
                }
            }

            Piece nextDiagPiece = tiles[new Vector2(i, i)].associatedPiece;
            if(nextDiagPiece != null) {
                if(nextDiagPiece.type == PieceType.Exe) {
                    diag++;
                }
            }


            Piece nextRdiagPiece = tiles[new Vector2(i, _width - (i + 1))].associatedPiece;
            if(nextRdiagPiece != null) {
                if(nextRdiagPiece.type == PieceType.Exe) {
                    rdiag++;
                }
            }
        }

        if(row == _width || col == _width || diag == _width || rdiag == _width) 
            winner = true;
        
        return winner;
    }

    public void PlacePieceAtLocation(Vector2 tileLocation, PieceType type)
    {
        Tile tile = GetTileAtPosition(tileLocation);

        if(tile.associatedPiece == null)
        {
            Piece spawnedPiece = Instantiate(piecePrefab, tile.transform.position, Quaternion.identity);
            spawnedPiece.Init(tile, type);
            tile.SetAssociatedPiece(spawnedPiece);
            Debug.Log("Winner? -> " + CheckWinCondition(tile._location));
        } else 
        {
            Debug.Log("Oops, Tile position occupied.");
        }
    }
}
