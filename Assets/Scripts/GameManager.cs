using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action<GridManager> onLevelChange;
    private TextAsset currentLevel;
    private TextGridManager grid;

    [SerializeField] private int currentLevelId;
    [SerializeField] private TextGridManager gridPrefab;

    void Start()
    {
        LoadLevel(currentLevelId);
    }

    public void LoadLevel(int levelNumber)
    {
        UnloadCurrentLevel();

        currentLevel = Resources.Load<TextAsset>($"Levels/level_{levelNumber}");
        currentLevelId = levelNumber;
        grid = Instantiate(gridPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        grid.Init(this);
        grid.GenerateGrid(currentLevel);

        // Invoke delegate
        onLevelChange?.Invoke(grid);
    }

    public void LoadLevel()
    {
        int nextLevel = currentLevelId + 1;
        LoadLevel(nextLevel);
    }

    public void UnloadCurrentLevel()
    {
        Destroy(grid?.gameObject);
    }
}
