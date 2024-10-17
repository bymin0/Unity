using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeTile : MonoBehaviour
{
    public string Type;
    public Renderer tileRenderer;
    public TextMeshProUGUI textMeshPro;

    public void Initialize(string type, int r, int c)
    {
        Type = type;
        AssignRenderer();
        UpdateTileDisplay();
    }

    private void UpdateTileDisplay()
    {
        //AssignRenderer();

        switch(Type)
        {
            case "HEART":
                tileRenderer.material.color = Color.red;
                break;
            case "DIAMOND":
                tileRenderer.material.color = Color.blue;
                break;
            case "CLUB":
                tileRenderer.material.color = Color.gray;
                break;
            case "SPADE":
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
