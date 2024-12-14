using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour
{
    public TextMeshProUGUI TotalScoreTxt;
    public TextMeshProUGUI GainCoinTxt;
    public Button GoToShopBtn;

    private GameManager gameManager;
    private GameUIManager gameUIManager;
    private MainMusic mainMusic;

    public void Initialize(GameManager gameManager, GameUIManager gameUIManager, MainMusic mainMusic)
    {
        this.gameManager = gameManager;
        this.gameUIManager = gameUIManager;
        this.mainMusic = mainMusic;

        GoToShopBtn.onClick.AddListener(OnGoToShopButtonCliced);
    }

    public void StageClear()
    {
        mainMusic.StageClearSFX();
        gameManager.SetUserData(true);
        TotalScoreTxt.text = gameManager.GetCurrentScore().ToString(ConstCollect.K);
        GainCoinTxt.text = gameManager.GetCurrentCoins().ToString(ConstCollect.K);
        gameUIManager.GameClear();
    }

    private void OnGoToShopButtonCliced()
    {
        mainMusic.ClickEventSFX();
        gameManager.GoToShop();
    }
}
