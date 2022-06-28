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
    private int currentTileSelection = 0;

    void Awake()
    {
        playerControl = new PlayerControl();
    }

    void Start()
    {
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
        currentTileSelection = GetNextTileFromInput(playerInput);
        grid.SetHoveredTile(currentTileSelection);
    }

    /// Request to rotate grid
    private void OnPlayerRotate(InputAction.CallbackContext context) 
    {
        grid.RotateGrid();
        currentTileSelection = 0;
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
