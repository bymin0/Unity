using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameManager gameManager;

    private float time;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }
}
