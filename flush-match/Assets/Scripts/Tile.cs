using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Tile : MonoBehaviour
{
    public int Row { get; private set; }
    public int Col { get; private set; }

    public TileType Type;
    public TilecolorData colorData;
    public GameManager gameManager;
    public Renderer tileRenderer;

    private void Start()
    {
        gameManager = GameManager.instance; // cashing
    }

    // will be remove method
    public void Initialize(TileType type, int r, int c)
    {
        Type = type;
        Row = r;
        Col = c;

        //UpdateTileDisplay();
    }

    // display tile's color(or img?)
    public void UpdateTileDisplay(TileType type)
    {
        Type = type;

        if (tileRenderer == null)
            tileRenderer = GetComponent<Renderer>();

        if (colorData == null)
            colorData = new TilecolorData();

        string colorHex = colorData.GetColor(Type);
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
