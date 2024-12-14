using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;
using System.Linq;
using Random = UnityEngine.Random;


public class StageMaker : MonoBehaviour
{
    [Header("Stage Settings")]
    public int stageNumber = 1;
    public TileType maxTile;

    public float timer = 100;
    public int[] star = new[] { 50, 30, 1 };
    
    
    
    public Sprite[] Sprites; 
    public string savePath = "Assets/StreamingAssets";

    public int width;
    [FormerlySerializedAs("heihgt")] public int height;

    private List<GameObject> grid = new List<GameObject>();
    private int[] tileIDs;
    public GameObject prefab;

    private bool isChanged;
    private int lastLevel;
    
    
    public void Generate()
    {
        for (int i = transform.childCount -1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        grid.Clear();
        
        float offsetX = width / 2f;
        float offsetY = height / 2f;
        
        for (int i = 0; i < width * height; i++)
        {
            int x = i % width;
            int y = i / width;

            GameObject go = Instantiate(prefab, this.transform);
            go.transform.parent = this.transform;
            go.transform.position = new Vector3(x - offsetX + 0.5f, y - offsetY + 0.5f);
            grid.Add(go);
        }

        isChanged = true;
    }

    public void SetTile()
    {
        int[] tiles = GetActiveTileIndex();

        if (tiles.Length % 2 != 0)
        {
            Debug.Log("not 짝수");
            return;
        }

        tileIDs = new int[grid.Count];
        for (int i = 0; i < tileIDs.Length; i++)
            tileIDs[i] = 0;

        int tileCnt = tiles.Length / 2;
        List<int> tileList = new List<int>();
        
        // TileType을 2개씩 추가
        for (int i = 0; i < tileCnt; i++)
        {
            int n = Random.Range(1, (int)maxTile-1);
            tileList.Add(n);
            tileList.Add(n);
        }
        // 타일을 랜덤으로 섞음
        Shuffle(tileList);

        for (int i = 0; i < tiles.Length; i++)
        {
            grid[tiles[i]].GetComponent<SpriteRenderer>().sprite = Sprites[tileList[i]];
            tileIDs[tiles[i]] = tileList[i];
        }

        isChanged = false;
    }
    
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void OnClick(Vector3 postion)
    {
        float offsetX = width / 2f;
        float offsetY = height / 2f;
        int x = (int)((postion.x + offsetX - 0.5f));
        int y = (int)((postion.y + offsetY - 0.5f));

        SpriteRenderer sr = grid[x + y * width].GetComponent<SpriteRenderer>();
        sr.enabled = !sr.enabled;
        isChanged = true;
    }
    
    
    public void Save()
    {
        string folderPath = Path.Combine(Application.dataPath, savePath.TrimStart("Assets/".ToCharArray()));
        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Created folder: {folderPath}");
        }

        string path = Path.Combine(folderPath, $"Level{stageNumber}.json");
        SaveStageToFile(path);
        ChangeMaxLevel(stageNumber); //
    }
 
    public void SaveAs(string customPath) {
        SaveStageToFile(customPath);
    }
    
    public void SaveStageToFile(string path)
    {
        if (isChanged)
        {
            Debug.Log("수정됨 다시 만들기");
            return;
        }
        
        LevelData levelData = new LevelData()
        {
            columns = width,
            depth = 0,
            rows = height,
            stars = new int[] { star[0], star[1], star[2] },
            tiles = tileIDs,
            timer = timer
        };
        Debug.Log("Save");
        
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(path, json);
        Debug.Log($"Stage saved to {path}");
    }
    
    public int[] GetActiveTileIndex()
    {
        return grid
            .Select((obj, index) => new { obj, index }) // 오브젝트와 인덱스를 함께 선택
            .Where(item => item.obj != null && item.obj.GetComponent<SpriteRenderer>()?.enabled == true) // 조건 필터링
            .Select(item => item.index) // 인덱스만 선택
            .ToArray();
    }
    public void Load()
    {
        string folderPath = Path.Combine(Application.dataPath, savePath.TrimStart("Assets/".ToCharArray()));
        string path = Path.Combine(folderPath, $"Level{stageNumber}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"File not found at path: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        // 로드한 데이터를 StageMaker에 반영
        width = levelData.columns;
        height = levelData.rows;
        timer = levelData.timer;
        star = levelData.stars;
        tileIDs = levelData.tiles;

        // 기존 grid를 클리어하고 다시 생성
        Generate();

        // 로드된 데이터를 기반으로 타일 설정
        for (int i = 0; i < tileIDs.Length; i++)
        {
            if (tileIDs[i] > 0) // 타일 ID가 유효한 경우
            {
                SpriteRenderer sr = grid[i].GetComponent<SpriteRenderer>();
                sr.sprite = Sprites[tileIDs[i]];
                sr.enabled = true;
            }
            else
            {
                grid[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        Debug.Log($"Level {stageNumber} loaded from {path}");
    }
    
    public void ChangeMaxLevel(int newMaxLevel)
    {
        LastLevelData lastLevelData = new LastLevelData { maxLevel = lastLevel };
        string maxLevelPath = GetMaxLevelPath();
        if (maxLevelPath == null)
        {
            Debug.LogError("Can't find MaxLevel.json file");
            return;
        }

        if (lastLevel < newMaxLevel)
        {
            lastLevelData.maxLevel = newMaxLevel;
            string json = JsonUtility.ToJson(lastLevelData, true);

            File.WriteAllText(maxLevelPath, json);
            Debug.Log("ChangeMaxLevel!");
        }
        else
        {
            Debug.Log("Don't ChangeMaxLevel....");
        }
    }
    
    public string GetMaxLevelPath()
    {
        string folderPath = Path.Combine(Application.dataPath, savePath.TrimStart("Assets/".ToCharArray()));
        string path = Path.Combine(folderPath, "MaxLevel.json");

        if (!File.Exists(path))
        {
            Debug.LogError("MaxLevel not found. Defaulting to 1");
            return null;
        }

        string json = File.ReadAllText(path);
        LastLevelData lastLevelData = JsonUtility.FromJson<LastLevelData>(json);
        lastLevel = lastLevelData.maxLevel;
        return path;
    }
}
