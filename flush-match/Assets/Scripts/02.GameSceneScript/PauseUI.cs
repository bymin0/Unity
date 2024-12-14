using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public TextMeshProUGUI UserName;
    public Slider BGMVolume;
    public Slider SFXVolume;
    public Button CancleBtn, CheckBtn, ExitBtn, AssetBtn, CloseAssetBtn;
    public GameObject AssetsList;

    private GameManager gameManager;
    private GameUIManager gameUIManager;
    private TopUI topUI;
    private MainMusic mainMusic;

    public void Initialize(GameManager gameManager, GameUIManager gameUIManager, TopUI topUI, MainMusic mainMusic)
    {
        this.gameManager = gameManager;
        this.gameUIManager = gameUIManager;
        this.topUI = topUI;
        this.mainMusic = mainMusic;

        AssetsList.SetActive(false);

        CancleBtn.onClick.AddListener(OnCancleButtonClicked);
        CheckBtn.onClick.AddListener(() => SetVolume(BGMVolume.value, SFXVolume.value));
        ExitBtn.onClick.AddListener(OnExitButtonClicked);
        AssetBtn.onClick.AddListener(OnAssetButtonClicked);
        CloseAssetBtn.onClick.AddListener(OnCloseAssetButtonClicked);
    }

    public void SetVolumeSliderPos()
    {
        BGMVolume.value = mainMusic.GetBGMVolume();
        SFXVolume.value = mainMusic.GetSFXVolume();
    }

    public void SetUserName()
    {
        UserName.text = GameData.CurrentUserData.playerName;
    }

    private void OnCancleButtonClicked()
    {
        mainMusic.ClickEventSFX();
        gameUIManager.ResumeGame();
    }

    private void OnExitButtonClicked()
    {
        gameManager.SetUserData(false);
        mainMusic.ClickEventSFX();
        gameManager.GoToMain();
    }

    private void OnAssetButtonClicked()
    {
        mainMusic.ClickEventSFX();
        AssetsList.SetActive(true);
    }
    private void OnCloseAssetButtonClicked()
    {
        mainMusic.ClickEventSFX();
        AssetsList.SetActive(false);
    }
    private void SetVolume(float BGM, float SFX)
    {
        mainMusic.ClickEventSFX();
        mainMusic.UpdateBgmVolume(BGM);
        mainMusic.UpdateSfxVolume(SFX);
        gameUIManager.ResumeGame();
    }
}
