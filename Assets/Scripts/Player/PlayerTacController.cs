using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTacController : MonoBehaviour
{
    [SerializeField] private TacGridManager grid;
    private PlayerControl playerControl;
    [SerializeField] private int currentTileSelection = 0;

    private Vector2 currentTile;

    void Awake()
    {
        playerControl = new PlayerControl();
    }

    void Start()
    {
        currentTile = new Vector2(0, 0);
        playerControl.Player.Enable();
        PlayerInput();
    }

    private void PlayerInput()
    {
        playerControl.Player.Move.started += OnPlayerChangedTile;
        playerControl.Player.Rotate.started += OnPlayerRotate;
        playerControl.Player.PlacePiece.started += OnPlayerAction;
    }

    /// Changing tile hover selection
    private void OnPlayerChangedTile(InputAction.CallbackContext context) 
    {
        float playerInput = context.ReadValue<float>();
        //currentTileSelection = GetNextTileFromInput(playerInput, grid._direction);

        var possibleTiles = grid.GetTileSubset(grid._direction, currentTile);
        int _cIndex = possibleTiles.FindIndex(t => t._location == currentTile);
        currentTile = possibleTiles[GetNextTileFromInput(_cIndex)]._location;
        grid.SetHoveredTile(currentTile);
    }

    /// Request to rotate grid
    private void OnPlayerRotate(InputAction.CallbackContext context) 
    {
        grid.RotateGrid();
        //currentTileSelection = GetNextTileFromInput(currentTileSelection + grid._width);
    }

    /// Request to play an action
    private void OnPlayerAction(InputAction.CallbackContext context) 
    {
        grid.PlacePieceAtLocation(currentTileSelection, Pieces.PieceType.Exe);
    }

    private int GetNextTileFromInput(float input)
    {
        var nextTile = currentTileSelection + (int)input;
        int tileCount = grid._width;

        if(nextTile < 0) {
            return tileCount - 1;
        }

        if(nextTile >= tileCount) {
            return 0;
        }
        return nextTile;
    }
}
