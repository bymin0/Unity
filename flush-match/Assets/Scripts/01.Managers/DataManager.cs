using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string jsonFileLevelNameTemplate = "Level{0}.json";
    private string rankJsonFile = "rankData.json";
    private string player = "Player";
    private string maxLevelFile = "maxLevel.json";

    public void SaveUserData(int slot, string playerName, int bestScore, int bestStage)
    {
        UserData user = new UserData(playerName, bestScore, bestStage);
        GameData.SaveSlotData(slot, user);

        string jsonData = JsonUtility.ToJson(user);
        PlayerPrefs.SetString($"Slot{slot}", jsonData);
        PlayerPrefs.Save();
    }

    public UserData LoadUserData(int slot)
    {
        string key = $"Slot{slot}";
        if(PlayerPrefs.HasKey(key))
        {
            string jsonData = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<UserData>(jsonData);
        }
        return null;
    }

    public UserData SaveNLoadUserData(int slot, string playerName, int bestScore, int bestStage)
    {
        SaveUserData(slot, playerName, bestScore, bestStage);

        return LoadUserData(slot);
    }

    public LevelData LoadLevelData(int levelNumber)
    {
        string jsonFileName = string.Format(jsonFileLevelNameTemplate, levelNumber);
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {            
            string jsonData = File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonData);
            return levelData;
        }
        else
        {
            //Debug.Log("Failed to find level data: " + jsonFileName);
            return null;
        }
    }

    public int LoadMaxLevel()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, maxLevelFile);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            LastLevelData levelData = JsonUtility.FromJson<LastLevelData>(jsonData);
            return levelData.maxLevel;
        }
        else
        {
            //Debug.Log("Failed to find level data: " + filePath);
            return 1;
        }
    }
    public void SaveRankData(RankList rankList)
    {
        string jsonFile = JsonUtility.ToJson(rankList);
        System.IO.File.WriteAllText(rankJsonFile, jsonFile);
        Debug.Log("Rank Data Save!");
    }
}
