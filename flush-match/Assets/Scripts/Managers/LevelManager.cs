using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string jsonFileNameTemplate = "Level{0}.json";

    public LevelData LoadLevelData(int levelNumber)
    {
        string jsonFileName = string.Format(jsonFileNameTemplate, levelNumber);
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
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
