using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public string jsonFileName = "Level1.json";
    public float xOffset;
    public float yOffset;
    public int rows;
    public int cols;

    public GameObject tilePrefab;

    void Start()
    {
        LoadLevelData();

        // tileMatch test
        int[,] board = new int[rows, cols];
        Vector2 start = new Vector2(2, 3);
        Vector2 end = new Vector2(4, 5);

        bool isMatch = TileMatcher.AreTilesMatching(start, end, board);
        Debug.Log(isMatch);
    }

    // load json file & call GenerateTiles(levelData)
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

            //Print tiles
            GenerateTiles(levelData);

            Debug.Log("Success Level Data load: " + jsonFileName);
        }
        else
        {
            Debug.LogError("Fail to find Level data: " + jsonFileName);
        }
    }

    // print tiles
    void GenerateTiles(LevelData levelData)
    {
        // get tile prefabs's size
        Vector3 tileSize = tilePrefab.GetComponent<Renderer>().bounds.size;

        // edit tiles position
        float totalWidth = (levelData.columns * tileSize.x);
        float totalHeight = (levelData.rows * tileSize.y);

        xOffset = -totalWidth / 2 + tileSize.x / 2;
        yOffset = totalHeight / 2 - tileSize.y / 2;

        // print tiles
        for (int row = 0; row < levelData.rows; row++)
        {
            for (int col = 0; col < levelData.columns; col++)
            {
                // calc index
                int index = row * levelData.columns + col; // levelData: 1dimensional, tiles: 2d

                // get tile's value
                string tileType = levelData.tiles[index];

                // if levelData.tiles[index] value is "Empty" will not show
                if (tileType == "EMPTY")
                    continue;

                // calculate tile positions
                float xPositin = col * tileSize.x + xOffset;
                float yPosition = -row * tileSize.y + yOffset;

                // create tile & initialize
                Vector3 posiiton = new Vector3(xPositin, yPosition, 0);
                GameObject newTile = Instantiate(tilePrefab, posiiton, Quaternion.identity);

                MakeTile tile = newTile.GetComponent<MakeTile>();
                tile.Type = tileType;
                tile.Initialize(row, col);
            }
        }
    }
}
