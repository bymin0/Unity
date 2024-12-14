using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // game ui
    public CanvasGroup ComboBox;
    public RectTransform uiElement;
    public GameObject GameClearImg;
    public TextMeshProUGUI ComboCntTxt;
    // item container
    public GameObject[] ItemObjects;
    // pause ui
    public GameObject PausePannel;

    private GameManager gameManager;

    // combo box effect(faid in -> move up -> faid out)
    Vector2 startPosition;
    private Coroutine comboEffectCoroutinie;
    private Coroutine moveCoroutine;
    private float fadeDuration = 0.5f;
    private float moveDistance = 50f;
    private float moveDuration = 2.0f;

    public float Timer { get; private set; }
    public int CurrLevel { get; private set; }
    
    public void Initialize
        (GameManager gameManager, ItemManager itemManager, MainMusic mainMusic,
        TopUI topUI, BottomUI bottomUI, PauseUI pauseUI, ClearUI clearUI)
    {
        this.gameManager = gameManager;

        Timer = itemManager.GetTime();

        PausePannel.SetActive(false);
        DisableCanvasGroup();
        GameClearImg.SetActive(false);
        CurrLevel = gameManager.GetCurrentLevel();

        topUI.Initialize(this, mainMusic);
        bottomUI.initialize(this, itemManager, mainMusic);
        pauseUI.Initialize(gameManager, this, topUI, mainMusic);
        clearUI.Initialize(gameManager, this, mainMusic);

        for (int i = 0; i < 3; i++)
        {
            topUI.SetStarsXPosition(i, itemManager.GetStarsPos(i));
        }
        
        for (int i = 0; i < gameManager.ActiveToLevel(0); i++)
            ItemObjects[i].SetActive(true);

        pauseUI.SetUserName();
        pauseUI.SetVolumeSliderPos();
        startPosition = uiElement.anchoredPosition;
    }
    public void SetComboTxt(int combo)
    {
        if (comboEffectCoroutinie != null)
            StopCoroutine(comboEffectCoroutinie);
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        ComboCntTxt.text = combo.ToString(ConstCollect.K);
        ComboBox.gameObject.SetActive(true);
        comboEffectCoroutinie = StartCoroutine(ComboEffectSequence());
        moveCoroutine = StartCoroutine(MoveUp());
    }

    public void GameClear()
    {
        GameClearImg.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameManager.SetIsPaused(true);
        PausePannel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameManager.SetIsPaused(false);
        PausePannel.SetActive(false);
    }

    private void EnableCanvasGroup()
    {
        ComboBox.alpha = 1;
        ComboBox.interactable = true;
        ComboBox.blocksRaycasts = true;
    }

    private void DisableCanvasGroup()
    {
        ComboBox.alpha = 0;
        ComboBox.interactable = false;
        ComboBox.blocksRaycasts = false;
    }

    private IEnumerator ComboEffectSequence()
    {
        yield return StartCoroutine(FaidIn());

        yield return StartCoroutine(FaidOut());

        comboEffectCoroutinie = null;
    }

    private IEnumerator MoveUp()
    {
        uiElement.anchoredPosition = startPosition;

        Vector2 endPosition = startPosition + new Vector2(0, moveDistance);
        float spendTime = 0f;

        while (spendTime < moveDuration)
        {
            spendTime += Time.deltaTime;
            float t = spendTime / moveDuration;
            uiElement.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t); // move up
            yield return null;
        }

        uiElement.anchoredPosition = startPosition;

        moveCoroutine = null;
    }

    private IEnumerator FaidIn()
    {
        float spendTime = 0f;

        while(spendTime < fadeDuration)
        {
            spendTime += Time.deltaTime;
            ComboBox.alpha = Mathf.Clamp01(spendTime / fadeDuration);
            yield return null;
        }

        EnableCanvasGroup();
    }
    private IEnumerator FaidOut()
    {
        float spendTime = 0f;

        while (spendTime < fadeDuration)
        {
            spendTime += Time.deltaTime;
            ComboBox.alpha = Mathf.Clamp01(1 - (spendTime / fadeDuration));
            yield return null;
        }

        DisableCanvasGroup();
    }
}
