using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // set item price
    public struct ItemPrice
    {
        public string itemName;
        public string itemSName;
        public int price;
        public Sprite itemSprite;
        public string description;

        public ItemPrice(string name, string sName, int cost, Sprite sprite, string note)
        {
            itemName = name;
            itemSName = sName;
            price = cost;
            itemSprite = sprite;
            description = note;
        }
    }
    private List<ItemPrice> itemPrice;
    public List<Sprite> itemSprite;

    // kind of items
    private float AddComboTime = 0;
    private bool IsGoNextLevel = false;
    private bool IsFrozeTimer = false;
    private bool IsAutoRemove = false;

    public float time { get; set; }
    private float maxTime;
    private Dictionary<string, int> items = new Dictionary<string, int>()
    {
        {ConstCollect.Shuffle, 0 },
        {ConstCollect.Timer, 0 },
        {ConstCollect.Auto, 0 },
        {ConstCollect.Joker, 0 }
    };
    private List<string> oneTimeItems = new List<string> 
    { ConstCollect.Combo1, ConstCollect.Combo2, ConstCollect.JumpLevel, ConstCollect.FrozenTimer, ConstCollect.AutoRemove };
    private List<string> selectedItems = new List<string>();
    private int[] stars;

    private GameManager gameManager;
    private BoardManager boardManager;

    public void InitializedItemImg(List<Sprite> sprite)
    {
        itemPrice = new List<ItemPrice>()
        {
            new ItemPrice(ConstCollect.Shuffle, ConstCollect.ShuffleName, 120, sprite[0], ConstCollect.ShuffleCmt),
            new ItemPrice(ConstCollect.Timer, ConstCollect.TimerName, 135, sprite[1], ConstCollect.TimerCmt),
            new ItemPrice(ConstCollect.Auto, ConstCollect.AutoName, 155, sprite[2], ConstCollect.AutoCmt),
            new ItemPrice(ConstCollect.Joker, ConstCollect.JokerName, 200, sprite[3], ConstCollect.JokerCmt),
            new ItemPrice(ConstCollect.Combo1, ConstCollect.Combo1Name, 70, sprite[4], ConstCollect.AddCombo1Cmt),
            new ItemPrice(ConstCollect.Combo2, ConstCollect.Combo2Name, 100, sprite[5], ConstCollect.AddCombo2Cmt),
            new ItemPrice(ConstCollect.JumpLevel, ConstCollect.JumpLevleName, 10, sprite[6], ConstCollect.JumpLevelCmt),
            new ItemPrice(ConstCollect.FrozenTimer, ConstCollect.FrozenName, 50, sprite[7], ConstCollect.FrozenCmt),
            new ItemPrice(ConstCollect.AutoRemove, ConstCollect.AutoName, 25, sprite[8], ConstCollect.AutoRemoveCmt)
        };
    }

    public void Initialize(GameManager gameManager, BoardManager boardManager)
    {
        this.gameManager = gameManager;
        this.boardManager = boardManager;

        InitializedItemImg(itemSprite);
    }

    public void initItem(float time, int[] stars)
    {
        this.time = time;
        maxTime = time;
        this.stars = stars;
        SettingOneTimeItems();
        AddComboTime = 0;
        IsGoNextLevel = false;
        IsFrozeTimer = false;
        IsAutoRemove = false;
    }
    public float GetTime()
        { return time; }
    public void SetTime(float time)
        { this.time = time; }
    public bool GetNextLevel()
        { return IsGoNextLevel; }
    public void SetNextLevel()
        { IsGoNextLevel = true ; }
    public bool GetFrozeTimer()
        { return IsFrozeTimer; }
    public void SetFrozeTimer()
        { IsFrozeTimer = true; }
    public bool GetAutoRemove()
        { return IsAutoRemove; }
    public void SetAutoRemove()
        { IsAutoRemove = true; }
    public List<string> GetOneTiemItems()
        { return oneTimeItems; }
    public void SetComboTime(float AddComboTime)
    { this.AddComboTime += AddComboTime; }

    public void ActiveShuffleEvent()
    {
        if (items[ConstCollect.Shuffle] > 0 && boardManager.Shuffle())
        {
            items[ConstCollect.Shuffle]--;
        }
    }
    public void ActiveTimerEvenet(int flag = 0)
    {
        if (IsFrozeTimer && flag == 1)
        {
            time += 5;
            //Debug.Log("one time Add timer!");
            IsFrozeTimer = false;
        }
        else if (items[ConstCollect.Timer] > 0)
        {
            if (time + 20 < maxTime)
                time += 20;
            else
                time = maxTime;
            items[ConstCollect.Timer]--;
        }
    }
    public void ActiveJockerEvent()
    {
        if (items[ConstCollect.Joker] > 0)
        {
            boardManager.ChangeJoker(randomTile());
            items[ConstCollect.Joker]--;
        }
    }
    public void ActiveAutoEvent(int flag = 0)
    {
        boardManager.UseAutoMatch();
        if (IsAutoRemove && flag == 1)
        {
            //Debug.Log("one time Auto!");
            IsAutoRemove = false;
        }
        else if (items[ConstCollect.Auto] > 0)
        {
            items[ConstCollect.Auto]--;
        }
    }

    public void ChangeComboTime()
    {
        gameManager.SetComboTimer(AddComboTime);
    }

    public Dictionary<string, int> GetItemCounts()
    {
        return items;
    }

    public List<string> GetRandomStoreItems()
    {
        selectedItems.Clear();
        int idx = 0;
        
        // ragular items
        List<string> itemsKey = new List<string>(items.Keys);
        int currentLevel = gameManager.GetCurrentLevel();
        int ActiveItems = gameManager.ActiveToLevel(1);
        for (int i = items.Count - 1; i >= ActiveItems; i--)
        {
            itemsKey.RemoveAt(i);
        }
        if (currentLevel % 2 == 1 && currentLevel < 7)
        {
            string unlockedItem = itemsKey[ActiveItems - 1];
            selectedItems.Add(unlockedItem);
            if (itemsKey.Count > 1)
                itemsKey.Remove(unlockedItem);
            
            
            idx = Random.Range(0, itemsKey.Count);
            //Debug.Log("itemMange idx: "+idx);
            selectedItems.Add(itemsKey[idx]);
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                idx = Random.Range(0, itemsKey.Count);
                selectedItems.Add(itemsKey[idx]);
            }
        }

        // onetime items
        while (selectedItems.Count < 5)
        {
            string selectItem = oneTimeItems[Random.Range(0, oneTimeItems.Count)];

            if (selectItem == ConstCollect.Combo1 ||selectItem == ConstCollect.Combo2 || !selectedItems.Contains(selectItem))
            {
                selectedItems.Add(selectItem);
            }
        }

        return selectedItems;
    }

    public Sprite GetItemSprite(string itemName)
    { return itemPrice.Find(item => item.itemName == itemName).itemSprite; }
    public string GetItemDescription(string itemName)
    { return itemPrice.Find(item => item.itemName == itemName).description; }
    public int GetItemPrice(string itemName)
    { return itemPrice.Find(item => item.itemName == itemName).price; }
    public string GetItemName(string itemName)
    { return itemPrice.Find(item => item.itemName == itemName).itemSName; }

    public void AddItem(string item)
    {
        items[item]++;
    }

    public int GetStarsPos(int idx)
    {
        return stars[idx];
    }

    public void SettingOneTimeItems()
    {
        if (AddComboTime != 0)
            ChangeComboTime();
        if (IsGoNextLevel)
            gameManager.GoNextLevel();
        if (IsFrozeTimer)
            ActiveTimerEvenet(1);
        if (IsAutoRemove)
            ActiveAutoEvent(1);
    }

    private TileType randomTile()
    {
        HashSet<TileType> uniqueTiles = boardManager.GetUniqueTileTipes();
        int randomTile = Random.Range(0, uniqueTiles.Count);
        int curridx = 0;

        foreach (TileType tileType in uniqueTiles)
        {
            if (curridx == randomTile)
            {
                uniqueTiles.Remove(tileType);
                return tileType; 
            }
            curridx++;
        }

        return new TileType();
    }
}
