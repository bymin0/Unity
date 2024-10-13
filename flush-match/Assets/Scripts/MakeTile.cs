using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeTile : MonoBehaviour
{
    public string Type;
    public TextMeshProUGUI textMeshPro;

    public void Initialize(int r, int c)
    {
        Type = RandomType();
        UpdateTileDisplay();
    }

    private string RandomType()
    {
        string[] types = { "H", "S", "D", "C" };
        return types[Random.Range(0, types.Length)];
    }

    private void UpdateTileDisplay()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = Type;
        }
    }
}
