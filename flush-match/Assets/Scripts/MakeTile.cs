using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MakeTile : MonoBehaviour
{
    public TileType Type;
    public Renderer tileRenderer;

    public void Initialize(TileType type, int r, int c)
    {
        Type = type;
        AssignRenderer();
        UpdateTileDisplay();
    }

    private void UpdateTileDisplay()
    {
        switch(Type)
        {
            case TileType.HEART:
                tileRenderer.material.color = Color.red;
                break;
            case TileType.DIAMOND:
                tileRenderer.material.color = Color.blue;
                break;
            case TileType.SPADE:
                tileRenderer.material.color = Color.gray;
                break;
            case TileType.CLUB:
                tileRenderer.material.color = Color.black;
                break;
            default:
                tileRenderer.material.color = Color.white;
                break;
        }
    }

    private void AssignRenderer()
    {
        if (tileRenderer == null)
        {
            tileRenderer = GetComponent<Renderer>();

        }
    }
}
