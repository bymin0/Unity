using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // top ui
    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI MatchCnt;
    public Slider Timer;
    public Image[] Stars;

    // bottom ui
    public TextMeshProUGUI ShuffleCnt;
    public TextMeshProUGUI TimerCnt;
    public TextMeshProUGUI JokerCnt;
    public TextMeshProUGUI AutoCnt;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }
}
