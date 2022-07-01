using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pieces;

/// Extends from GridManager and manages additional Tic-Tac-Toe game logic
/// and win condition checking
public class TacGridManager : GridManager
{
    [SerializeField] public Piece piecePrefab;

    public PossibleWinner CheckWinCondition(Tile lastTilePlaced)
    {
        PieceType pieceTypeToCheck = lastTilePlaced.associatedPiece.type;
        int col, row, diag, rdiag;
        col = row = diag = rdiag = 0;

        for(int i = 0; i < _width; i++)
        {
            Piece nextColPiece = tiles[new Vector2(lastTilePlaced._location.x, i)].associatedPiece;
            if(nextColPiece != null) {
                if(nextColPiece.type == pieceTypeToCheck) {
                    col++;
                }
            }

            Piece nextRowPiece = tiles[new Vector2(i, lastTilePlaced._location.y)].associatedPiece;
            if(nextRowPiece != null) {
                if(nextRowPiece.type == pieceTypeToCheck) {
                    row++;
                }
            }

            Piece nextDiagPiece = tiles[new Vector2(i, i)].associatedPiece;
            if(nextDiagPiece != null) {
                if(nextDiagPiece.type == pieceTypeToCheck) {
                    diag++;
                }
            }


            Piece nextRdiagPiece = tiles[new Vector2(i, _width - (i + 1))].associatedPiece;
            if(nextRdiagPiece != null) {
                if(nextRdiagPiece.type == pieceTypeToCheck) {
                    rdiag++;
                }
            }
        }

        if(row == _width || col == _width || diag == _width || rdiag == _width) 
            return new PossibleWinner(_didWin: true, _pieceType: pieceTypeToCheck);
        
        return new PossibleWinner();
    }

    public void PlacePieceAtLocation(Vector2 tileLocation, PieceType type)
    {
        Tile tile = GetTileAtPosition(tileLocation);

        if(tile.associatedPiece == null)
        {
            Piece spawnedPiece = Instantiate(piecePrefab, tile.transform.position, Quaternion.identity);
            spawnedPiece.Init(tile, type);
            tile.SetAssociatedPiece(spawnedPiece);
            Debug.Log("Winner? -> " + CheckWinCondition(tile).ToString());
        } else 
        {
            Debug.Log("Oops, Tile position occupied.");
        }
    }
}
