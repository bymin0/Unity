using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int rows;
    public int cols;
    public string jsonFileName = "Level1.json";

    void Start()
    {
        LoadLevelData();
    }

    void LoadLevelData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        Debug.Log("file path: " + filePath);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData> (jsonData);

            rows = levelData.rows;
            cols = levelData.columns;
            // depth & tile data

            Debug.Log("Success Level Data load: " + jsonFileName);
        }
        else
        {
            Debug.LogError("Fail to find Level data: " + jsonFileName);
        }
    }
}
