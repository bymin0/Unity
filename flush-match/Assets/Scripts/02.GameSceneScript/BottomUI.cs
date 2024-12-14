using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{
    public Button ShuffleBtn, TimerBtn, JokerBtn, AutoBtn;
    public TextMeshProUGUI ShuffleCntTxt;
    public TextMeshProUGUI TimerCntTxt;
    public TextMeshProUGUI JokerCntTxt;
    public TextMeshProUGUI AutoCntTxt;

    private GameUIManager gameUIManager;
    private ItemManager itemManager;
    private MainMusic mainMusic;

    public void initialize(GameUIManager gameUIManager, ItemManager itemManager, MainMusic mainMusic)
    {
        this.gameUIManager = gameUIManager;
        this.itemManager = itemManager;
        this.mainMusic = mainMusic;

        ShuffleBtn.onClick.AddListener(OnShuffleButtonClicked);
        TimerBtn.onClick.AddListener(OnTimerButtonClicked);
        AutoBtn.onClick.AddListener(OnAutoButtonClicked);
        JokerBtn.onClick.AddListener(OnJokerButtonClicked);
    }

    private void OnShuffleButtonClicked()
    {
        mainMusic.ClickEventSFX();
        itemManager.ActiveShuffleEvent();
        ShuffleCntTxt.text = itemManager.GetItemCounts()[ConstCollect.Shuffle].ToString();
    }
    private void OnTimerButtonClicked()
    {
        mainMusic.ClickEventSFX();
        itemManager.ActiveTimerEvenet();
        TimerCntTxt.text = itemManager.GetItemCounts()[ConstCollect.Timer].ToString();
    }
    private void OnJokerButtonClicked()
    {
        mainMusic.ClickEventSFX();
        itemManager.ActiveJockerEvent();
        JokerCntTxt.text = itemManager.GetItemCounts()[ConstCollect.Joker].ToString();
    }
    private void OnAutoButtonClicked()
    {
        mainMusic.ClickEventSFX();
        itemManager.ActiveAutoEvent();
        AutoCntTxt.text = itemManager.GetItemCounts()[ConstCollect.Auto].ToString();
    }
}
