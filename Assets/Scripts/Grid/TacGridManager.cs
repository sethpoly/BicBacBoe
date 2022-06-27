using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Extends from GridManager and manages additional Tic-Tac-Toe game logic
/// and win condition checking
public class TacGridManager : GridManager
{
    override public bool CheckWinCondition()
    {
        return false;
    }
}
