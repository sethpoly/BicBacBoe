using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridManager grid;
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
    }

    private void OnPlayerChangedTile(InputAction.CallbackContext context) 
    {
        float playerInput = context.ReadValue<float>();
        currentTileSelection = GetNextTileFromInput(playerInput);
        grid.SetHoveredTile(currentTileSelection);
    }

    private void OnPlayerRotate(InputAction.CallbackContext context) 
    {
        grid.RotateGrid();
        currentTileSelection = 0;
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
