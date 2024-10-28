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

    private DataManager dataManager;
    private BoardManager boardManager;
    private ItemManager itemManager;
    // save clicked tile info
    private Tile firstSelectedTile = null;
    private Tile secondSelectedTile = null;

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
        dataManager = FindeComponet<DataManager>();
        boardManager = FindeComponet<BoardManager>();
        itemManager= FindeComponet<ItemManager>();

        if (dataManager != null && boardManager != null && itemManager != null)
            LoadLevel(currentLevel);
    }

    private T FindeComponet<T>() where T : MonoBehaviour
    {
        T component = FindObjectOfType<T>();
        if (component == null)
            Debug.Log(typeof(T).Name + " could not be found in the scenes");

        return component;
    }

    // load json file & call GenerateTiles(levelData)
    public void LoadLevel(int levelNumber)
    {
        LevelData levelData = dataManager.LoadLevelData(levelNumber);
        if (levelData != null)
        {
            boardManager.GenerateTiles(levelData, tilePrefab);
            itemManager.SetTime(levelData.timer);
        }
    }

    // Tile Click event
    public void OnTileClicked(Tile clickedTile)
    {
        Debug.Log("clickedTile info\nrow: " + clickedTile.Row + ", col: " + clickedTile.Col + ", type: " + clickedTile.Type + ", idx = [" + clickedTile.Row + ", "+clickedTile.Col + "]");
        if (clickedTile == null)
        {
            Debug.LogError("Clicked tile is null");
            return;
        }
        if (firstSelectedTile == null)
        {
            firstSelectedTile = clickedTile;
        }
        else if (secondSelectedTile == null && clickedTile != firstSelectedTile)
        {
            secondSelectedTile = clickedTile;

            boardManager.CheckMatch(firstSelectedTile, secondSelectedTile);
            ResetSelection();
        }
    }

    void ResetSelection()
    {
        firstSelectedTile = null;
        secondSelectedTile = null;
    }
}
