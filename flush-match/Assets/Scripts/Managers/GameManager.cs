using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using static UserData;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int currentLevel = 1;

    public GameObject tilePrefab;

    private DataManager dataManager;
    private BoardManager boardManager;
    private ItemManager itemManager;
    private UIManager uiManager;
    private Tile tile;
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
        boardManager.Initialize(this, itemManager);
        itemManager= FindeComponet<ItemManager>();
        uiManager= FindeComponet<UIManager>();

        if (dataManager != null && boardManager != null && itemManager != null && uiManager != null)
        {
            InitialGame(currentLevel);
            itemManager.Initialize(this, boardManager);
            uiManager.Initialize(this, itemManager);
        }
    }

    private void Update()
    {
        if (itemManager.GetTime() > 0)
        {
            itemManager.SetTime(itemManager.GetTime() - Time.deltaTime);
            uiManager.TimerSlider.value = itemManager.GetTime();
        }
    }

    private T FindeComponet<T>() where T : MonoBehaviour
    {
        T component = FindObjectOfType<T>();
        if (component == null)
            Debug.Log(typeof(T).Name + " could not be found in the scenes");

        return component;
    }

    // load json file & call GenerateTiles(levelData)
    public void InitialGame(int levelNumber)
    {
        UserData userData = dataManager.LoadUserData();
        LevelData levelData = dataManager.LoadLevelData(levelNumber);
        if (levelData != null)
        {
            SetUserInfo(userData, levelData.timer, levelData.stars);
            boardManager.GenerateTiles(levelData, tilePrefab);
        }
    }

    public void SetUserInfo(UserData userData, float time, int[] stars)
    {
        itemManager.SetTime(time);
        uiManager.TimerSlider.maxValue = time;
        uiManager.TimerSlider.value = time;
        uiManager.ShuffleCntTxt.text = userData.itemCnt.ShuffleCnt.ToString();
        uiManager.TimerCntTxt.text = userData.itemCnt.TimerCnt.ToString();
        uiManager.JokerCntTxt.text = userData.itemCnt.JokerCnt.ToString();
        uiManager.AutoCntTxt.text = userData.itemCnt.AutoCnt.ToString();

        Dictionary<string, int> items = new Dictionary<string, int>()
        {
            {itemManager.ShuffleCnt, userData.itemCnt.ShuffleCnt},
            {itemManager.TimerCnt, userData.itemCnt.TimerCnt},
            {itemManager.JokerCnt, userData.itemCnt.JokerCnt},
            {itemManager.AutoCnt, userData.itemCnt.AutoCnt}
        };
            
        itemManager.initItemCnt(items, time, stars);
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
