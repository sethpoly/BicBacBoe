using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static event Action<GridManager> onLevelChange;
    private TextAsset currentLevel;
    private TextGridManager grid;
    [SerializeField] private TextGridManager gridPrefab;

    void Start()
    {
        grid = GameObject.Find("TextGridManager").GetComponent<TextGridManager>();
        LoadLevel(2);
        StartCoroutine(passiveMe(2));
    }

    IEnumerator passiveMe(int secs)
    {
        yield return new WaitForSeconds(secs);
        LoadLevel(3);
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
         //grid.ResetAndCleanup();
         Destroy(grid.gameObject);
    }

}
