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
    [SerializeField] private Vector2 currentTile;

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
        if(playerInput == 0) { return; }

        var _possibleTiles = grid.GetTileSubset(grid._direction, currentTile);
        int _currentTileIndex = _possibleTiles.FindIndex(t => t._location == currentTile);
        int _nextTileIndex = GetNextTileFromInput(_currentTileIndex, playerInput);

        currentTile = _possibleTiles[_nextTileIndex]._location;
        grid.SetHoveredTile(currentTile);
    }

    /// Request to rotate grid
    private void OnPlayerRotate(InputAction.CallbackContext context) 
    {
        float playerInput = context.ReadValue<float>();
        if(playerInput == 0) { return; }
        bool isClockwise = playerInput == 1;
        grid.RotateGrid(isClockwise);
    }

    /// Request to play an action
    private void OnPlayerAction(InputAction.CallbackContext context) 
    {
        grid.PlacePieceAtLocation(currentTile, Pieces.PieceType.Exe);
    }

    private int GetNextTileFromInput(int currentTileIndex, float input)
    {
        int offset = 1;
        if(grid._direction == GridDirection.East || grid._direction == GridDirection.West) 
            offset = -1;

        var nextTile = currentTileIndex + (int)input * offset;
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
