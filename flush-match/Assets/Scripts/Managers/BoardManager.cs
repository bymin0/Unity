using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoardManager : MonoBehaviour
{// TileMatchManager
    public int Row { get; private set; }
    public int Col { get; private set; }
    public int[,] board;

    public TileType Type;
    public GameManager gameManager;
    public GameObject dummyObject;  

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    // create tiles
    public void Initialize(TileType type, int r, int c)
    {
        Type = type;
        Row = r;
        Col = c;

        if (GameManager.instance != null)
            board[r, c] = (int)type;
    }

    // print tiles
    public void GenerateTiles(LevelData levelData, GameObject tilePrefab)
    {
        board = new int[levelData.rows, levelData.columns];
        // get tile prefabs's size
        Vector3 tileSize = tilePrefab.GetComponent<Renderer>().bounds.size;

        // edit tiles position
        float totalWidth = (levelData.columns * tileSize.x);
        float totalHeight = (levelData.rows * tileSize.y);

        float xOffset = -totalWidth / 2 + tileSize.x / 2;
        float yOffset = totalHeight / 2 - tileSize.y / 2;

        // print tiles
        for (int row = 0; row < levelData.rows; row++)
        {
            for (int col = 0; col < levelData.columns; col++)
            {
                // calc index
                int index = row * levelData.columns + col; // levelData: 1dimensional, tiles: 2d

                // get tile's value
                TileType tileType = (TileType)levelData.tiles[index];

                // if levelData.tiles[index] value is "Empty" will not show
                if (tileType == 0)
                    continue;

                // calculate tile positions
                float xPositin = col * tileSize.x + xOffset;
                float yPosition = -row * tileSize.y + yOffset;

                // create tile & initialize
                Vector3 posiiton = new Vector3(xPositin, yPosition, 0);
                GameObject newTile = Instantiate(tilePrefab, posiiton, Quaternion.identity);

                Tile tile = newTile.GetComponent<Tile>();
                
                Initialize(tileType, row, col);
                tile.UpdateTileDisplay(Type, row, col);
            }
        }
    }

    public void CheckMatch(Tile firstSelectedTile, Tile secondSelectedTile)
    {
        if (firstSelectedTile == null || secondSelectedTile == null)
        {
            Debug.LogError("One of the tiles is null. Cannot proced with matching");
            return;
        }
        if (firstSelectedTile.Type == secondSelectedTile.Type)
        {
            // Vector2 start = new Vector2(firstSelectedTile.Row, firstSelectedTile.Col);
            // Vector2 end = new Vector2(secondSelectedTile.Row, secondSelectedTile.Col);
            Vector2 start = new Vector2( firstSelectedTile.Col, firstSelectedTile.Row);
            Vector2 end = new Vector2(secondSelectedTile.Col, secondSelectedTile.Row );

            if (AreTilesMatching(start, end, board))
            {
                HandleMatch(firstSelectedTile, secondSelectedTile);
                ChangeBoard(firstSelectedTile, secondSelectedTile);
            }
        }
        else
        {
            Debug.Log("Tiles do not match");
        }
    }

    void ChangeBoard(Tile firstSelectedTile, Tile secondSelectedTile)
    {
        board[firstSelectedTile.Row, firstSelectedTile.Col] = 0;
        board[secondSelectedTile.Row, secondSelectedTile.Col] = 0;
    }

    // check two tiles match algorithm
    public bool AreTilesMatching(Vector2 start, Vector2 end, int[,] board)
    {
        // setting BFS direction
        Vector2[] directions = {Vector2.left, Vector2.right, Vector2.up, Vector2.down};

        /* queue's save data type : tuple
        * position : now position(x, y)
        * bends : how many time bands?
        * lastDirection : last use directions(¡Ö previous direction)
        */
        Queue<(Vector2 position, int bends, Vector2 lastDirection)> queue = new Queue<(Vector2, int, Vector2)>();
        // save visited note
        HashSet<Vector2> visited = new HashSet<Vector2>();

        // initialize start point
        queue.Enqueue((start, 0, Vector2.zero));
        visited.Add(start);
        Debug.Log($"starting BFS form: {start} to: {end}"); // debugging
        // BFS logic
        while (queue.Count > 0)
        {
            var (currentPosition, bends, lastDirection) = queue.Dequeue();
            Debug.Log($"Dequeued: {currentPosition} with bends: {bends}"); // debugging
            visited.Add(currentPosition);

            // reached to end position -> can remove tile set
            // if (currentPosition == end && bends < 4)
            // {
            //     Debug.Log($"Reached destination: {end} with {bends} bends."); // debugging 
            //     return true;
            // }

            // // set limit bends time
            // if (bends > 3)
            // {
            //     Debug.Log($"Too many bends at: {currentPosition}. Continuing to next node..."); // debugging
            //     continue; 
            // }

            // search 4 direction
            foreach (var direction in directions)
            {
                Vector2 nextPosition = currentPosition + direction;
                
                // check position is out of bounds of th board
                if (nextPosition.x < 0 || nextPosition.x >= board.GetLength(1) ||
                    nextPosition.y < 0 || nextPosition.y >= board.GetLength(0))
                {
                    Debug.Log($"Out of bounds: {nextPosition}"); // debugging
                    continue;
                }

                int newBends = bends + (direction != lastDirection && lastDirection != Vector2.zero ? 1 : 0);
                
                if(newBends > 3) continue;

                if (nextPosition == end )
                    return true;
                
                // skip if already visited node or reached other tile
                if (visited.Contains(nextPosition) || board[(int)nextPosition.y, (int)nextPosition.x] != 0)
                {
                    Debug.Log($"\t\tBlocked or already visited: {nextPosition}"); // debugging
                    continue;
                }
                
                // turn in a diffrent direction
                
                queue.Enqueue((nextPosition, newBends, direction));
                Debug.Log($"Enqueued: {nextPosition} with bends: {newBends}"); // debugging
                //visited.Add(nextPosition);
            }
        }

        Debug.Log($"No matching path found from {start} to {end}."); // debugging
        // faild search channel
        return false;
    }

    // disable matching tiles
    public void HandleMatch(Tile firstTile, Tile secondTile)
    {
        firstTile.gameObject.SetActive(false);
        secondTile.gameObject.SetActive(false);
    }
    
    
}
