using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int Row { get; private set; }
    public int Col { get; private set; }

    public Vector2 imgSize;

    public Image image; // load ui image
    public TileType Type;
    public TileData tileData;
    public GameManager gameManager;
    public Renderer tileRenderer;

    private void Start()
    {
        gameManager = GameManager.instance; // cashing
    }

    // display tile's color(or img?)
    public void UpdateTileDisplay(TileType type, int r, int c)
    {
        Type = type;
        Row = r;
        Col = c;

        if (tileRenderer == null)
            tileRenderer = GetComponent<Renderer>();

        if (tileData == null)
            tileData = new TileData();

        string colorHex = tileData.GetColor(Type);
        
        if (ColorUtility.TryParseHtmlString(colorHex, out var color))
        {
            Material newMaterial = new Material(tileRenderer.sharedMaterial) { color = color };
            tileRenderer.material = newMaterial;
        }
    }

    // click event
    void OnMouseDown()
    {
        if (gameManager != null)
        gameManager.OnTileClicked(this);
    }
}
