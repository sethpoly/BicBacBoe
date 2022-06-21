using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Update() 
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        playerInput = playerControl.Player.Move.ReadValue<float>();

        // TODO: Update current selection - make maintainable
        currentSelection = NormalizePlayerInput(playerInput);
        Debug.Log("Current selection -> " + playerInput);
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
