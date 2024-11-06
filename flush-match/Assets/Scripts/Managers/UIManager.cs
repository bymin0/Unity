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
    public Slider TimerSlider;
    public Image[] StarsImg;

    // bottom ui
    public Button ShuffleBtn, TimerBtn, JokerBtn, AutoBtn;
    public TextMeshProUGUI ShuffleCntTxt;
    public TextMeshProUGUI TimerCntTxt;
    public TextMeshProUGUI JokerCntTxt;
    public TextMeshProUGUI AutoCntTxt;

    private GameManager gameManager;
    private ItemManager itemManager;

    public void Initialize(GameManager gameManager, ItemManager itemManager)
    {
        this.gameManager = gameManager;
        this.itemManager = itemManager;

        ShuffleBtn.onClick.AddListener(() => OnShuffleButtonClicked());
        TimerBtn.onClick.AddListener(() => OnTimerButtonClicked());
        JokerBtn.onClick.AddListener(() => OnJokerButtonClicked());
        AutoBtn.onClick.AddListener(() => OnAutoButtonClicked());

        for (int i = 0; i < StarsImg.Length; i++)
            SetStarsXPosition(i, itemManager.GetStarsPos(i));
    }

    public void SetStarsXPosition(int idx, float newX)
    {
        RectTransform rectTransform = StarsImg[idx].GetComponent<RectTransform>();

        float halfwith = rectTransform.rect.width / 2;

        Vector2 position = rectTransform.anchoredPosition;
        position.x = (500 * newX) / itemManager.GetTime() - halfwith;
        rectTransform.anchoredPosition = position;
    }

    public void OnShuffleButtonClicked()
    {
        // not yet
        itemManager.ActiveShuffleEvent();
       ShuffleCntTxt.text = itemManager.GetItemCounts()[itemManager.ShuffleCnt].ToString();
    }
    public void OnTimerButtonClicked()
    {
        itemManager.ActiveTimerEvenet();
        TimerCntTxt.text = itemManager.GetItemCounts()[itemManager.TimerCnt].ToString();
    }
    public void OnJokerButtonClicked()
    {
        // not yet
        itemManager.ActiveJockerEvent();
        JokerCntTxt.text = itemManager.GetItemCounts()[itemManager.JokerCnt].ToString();
    }
    public void OnAutoButtonClicked()
    {
        // not yet
        itemManager.ActiveAutoEvent();
        AutoCntTxt.text = itemManager.GetItemCounts()[itemManager.AutoCnt].ToString();
    }
}
