using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public string ShuffleCnt = "ShuffleCnt";
    public string JokerCnt = "JokerCnt";
    public string TimerCnt = "TimerCnt";
    public string AutoCnt = "AutoCnt";

    private float time;
    private float maxTime;
    private Dictionary<string, int> items = new Dictionary<string, int>();
    private int[] stars;

    private GameManager gameManager;
    private BoardManager boardManager;

    public void Initialize(GameManager gameManager, BoardManager boardManager)
    {
        this.gameManager = gameManager;
        this.boardManager = boardManager;
    }

    public void initItemCnt(Dictionary<string, int> initItemCnt, float time, int[] stars)
    {
        if (initItemCnt != null)
        {
            this.time = time;
            maxTime = time;
            this.stars = stars;
            foreach (var cnt in initItemCnt)
            {
                string key = cnt.Key;
                items[key] = cnt.Value;
            }
        }
        else
        {
            Debug.LogError("Failed to initialize item counts. No data provvidede");
        }
    }
    public float GetTime()
    {
        return time;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }

    public void ActiveShuffleEvent()
    {
        // not yet
        if (items[ShuffleCnt] > 0)
        {
            items[ShuffleCnt]--;
        }
    }
    public void ActiveTimerEvenet()
    {
        if (items[TimerCnt] > 0)
        {
            if (time + 20 < maxTime)
                time += 20;
            else
                time = maxTime;
            items[TimerCnt]--;
        }
    }

    public void ActiveJockerEvent()
    {
        if (items[JokerCnt] > 0)
        {
            boardManager.ChangeJoker(randomTile());
            items[JokerCnt]--;
        }
    }

    public void ActiveAutoEvent()
    {
        // not yet
        if (items[AutoCnt] > 0)
        {
            items[AutoCnt]--;
        }
    }

    public Dictionary<string, int> GetItemCounts()
    {
        return items;
    }

    public int GetStarsPos(int idx)
    {
        return stars[idx];
    }

    TileType randomTile()
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
