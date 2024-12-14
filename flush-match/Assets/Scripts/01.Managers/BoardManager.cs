using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{// TileMatchManager
    public int Row { get; private set; }
    public int Col { get; private set; }
    public int[,] board;

    private TileType Type;
    private GameManager gameManager;
    private MainMusic mainMusic;
    private TileSpriteLoader tileSpriteLoader;

    private List<Tile> tiles;
    private List<int> jokerIndex = new List<int>();
    private HashSet<TileType> uniqueTileTypes;

    private int activeTileCnt;

    public void Initialize(GameManager gameManager, MainMusic mainMusic, TileSpriteLoader tileSpriteLoader)
    {
        // gameManager = GameManager.instance;
        this.gameManager = gameManager;
        this.mainMusic= mainMusic;
        this.tileSpriteLoader= tileSpriteLoader;
        uniqueTileTypes = new HashSet<TileType>();
        activeTileCnt = 0;
        tiles = new List<Tile>();
    }

    // create tiles
    public void Initialize(TileType type, int r, int c)
    {
        Type = type;
        Row = r;
        Col = c;

        if (GameManager.instance != null)
            board[r, c] = (int)type;
        
        if (type != TileType.EMPTY)
            uniqueTileTypes.Add(type);
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
                {
                    tiles.Add(null);
                    continue;
                }

                // calculate tile positions
                float xPositin = col * tileSize.x + xOffset;
                float yPosition = -row * tileSize.y + yOffset;

                // create tile & initialize
                Vector3 posiiton = new Vector3(xPositin, yPosition, 0);
                GameObject newTile = Instantiate(tilePrefab, posiiton, Quaternion.identity);

                Tile tile = newTile.GetComponent<Tile>();
                
                tile.Initialize(gameManager, this, tileSpriteLoader);
                tiles.Add(tile);
                
                Initialize(tileType, row, col);
                tile.UpdateTileDisplay(Type, row, col);
                activeTileCnt++;
            }
        }
        Row = board.GetLength(0);
        Col = board.GetLength(1);
        //Debug.Log("activeTile cnt: "+ activeTileCnt);
    }

    public void DestoryTile(Tile tile)
    {
        if (!tiles.Contains(tile))
            return;

        tiles[tiles.IndexOf(tile)] = null;
    }

    public bool CheckMatch(Tile firstSelectedTile, Tile secondSelectedTile)
    {
        if (firstSelectedTile == null || secondSelectedTile == null)
        {
            //Debug.LogError("One of the tiles is null. Cannot proced with matching");
            return false;
        }

        if (firstSelectedTile.Type == secondSelectedTile.Type)
        {
            Vector2 start = new Vector2(firstSelectedTile.Col, firstSelectedTile.Row);
            Vector2 end = new Vector2(secondSelectedTile.Col, secondSelectedTile.Row);

            if (AreTilesMatching(start, end))
            {
                HandleMatch(firstSelectedTile, secondSelectedTile);
                ChangeBoard(firstSelectedTile, secondSelectedTile);
                CheckActiveTiles(2);

                gameManager.HandleCombo();

                return true;
            }
            else
                gameManager.EndCombo();
        }

        return false;
    }

    void ChangeBoard(Tile firstSelectedTile, Tile secondSelectedTile)
    {
        board[firstSelectedTile.Row, firstSelectedTile.Col] = 0;
        board[secondSelectedTile.Row, secondSelectedTile.Col] = 0;
    }

    // check two tiles match algorithm
    public bool AreTilesMatching(Vector2 start, Vector2 end)
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

        // BFS logic
        while (queue.Count > 0)
        {
            var (currentPosition, bends, lastDirection) = queue.Dequeue();
            visited.Add(currentPosition);

            // search 4 direction
            foreach (var direction in directions)
            {
                Vector2 nextPosition = currentPosition + direction;
                
                // check position is out of bounds of th board
                if (nextPosition.x < 0 || nextPosition.x >= Col ||
                    nextPosition.y < 0 || nextPosition.y >= Row)
                {
                    continue;
                }

                int newBends = bends + (direction != lastDirection && lastDirection != Vector2.zero ? 1 : 0);
                
                if(newBends > 2) continue;

                if (nextPosition == end )
                    return true;
                
                // skip if already visited node or reached other tile
                if (visited.Contains(nextPosition) || board[(int)nextPosition.y, (int)nextPosition.x] != 0)
                {
                    continue;
                }
                
                // turn in a diffrent direction
                
                queue.Enqueue((nextPosition, newBends, direction));
            }
        }

        // faild search channel
        return false;
    }

    // disable matching tiles
    public void HandleMatch(Tile firstTile, Tile secondTile)
    {
        firstTile.Disable();
        secondTile.Disable();

        RemoveTileTypeIfEmpty(firstTile.Type);
    }
    
    public HashSet<TileType> GetUniqueTileTipes()
    {
        return uniqueTileTypes;
    }

    public void ChangeJoker(TileType tile)
    {
        jokerIndex.Clear();
        
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                int idx = i * Col + j;
                bool isTileJoker = board[i, j] == (int)tile;
                bool isTileValid = tiles[idx] != null && tiles[idx].Type == tile && tiles[idx].gameObject.activeSelf;

                if (isTileJoker && isTileValid)
                {
                    board[i,j] = (int)TileType.JOKER;
                    tiles[idx].UpdateTileDisplay(TileType.JOKER);

                    jokerIndex.Add(idx);
                }
            }
        }
        StartCoroutine(RemoveJokerCard());
    }

    public bool Shuffle()
    {
        int cnt = 0;
        List<Tile> shuffleBoard;
        do
        {
            cnt++;
            shuffleBoard = ShuffleBoard();
            if (cnt > 3)
            {
                return false;
            }
        } while (!HasTwoMatchTilePairs(shuffleBoard));

        Debug.Log("board all cnt: "+board.GetLength(0)*board.GetLength(1)+", tiles cnt: "+tiles.Count+", shuffleBoard cnt: "+shuffleBoard.Count);

        int idx = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 0 || shuffleBoard[idx] == null)
                {
                    idx++;
                    continue; 
                }
                board[i, j] = (int)shuffleBoard[idx].Type;
                tiles[idx].UpdateTileDisplay(shuffleBoard[idx].Type);
                idx++;
            }
        }
        return true;
    }
    public void UseAutoMatch()
    {
        List<Tile> validTiles = tiles.Where(tile => tile != null && tile.Type != TileType.EMPTY).ToList();
        List<Tile> attemptedTiles = new List<Tile>();

        while (attemptedTiles.Count < validTiles.Count)
        {
            List<Tile> remainingTiles = validTiles.Except(attemptedTiles).ToList();
            Tile randomTile = validTiles[UnityEngine.Random.Range(0, remainingTiles.Count)];

            attemptedTiles.Add(randomTile);

            foreach (var matchTile in validTiles)
            {
                if (matchTile == randomTile) continue;

                if (CheckMatch(randomTile, matchTile))
                    return;
            }
        }
    }

    public int GetActiveTilesCnt()
        { return activeTileCnt; }

    private List<Tile> ShuffleBoard()
    {
        System.Random rand = new System.Random();
        List<Tile> shuffleBoard = new List<Tile>(tiles);
        
        List<TileType> nonEmptyTiles = shuffleBoard
                                                .Where(tile => tile != null && tile.Type != TileType.EMPTY)
                                                .Select(tile => tile.Type)
                                                .ToList();

        //Debug.Log("Before shuffle:");
        foreach (TileType type in Enum.GetValues(typeof(TileType)))
        {
            int count = shuffleBoard.Count(tile => tile != null && tile.Type == type);
            //Debug.Log(type + " count: " + count);
        }

        for (int i = nonEmptyTiles.Count - 1; i > 0 ; i--)
        {
            int j = rand.Next(i + 1);
            TileType temp = nonEmptyTiles[i];
            nonEmptyTiles[i] = nonEmptyTiles[j];
            nonEmptyTiles[j] = temp;
        }

        int tileIndex = 0;
        for (int i = 0; i < shuffleBoard.Count; i++)
        {
            if (shuffleBoard[i] != null && shuffleBoard[i].Type != TileType.EMPTY )
            {
                shuffleBoard[i].Type = nonEmptyTiles[tileIndex];
                tileIndex++;
            }
        }

        return shuffleBoard;
    }
    
    private void RemoveTileTypeIfEmpty(TileType type)
    {
        bool isTypeRemaining = tiles.Any(tile => tile != null && tile.Type == type);

        if (!isTypeRemaining)
           uniqueTileTypes.Remove(type);
    }

    private bool HasTwoMatchTilePairs()
    {
        return HasTwoMatchTilePairs(tiles);
    }

    private bool HasTwoMatchTilePairs(List<Tile> CheckBoard)
    {
        int matchPairsFound = 0;
        HashSet<int> matchedIndexs = new HashSet<int>();

        for (int i = 0; i < CheckBoard.Count; i++)
        {
            if (CheckBoard[i] == null || CheckBoard[i].Type == TileType.EMPTY || matchedIndexs.Contains(i))
                continue;
            var tile1 = CheckBoard[i].Type;

            for (int j = i + 1;  j < CheckBoard.Count; j++)
            {
                if (CheckBoard[j] == null || CheckBoard[j].Type == TileType.EMPTY || matchedIndexs.Contains(i))
                    continue;
                var tile2 = CheckBoard[j].Type;
                
                Vector2 start = new Vector2(CheckBoard[i].Col, CheckBoard[i].Row);
                Vector2 end = new Vector2(CheckBoard[j].Col, CheckBoard[j].Row);
                
                if (tile1 == tile2 && AreTilesMatching(start, end))
                {
                    matchPairsFound++;

                    if (matchPairsFound == 2)
                        return true;
                }
            }
        }
        return false;
    }

    private IEnumerator RemoveJokerCard()
    {
        yield return ConstCollect.waitFor5ms;

        foreach (int idx in jokerIndex)
        {
            int i = idx / Col;
            int j = idx % Col;

            board[i, j] = (int)TileType.EMPTY;
            tiles[idx].Type = TileType.EMPTY;
            tiles[idx].gameObject.SetActive(false);
        }
        CheckActiveTiles(jokerIndex.Count);
        jokerIndex.Clear();
    }

    public void CheckActiveTiles(int cnt)
    {
        activeTileCnt -= cnt;
        if (activeTileCnt == 0)
        {
            if (gameManager.GetCurrentLevel() < gameManager.GetMaxLevel())
            {
                StageClear();
            }
            else
                gameManager.GoToResult(true);
        }
    }

    public void StageClear()
    {
        gameManager.AddCoin(gameManager.StageClearSetting(true));
    }
}
