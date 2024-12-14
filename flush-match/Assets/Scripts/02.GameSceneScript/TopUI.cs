using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : MonoBehaviour
{
    public Button PauseBtn;
    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI ScoreCntTxt;
    public Slider TimerSlider;
    public Image[] StarsImg;
    
    private GameUIManager gameUIManager;
    private MainMusic mainMusic;

    public void Initialize(GameUIManager gameUIManager, MainMusic mainMusic)
    {
        this.gameUIManager = gameUIManager;
        this.mainMusic = mainMusic;

        PauseBtn.onClick.AddListener(OnPauseButtonClicked);

        LevelTxt.text = gameUIManager.CurrLevel.ToString();
    }

    public void SetStarsXPosition(int idx, float newX)
    {
        RectTransform rectTransform = StarsImg[idx].GetComponent<RectTransform>();
        
        float halfwith = rectTransform.rect.width / 2;

        Vector2 position = rectTransform.anchoredPosition;
        position.x = (1000 * newX) / gameUIManager.Timer - halfwith;
        rectTransform.anchoredPosition = position;
    }

    public void SetScore(int score)
    {
        ScoreCntTxt.text = score.ToString(ConstCollect.K);
    }

    private void OnPauseButtonClicked()
    {
        mainMusic.ClickEventSFX();
        gameUIManager.PauseGame();
    }

}
