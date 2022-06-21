using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    private float playerInput = 0;

    // Current user selection for
    private TileChoice currentSelection = TileChoice.Left;

    private void Update() {
        PlayerInput();
    }

    private void PlayerInput()
    {
        playerInput = Input.GetAxis("Horizontal");

        // TODO: Update current selection - make maintainable
    }
}
