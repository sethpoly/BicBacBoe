using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TextAsset currentLevel;
    private TextGridManager grid;

    void Start()
    {
        grid = GameObject.Find("TextGridManager").GetComponent<TextGridManager>();
        LoadLevel(2);
    }

    public void LoadLevel(int levelNumber)
    {
        UnloadCurrentLevel();

        currentLevel = Resources.Load<TextAsset>($"Levels/level_{levelNumber}");
        grid.GenerateGrid(currentLevel);
    }

    public void UnloadCurrentLevel()
    {
        // TODO: 
    }
}
