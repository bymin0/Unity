using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int currentLevel = 1;

    public GameObject tilePrefab;

    private LevelManager levelManager;
    private TileGenerator tileGenerator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        tileGenerator = FindObjectOfType<TileGenerator>();
        if (levelManager == null)
            Debug.Log("LevelManager could not be found in the scene.");
        if (tileGenerator == null)
            Debug.Log("TileGenerator could not be found in the scenes");
        if (levelManager != null && tileGenerator != null)
            LoadLevel(currentLevel);
    }

    // load json file & call GenerateTiles(levelData)
    public void LoadLevel(int levelNumber)
    {
        LevelData levelData = levelManager.LoadLevelData(levelNumber);
        if (levelData != null)
            tileGenerator.GenerateTiles(levelData, tilePrefab);
    }
}
