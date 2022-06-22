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

    private float playerInput = 0;

    // Current user selection for
    private TileChoice currentSelection = TileChoice.Left;

    void Awake()
    {
        playerControl = new PlayerControl();
    }

    void Start()
    {
        playerControl.Player.Enable();
        PlayerInput();
    }

    private void Update() 
    {

    }

    private void PlayerInput()
    {
        playerControl.Player.Move.started += OnPlayerChangedTile;
    }

    private void OnPlayerChangedTile(InputAction.CallbackContext context) 
    {
        playerInput = context.ReadValue<float>();
        currentSelection = NormalizePlayerInput(playerInput);
        grid.SetHoveredTile(currentSelection);
    }

    private TileChoice NormalizePlayerInput(float input)
    {
        var rawTile = (int)currentSelection + (int)input;
        var typedTile = (TileChoice)rawTile;
        List<TileChoice> tiles = Enum.GetValues(typeof(TileChoice)).Cast<TileChoice>().ToList();

        if(tiles.Contains(typedTile)) 
            return typedTile;

        // Change tile to upper or lower bound
        if(rawTile < 0) 
            return tiles.Last();
         
        return tiles.First();
    }
}
