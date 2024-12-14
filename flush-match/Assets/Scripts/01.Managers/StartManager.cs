using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject ExitPannel;
    public AudioClip BGM;
    public AudioClip ClickSFX;

    private MusicManager musicManager;

    private void Start()
    {
        musicManager = MusicManager.instance;

        ExitPannel.SetActive(false);
        if (musicManager!= null && BGM != null)
        {
            SetStartVolume();
            musicManager.PlayBGM(BGM);
        }
    }
    public void OnStartButtonClick()
    {
        if (musicManager != null && ClickSFX != null)
            musicManager.PlaySFX(ClickSFX);
        SceneManager.LoadScene(ConstCollect.SaveLoad);
    }

    public void OnEndButtonClick()
    {
        musicManager.PlaySFX(ClickSFX);
        ExitPannel.SetActive(true);
    }

    public void OnYesButtonClick()
    {
        musicManager.PlaySFX(ClickSFX);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNoButtonClick()
    {
        musicManager.PlaySFX(ClickSFX);
        ExitPannel.SetActive(false);
    }

    private void SetStartVolume()
    {
        musicManager.SetBgmVolume(musicManager.GetBGMVolume());
        musicManager.SetSFXVolume(musicManager.GetSFXVolume());
    }
}
