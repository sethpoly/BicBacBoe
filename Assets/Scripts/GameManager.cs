using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action<GridManager> onLevelChange;
    private TextAsset currentLevel;
    private TextGridManager grid;

    [SerializeField] private int startLevel;
    [SerializeField] private TextGridManager gridPrefab;

    void Start()
    {
        LoadLevel(startLevel);
    }

    public void LoadLevel(int levelNumber)
    {
        UnloadCurrentLevel();

        currentLevel = Resources.Load<TextAsset>($"Levels/level_{levelNumber}");
        grid = Instantiate(gridPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        grid.GenerateGrid(currentLevel);

        // Invoke delegate
        onLevelChange?.Invoke(grid);
    }

    public void UnloadCurrentLevel()
    {
        Destroy(grid?.gameObject);
    }
}
