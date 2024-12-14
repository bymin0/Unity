using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject Sucess;
    public GameObject ResultObj;
    public Button GoToMainBtn;

    private TextMeshProUGUI resultTxt;
    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        if (ResultObj != null)
        {
            resultTxt = ResultObj.transform.Find("Result").GetComponent<TextMeshProUGUI>();
            resultTxt.text = $"최종 점수: {gameManager.Score}\n최종 스테이지: {gameManager.GetCurrentLevel()}";
        }

        GoToMainBtn.onClick.AddListener(OnGoToMainButtonClicked);
    }

    public void ClearGame()
    {
        if (Sucess!=null)Sucess.SetActive(true);
        if (resultTxt != null)
        {
            gameManager.SetUserData(true);
            
        }
    }

    public void FailGame()
    {
        if (Sucess != null) Sucess.SetActive(false);
        if (resultTxt != null)
        {
            gameManager.SetUserData(false);
        }
    }

    private void OnGoToMainButtonClicked()
    {
        gameManager.GoToMain();
    }
}
