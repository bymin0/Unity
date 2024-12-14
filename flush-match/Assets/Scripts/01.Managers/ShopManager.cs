using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private GameManager gameManager;
    private ItemManager itemManager;
    private MusicManager musicManager;

    public TextMeshProUGUI Coins;

    // item btn
    public Button Item1_btn;
    public Button Item2_btn;
    // one time item btn
    public Button Incense1_btn;
    public Button Incense2_btn;
    public Button Incense3_btn;
    // game conditionn btn
    public Button NextStage_btn;
    public Button DrawAgain_btn;

    // item img list
    //public List<Sprite> itemSprites;
    public GameObject[] AlertImgs;

    // can use coin
    private int coin;
    private string itemName;

    public AudioClip BGM;
    public AudioClip ClickSFX;
    public AudioClip BuySFX;
    public AudioClip ErrSFX;

    public void Initialize(GameManager gameManager, ItemManager itemManager, MusicManager musicManager)
    {
        this.gameManager = gameManager;
        this.itemManager = itemManager;
        this.musicManager = musicManager;

        Time.timeScale = 1;
        coin = gameManager.GetCoins();
        Coins.text = coin.ToString();
        
        NextStage_btn.onClick.AddListener(() => OnNextStageButtonClicked());
        DrawAgain_btn.onClick.AddListener(() => OnDrawAgainButtonClicked());

        foreach (var alert in AlertImgs)
            alert.SetActive(false);

        if (musicManager != null && BGM != null)
            musicManager.PlayBGM(BGM);
    }

    public void SetRandomItems(List<string> items)
    {
        List<Button> buttons = new List<Button> { Item1_btn, Item2_btn, Incense1_btn, Incense2_btn, Incense3_btn };
        foreach (var button in buttons)
            button.onClick.RemoveAllListeners();

        for (int i = 0; i < buttons.Count; i++)
        {
            string itemName = items[i];
            Button button = buttons[i];

            Transform iconTransform = button.transform.Find(ConstCollect.IconContainer + ConstCollect.Icon);
            Transform priceTransform = button.transform.Find(ConstCollect.IconContainer + ConstCollect.Price);
            Transform descriptionTransform = button.transform.Find(ConstCollect.Description);

            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                iconImage.sprite = itemManager.GetItemSprite(itemName);
            }

            if (descriptionTransform != null)
            {
                TextMeshProUGUI descriptionText = descriptionTransform.GetComponent<TextMeshProUGUI>();
                descriptionText.text = itemManager.GetItemDescription(itemName);
            }
            if (priceTransform != null)
            {
                TextMeshProUGUI priceText = priceTransform.GetComponent<TextMeshProUGUI>();
                priceText.text = itemManager.GetItemPrice(itemName).ToString() + ConstCollect.Coin;
            }

            button.onClick.AddListener(() => OnItemButtonClicked(button, itemName));
        }
    }

    public void OnItemButtonClicked(Button clickedButton, string itemName)
    {
        musicManager.PlaySFX(ClickSFX);
        int itemPrice = itemManager.GetItemPrice(itemName);
        this.itemName = itemName;

        if (coin < itemPrice)
        {
            musicManager.PlaySFX(ErrSFX);
            StartCoroutine(CloseImg(ConstCollect.ErrIdx));
            return;
        }

        musicManager.PlaySFX(BuySFX);
        clickedButton.interactable = false;
        //Debug.Log("아이템 " + itemName + "을 얻었습니다!");
        if (itemManager.GetItemCounts().ContainsKey(itemName))
        {
            itemManager.AddItem(itemName);
        }
        else if (itemName == ConstCollect.Combo1)
        {
            itemManager.SetComboTime(ConstCollect.Add1s);
        }
        else if (itemName == ConstCollect.Combo2)
        {
            itemManager.SetComboTime(ConstCollect.Add2s);
        }
        else if (itemName == ConstCollect.JumpLevel)
        {
            itemManager.SetNextLevel();
        }
        else if (itemName == ConstCollect.FrozenTimer)
        {
            itemManager.SetFrozeTimer();
        }
        else if (itemName == ConstCollect.AutoRemove)
        {
            itemManager.SetAutoRemove();
        }

        SetCoin(itemPrice);

        StartCoroutine(CloseImg(ConstCollect.BuyIdx));
    }

    public void OnNextStageButtonClicked()
    {
        musicManager.PlaySFX(ClickSFX);
        gameManager.IncreaseCurrLevel();
        SceneManager.LoadScene(ConstCollect.Game);
        gameManager.LoadNextLevel();
    }

    public void OnDrawAgainButtonClicked()
    {
        musicManager.PlaySFX(ClickSFX);
        if (coin < ConstCollect.ReDrawPrice)
        {
            musicManager.PlaySFX(ErrSFX);
            StartCoroutine(CloseImg(ConstCollect.ErrIdx));
            return;
        }
        SetRandomItems(itemManager.GetRandomStoreItems());
        SetCoin(ConstCollect.ReDrawPrice);

        Item1_btn.interactable = true;
        Item2_btn.interactable = true;
        Incense1_btn.interactable = true;
        Incense2_btn.interactable = true;
        Incense3_btn.interactable = true;

        //Debug.Log("re draw!!");
    }

    private void SetCoin(int price)
    {
        gameManager.ReduceCoin(price);
        coin = gameManager.GetCoins();
        Coins.text = coin.ToString();
    }
    private IEnumerator CloseImg(int idx)
    {
        if (gameManager == null)
            yield break;
        AlertImgs[idx].SetActive(true);
        if (idx == ConstCollect.BuyIdx)
        {
            Transform buyItemName = AlertImgs[idx].transform.Find(ConstCollect.BuyItem);
            if (buyItemName != null)
            {
                TextMeshProUGUI itemNameText = buyItemName.GetComponent<TextMeshProUGUI>();
                itemNameText.text = itemManager.GetItemName(itemName) + ConstCollect.BuyAlert;
            }
        }

        yield return ConstCollect.waitFor7ms;//new WaitForSeconds(0.7f);

        AlertImgs[idx].SetActive(false);
    }
}
