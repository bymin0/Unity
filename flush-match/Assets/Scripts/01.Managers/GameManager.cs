using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    // tile prefab
    public GameObject tilePrefab;
    
    // game variables
    private int currentLevel = 1;
    private int maxLevel;
    private float time;
    private int coins = 1000;
    private int currCoins = 0;
    private int currentScore = 0;
    public int bestScore { get; private set; }
    public int bestStage { get; private set; }

    public int Score { get; private set; }
    private int lastCombo = 0;
    private int comboCount = 0;
    private float maxComboTimer = 5.0f;
    private float comboTimer = 1.5f;
    private float comboTimeRemaining;
    private bool isComboActive = true;
    private bool isPaused = false;
    private bool isSceneLoaded = false;
    private bool isSceneChange = false;

    // load Scropt
    private MusicManager musicManager;
    private MainMusic mainMusic;
    private DataManager dataManager;
    private BoardManager boardManager;
    private ShopManager shopManager;
    private ItemManager itemManager;
    private GameResult gameResult;
    private GameUIManager uiManager;
    private TopUI topUI;
    private BottomUI bottomUI;
    private PauseUI pauseUI;
    private ClearUI clearUI;
    private UserData userData;
    private TileSpriteLoader tileSpriteLoader;
    
    // save clicked tile info
    private Tile firstSelectedTile = null;
    private GameObject firstTile;
    private Tile secondSelectedTile = null;
    private GameObject secondTile;

    private bool isClear;
    private bool isNotMushTime = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isSceneChange) return;

        time = itemManager.GetTime();
        if (time > 0)
        {
            time -= Time.deltaTime;
            itemManager.SetTime(time);
            topUI.TimerSlider.value = time;
            if (time <= 10 && !isNotMushTime)
                StartCoroutine(NotMuchTimeEffect());
        }
        else
        {
            GoToResult(false);
        }

        if (isComboActive)
        {
            comboTimeRemaining -= Time.deltaTime;

            if (comboTimeRemaining <= 0)
                EndCombo();
        }
    }

    private T FindComponet<T>() where T : MonoBehaviour
    {
        T component = FindObjectOfType<T>();
        if (component == null)
            Debug.Log(typeof(T).Name + " could not be found in the scenes");

        return component;
    }

    public void InitializeComponent()
    {
        musicManager = FindComponet<MusicManager>();
        mainMusic = FindComponet<MainMusic>();
        dataManager = FindComponet<DataManager>();
        tileSpriteLoader = FindComponet<TileSpriteLoader>();
        boardManager = FindComponet<BoardManager>();
        boardManager.Initialize(this, mainMusic, tileSpriteLoader);
        tileSpriteLoader = FindComponet<TileSpriteLoader>();
        itemManager = FindComponet<ItemManager>();
        uiManager = FindComponet<GameUIManager>();
        topUI = FindComponet<TopUI>();
        bottomUI = FindComponet<BottomUI>();
        pauseUI = FindComponet<PauseUI>();
        clearUI = FindComponet<ClearUI>();
        
        if (dataManager != null && boardManager != null && itemManager != null && uiManager != null)
        {
            InitialGame();
            itemManager.Initialize(this, boardManager);
            mainMusic.Initialized(musicManager);
            uiManager.Initialize(this, itemManager,
                mainMusic, topUI, bottomUI, pauseUI, clearUI);
        }
        isSceneLoaded = true;
    }

    public List<UserData> GetUserDatas()
    {
        return null;
    }

    public void SetUserData(bool isClear)
    {
        bestStage = ReturnBestStage();
        if (isClear)
        {
            bestScore = ReturnBestScore(Score);
        }
        else
        {
            Score += currentScore;
            bestScore = ReturnBestScore(Score);
        }
        dataManager.SaveUserData(GameData.CurrentSlotNumber, GameData.CurrentUserData.playerName, bestScore, bestStage);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public void IncreaseCurrLevel()
    {
        currentLevel++;
    }
    public int GetMaxLevel()
    {
        return maxLevel;
    }
    public void AddCoin(int coin)
        { coins += coin; }
    public void ReduceCoin(int coin)
        { coins -= coin; }
    public int GetCurrentScore()
    {
        return currentScore;
    }
    public int GetCurrentCoins()
    {
        return currCoins;
    }
    public int GetCoins()
        { return coins; }
    public float GetMaxComboTime()
    {
        return maxComboTimer;
    }
    public void SetComboTimer(float comboTimer)
    {
        float setComboTimer = this.comboTimer + comboTimer;
        if (setComboTimer < maxComboTimer)
            this.comboTimer = setComboTimer;
        else
            this.comboTimer = maxComboTimer;
        //Debug.Log("Combo one time item! comboTimer: "+ comboTimer);
    }

    public float GetComboTimer()
        { return comboTimer; }
    public void SetIsPaused(bool isPaused)
    {
        this.isPaused = isPaused;
    }
    public bool GetIsPause()
    {
        return isPaused;
    }    

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
    }

    // load json file & call GenerateTiles(levelData)
    public void InitialGame()
    {
        maxLevel = dataManager.LoadMaxLevel();
        LevelData levelData = dataManager.LoadLevelData(currentLevel);
        if (levelData != null)
        {
            isNotMushTime = false;
            currentScore = 0;
            time = levelData.timer;
            boardManager.GenerateTiles(levelData, tilePrefab);
            SetGame(levelData.stars);
        }
    }

    public void GoToShop()
    {
        SceneManager.LoadScene(ConstCollect.Shop);
    }
 
    public void SetGame(int[] stars)
    {
        Dictionary<string, int> itemCnt = itemManager.GetItemCounts();

        topUI.TimerSlider.maxValue = time;
        topUI.TimerSlider.value = time;
        topUI.ScoreCntTxt.text = currentScore.ToString();
        
        bottomUI.ShuffleCntTxt.text = itemCnt[ConstCollect.Shuffle].ToString();
        bottomUI.TimerCntTxt.text = itemCnt[ConstCollect.Timer].ToString();
        bottomUI.AutoCntTxt.text = itemCnt[ConstCollect.Auto].ToString();
        bottomUI.JokerCntTxt.text = itemCnt[ConstCollect.Joker].ToString();
        itemManager.initItem(time, stars);
    }

    // Tile Click event
    public void OnTileClicked(Tile clickedTile)
    {
        if (clickedTile == null)
        {
            Debug.LogError("Clicked tile is null");
            return;
        }
        mainMusic.OnClickedTileSFX();
        if (firstSelectedTile == null)
        {
            firstSelectedTile = clickedTile;
            firstTile = firstSelectedTile.gameObject;

            tileSpriteLoader.CreateOutline(firstTile);
        }
        else if (secondSelectedTile == null && clickedTile != firstSelectedTile)
        {
            secondSelectedTile = clickedTile;
            secondTile = secondSelectedTile.gameObject;

            tileSpriteLoader.CreateOutline(secondTile);

            boardManager.CheckMatch(firstSelectedTile, secondSelectedTile);
            ResetSelection();
        }
    }

    public void HandleCombo()
    {
        if (!isComboActive) // start combo
        {
            comboCount = 1;
            isComboActive = true;
            comboTimeRemaining = comboTimer;
        }
        else // already start combo
        {
            comboCount++;
            comboTimeRemaining = comboTimer;
        }
        if (isSceneLoaded)
            mainMusic.MatchedSFX();
        currentScore += AddScore();
        topUI.SetScore(currentScore);
        uiManager.SetComboTxt(comboCount);
    }

    public void EndCombo()
    {
        isComboActive = false;
        comboCount = 0;
        comboTimeRemaining = 0;
        //Debug.Log("Combo is End");
    }

    public int StageClearSetting(bool isGoNextStage)
    {
        Time.timeScale = 0;
        currentScore += lastCombo;
        currCoins = CalcCoin();
        if (isGoNextStage)
            clearUI.StageClear();
        currentScore -= lastCombo;
        return currCoins;
    }
    
    public void GoToResult(bool isClear)
    {
        if (isSceneChange) return;
        isSceneChange = true;
        StageClearSetting(false);
        SceneManager.LoadScene(ConstCollect.End);
        this.isClear = isClear;
    }

    public void GoNextLevel()
    {
        if (itemManager.GetNextLevel() && currentLevel + 1 < maxLevel)
        {
            IncreaseCurrLevel();
           // Debug.Log("one time goNextLevel!");
        }
    }
    
    public void GoToMain()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(ConstCollect.StartScene);
    }
    public int ActiveToLevel(int kind)
    {
        if (kind == 0)
        {
            if (currentLevel >= 8)
            return 4;
            if (currentLevel >= 6)
                return 3;
            if (currentLevel >= 4)
                return 2;
            if (currentLevel >= 2)
                return 1;
            return 0;
        }
        else if (kind == 1)
        {
            if (currentLevel >= 8)
                return 5;
            if (currentLevel >= 7)
                return 4;
            if (currentLevel >= 5)
                return 3;
            if (currentLevel >= 3)
                return 2;
            if (currentLevel >= 1)
                return 1;
        }
        return 0;
    }

    private void ResetSelection()
    {
        firstSelectedTile = null;
        secondSelectedTile = null;

        tileSpriteLoader.RemoveOutline(firstTile);
        tileSpriteLoader.RemoveOutline(secondTile);
        firstTile = null;
        secondTile = null;
    }

    private int AddScore()
    {
        if (comboCount >= 10)
            return 500;
        if (comboCount >= 5)
            return 250;
        if (comboCount >= 3)
            return 100;
        return 10;
    }
    
    private int CalcCoin()
    {
        int converStarToCoin = 0;
        int returnCoin = 0;
        for (int i = 0; i < 3; i++)
        {
            if (time > itemManager.GetStarsPos(i))
            {
                converStarToCoin = Mathf.FloorToInt(time * (2 - i));
                break;
            }
        }
        lastCombo = AddScore();
        Score += lastCombo + currentScore;
        returnCoin = ((AddScore() + currentScore) / 100) + converStarToCoin;
        
        return returnCoin;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case ConstCollect.Shop:
                InitializeShopScene(); break;
            case ConstCollect.Game:
                InitializeMainScene(); break;
            case ConstCollect.End:
                InitializeResult(); break;

        }
    }

    private void InitializeShopScene()
    {
        isSceneLoaded = false;
        shopManager = FindComponet<ShopManager>();
        if (shopManager != null)
        {
            shopManager.Initialize(this, itemManager, musicManager);
            shopManager.SetRandomItems(itemManager.GetRandomStoreItems());
        }
    }

    private void InitializeMainScene()
    {
        InitializeComponent();
    }

    private void InitializeResult()
    {
        isSceneLoaded = false;
        gameResult = FindComponet<GameResult>();
        if (gameResult != null)
        {
            gameResult.Initialize(this);
            if (isClear)
            {
                gameResult.ClearGame();
            }
            else
            {
                gameResult.FailGame();
            }
        }
    }

    private int ReturnBestScore(int score)
    {
        if (GameData.CurrentUserData.bestScore < score)
            return score;
        return GameData.CurrentUserData.bestScore;
    }
    private int ReturnBestStage()
    {
        if (GameData.CurrentUserData.bestStage < currentLevel)
            return currentLevel;
        return GameData.CurrentUserData.bestStage;
    }

    private IEnumerator NotMuchTimeEffect()
    {
        isNotMushTime = true;

        mainMusic.NoTimeTikTok();

        yield return new WaitForSeconds(mainMusic.TiktokSFX.length);

        isNotMushTime = false;

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
