using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string jsonFileLevelNameTemplate = "Level{0}.json";
    public string jsonFileUserInfo = "UserInfo.json";

    public UserData LoadUserData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileUserInfo);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            UserData userData = JsonUtility.FromJson<UserData>(jsonData);
            Debug.Log("Successfully loaded level data: " + filePath);
            return userData;
        }
        else
        {
            Debug.Log("Failed to find level data: " + jsonFileUserInfo);
            return null;
        }
    }

    public LevelData LoadLevelData(int levelNumber)
    {
        string jsonFileName = string.Format(jsonFileLevelNameTemplate, levelNumber);
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            //TextAsset textAsset = Resources.Load<TextAsset>(jsonFileName);
            //textAsset.text;
            
            string jsonData = File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonData);
            Debug.Log("Successfully loaded level data: " + jsonFileName);
            return levelData;
        }
        else
        {
            Debug.Log("Failed to find level data: " + jsonFileName);
            return null;
        }
    }
}
