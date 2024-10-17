using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    // print tiles
    public void GenerateTiles(LevelData levelData, GameObject tilePrefab)
    {
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
                GameObject newTile = Instantiate(tilePrefab, posiiton, Quaternion.identity, transform);

                MakeTile tile = newTile.GetComponent<MakeTile>();
                tile.Type = tileType;
                tile.Initialize(tileType, row, col);
            }
        }
    }
}