using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    private DataManager dataManager;
    private MusicManager musicManager;

    // slot ui
    public Button[] Slots;
    public TextMeshProUGUI[] UserName, BestScore, BestStage;

    // notice ui
    public GameObject DataObj;
    public GameObject SaveObj;
    public TMP_InputField UserID;
    public Button SaveBtn;
    public GameObject ErrorObj;

    public GameObject LoadDeletObj;
    public TextMeshProUGUI PreviewTxt;
    public Button StartBtn;
    public Button DeleteBtn;

    // user data
    private List<UserData> UserDatas;
    private UserData SelectedUser;
    private int saveIdx;

    public AudioClip BGM;
    public AudioClip ClickSFX;
    public AudioClip ErrSFX;

    public void Start()
    {
        dataManager = GetComponent<DataManager>();
        musicManager = MusicManager.instance;
        UserDatas = new List<UserData>();
        SelectedUser = null;

        DataObj.SetActive(false);
        SaveObj.SetActive(false);
        LoadDeletObj.SetActive(false);
        ErrorObj.SetActive(false);

        for (int i = 0; i < Slots.Length; i++)
        {
            int currIdx = i;
            Slots[i].onClick.AddListener(() => OnSlotButtonClicked(currIdx));
        }
        SaveBtn.onClick.AddListener(() => OnSaveButtonClicked());
        StartBtn.onClick.AddListener(() => OnStartButtonClicked());
        DeleteBtn.onClick.AddListener(() => OnDeleteButtonClicked());

        SettingSlot();
        if (musicManager != null && BGM != null)
            musicManager.PlayBGM(BGM);
    }

    public void SettingSlot()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            UserData userData = dataManager.LoadUserData(i + 1);
            if (userData == null)
            {
                userData = dataManager.SaveNLoadUserData(i + 1, $"½½·Ô {i + 1}", 0, 0);
            }
            GameData.SaveSlotData(i + 1, userData);
            UserName[i].text = userData.playerName;
            BestScore[i].text = ConstCollect.BestScore + userData.bestScore.ToString(ConstCollect.K);
            BestStage[i].text = ConstCollect.BestStage + userData.bestStage.ToString(ConstCollect.K);
            UserDatas.Add(dataManager.LoadUserData(i + 1));
        }
    }

    public void OnSlotButtonClicked(int slot)
    {
        musicManager.PlaySFX(ClickSFX);
        DataObj.SetActive(true);
        saveIdx = slot + 1;

        if (UserDatas[slot].playerName.Contains(ConstCollect.Slot))
        {
            LoadDeletObj.SetActive(false);
            SaveObj.SetActive(true);
        }
        else
        {
            SelectUserData(slot);
            LoadDeletObj.SetActive(true);
            SaveObj.SetActive(false);
        }
    }

    private void SelectUserData(int slot)
    {
        GameData.SetCurrentUserData(slot + 1);
        PreviewTxt.text = GameData.CurrentUserData.playerName + ConstCollect.Slash + 
                          ConstCollect.BestScore + GameData.CurrentUserData.bestScore.ToString(ConstCollect.K) + ConstCollect.Slash + 
                          ConstCollect.BestStage + GameData.CurrentUserData.bestStage.ToString(ConstCollect.K);
    }

    public void OnSaveButtonClicked()
    {
        musicManager.PlaySFX(ClickSFX);
        string userName = ConstCollect.DefaultPlayName;
        if (UserID.text.Length > 15)
        {
            StartCoroutine(AlertError());
            return;
        }

        if (!string.IsNullOrWhiteSpace(UserID.text))
            userName = UserID.text;

        dataManager.SaveUserData(saveIdx, userName, 0, 0);
        UpdateUI(saveIdx);
        StartCoroutine(CloseAlert(SaveObj));
        //OnStartButtonClicked();
    }

    public void OnDeleteButtonClicked()
    {
        musicManager.PlaySFX(ClickSFX);
        dataManager.SaveUserData(saveIdx, $"½½·Ô {saveIdx}", 0, 0);
        UpdateUI(saveIdx);
        StartCoroutine(CloseAlert(SaveObj));
    }

    public void OnStartButtonClicked()
    {
        musicManager.PlaySFX(ClickSFX);
        SceneManager.LoadScene(ConstCollect.Game);
    }

    private void UpdateUI(int slot)
    {
        int idx = slot - 1;
        UserDatas[idx] = dataManager.LoadUserData(slot);
        UserName[idx].text = UserDatas[idx].playerName;
        BestScore[idx].text = ConstCollect.BestScore + UserDatas[idx].bestScore.ToString(ConstCollect.K);
        BestStage[idx].text = ConstCollect.BestStage + UserDatas[idx].bestStage.ToString(ConstCollect.K);
    }

    private IEnumerator CloseAlert(GameObject obj)
    {
        yield return ConstCollect.waitFor5ms;
        DataObj.SetActive(false);
        obj.SetActive(false);
    }

    private IEnumerator AlertError()
    {
        musicManager.PlaySFX(ErrSFX);
        ErrorObj.SetActive(true);
        yield return ConstCollect.waitFor5ms;
        ErrorObj.SetActive(false);
        UserID.text= "";
    }
}
