using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int currentLevel = 1;

   /* public Vector2 start;
    public Vector2 end;*/
    public GameObject tilePrefab;

    private DataManager dataManager;
    private BoardManager boardManager;
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

        if (dataManager != null && boardManager != null)
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
            boardManager.GenerateTiles(levelData, tilePrefab);
    }

    // Tile Click event
    public void OnTileClicked(Tile clickedTile)
    {
        if (clickedTile == null)
        {
            Debug.LogError("Clicked tile is null");
            return;
        }
        if (firstSelectedTile == null)
        {
            firstSelectedTile = clickedTile;
            Debug.Log("First tile selected: " + firstSelectedTile.Type);
        }
        else if (secondSelectedTile == null && clickedTile != firstSelectedTile)
        {
            secondSelectedTile = clickedTile;
            Debug.Log("Second tile selected: " + secondSelectedTile.Type);

            boardManager.CheckMatch(firstSelectedTile, secondSelectedTile);
            ResetSelection();
            //CheckMatch();
        }
    }

    /*void CheckMatch()
    {
        if (firstSelectedTile == null || secondSelectedTile == null)
        {
            Debug.LogError("One of the tiles is null. Cannot proced with matching");
            return;
        }
        if (firstSelectedTile.Type == secondSelectedTile.Type)
        {
            start = new Vector2(firstSelectedTile.Row, firstSelectedTile.Col);
            end = new Vector2(secondSelectedTile.Row, secondSelectedTile.Col);

            if (boardManager.AreTilesMatching(start, end, board))
            {
                boardManager.HandleMatch(firstSelectedTile, secondSelectedTile);
            }
        }
        else
        {
            Debug.Log("Tiles do not match");
        }

        ResetSelection();
    }*/
    void ResetSelection()
    {
        firstSelectedTile = null;
        secondSelectedTile = null;
    }
}
