using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject ExitPannel;

    private void Start()
    {
        ExitPannel.SetActive(false);
    }
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("main");
    }

    public void OnEndButtonClick()
    {
        ExitPannel.SetActive(true);
    }

    public void OnYesButtonClick()
    {
        Application.Quit();
    }

    public void OnNoButtonClick()
    {
        ExitPannel.SetActive(false);
    }
}
