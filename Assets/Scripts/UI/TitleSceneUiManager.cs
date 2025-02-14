using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUiManager : MonoBehaviour
{
    private TitleSceneCenterMsgType messageType;
    private GameModes gameMode = GameModes.Default;
    private UpgradeItems upgradeItem;
    private int selectedSlotIndex;

    public Button settingsButton;
    public Button continueButton;
    public Button newGameButton;
    public Button limitedResourceButton;
    public Button exitGameButton;
    public Button bestRecordButton;
    public Button devIconButton;

    public GameObject settingsWindow;
    public Button settingClose;
    public Button settingQuitButton;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject messageBox;
    public UiSaveLoadWindow saveloadWindow;
    public GameObject centerMessage;
    public GameObject centerMessageCheckBoxArea;
    public TextLocalizer centerMessageLC;

    public Button centerMessageCheckB;
    public Button centerMessageCloseB;

    public TMP_Dropdown languagesDD;

    public UiModeSelectWindow modeSelectWindow;
    public Button modeSelectCloseB;

    public TextLocalizer continueLC;
    public TextLocalizer newGameLC;
    public TextLocalizer LimitedResourceLC;
    public TextLocalizer ExitGameLC;
    public TextLocalizer quitLC;

    public UpgradeWindow upgradeWindow;
    public Button upgradeWindowOpenB;
    public Button upgradeCloseB;

    public TextMeshProUGUI diamonds;
    public Image rankImage;
    public TextMeshProUGUI rankText;
    public Slider expSlider;
    public Button rankButton;

    public AudioClip diamondSpentSfx;
    public AudioClip rankUpSfx;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 999;
        AddListeners();
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;
        Variables.currentLanguage = SaveLoadManager.BaseData.lastLanguageSetting;
        languagesDD.value = (int)Variables.currentLanguage;
        languagesDD.RefreshShownValue();
        UpdateTitleSceneDisplay();
    }

    private void OnEnable()
    {
        settingsWindow.SetActive(false);
        messageBox.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        
    }

    private void AddListeners()
    {
        settingsButton.onClick.AddListener(OnClickSettings);
        continueButton.onClick.AddListener(OnClickContinue);
        newGameButton.onClick.AddListener(OnClickNewGame);
        limitedResourceButton.onClick.AddListener(OnClickLimitedResource);
        exitGameButton.onClick.AddListener(OnClickExitGame);
        bestRecordButton.onClick.AddListener(OnClickBestRecord);
        devIconButton.onClick.AddListener(OnClickDevIcon);
        settingClose.onClick.AddListener(OnClickSettingClose);
        settingQuitButton.onClick.AddListener(OnClickExitGame);
        centerMessageCheckB.onClick.AddListener(OnClickCenterMsgCheck);
        centerMessageCloseB.onClick.AddListener(OnClickfCenterMessageClose);
        modeSelectCloseB.onClick.AddListener(OnClickfCenterMessageClose);
        upgradeWindowOpenB.onClick.AddListener(OnClickUpgradeWindow);
        upgradeCloseB.onClick.AddListener(OnClickfCenterMessageClose);
        rankButton.onClick.AddListener(OnClickRankButton);

        // dropdown
        languagesDD.onValueChanged.AddListener(OnLanguageChange);

        // sliders
        bgmSlider.onValueChanged.AddListener(OnValueChangeBGM);
        sfxSlider.onValueChanged.AddListener(OnValueChangeSFX);
    }

    private void OnClickSettings()
    {
        settingsWindow.SetActive(true);
    }

    private void OnClickSettingClose()
    {
        settingsWindow.SetActive(false);
    }

    private void OnClickNewGame()
    {
        //else
        //{
        //    GameManager.Instance.currentSavedSlotIndex = availableSlot;
        //    GameManager.Instance.SetupNewGame(GameModes.Default);
        //    SceneManager.LoadScene((int)SceneIds.MainScene);
        //}

        gameMode = GameModes.Default;
        OpenMessage(TitleSceneCenterMsgType.SelectNewGameSlot);
        
        //GameManager.Instance.currentSavedSlotIndex = 1;
        //GameManager.Instance.SetupNewGame(GameModes.Default);
        //SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickContinue()
    {
        OpenMessage(TitleSceneCenterMsgType.SelectLoadSlot);
    }

    public void OpenMessage(TitleSceneCenterMsgType msgType)
    {
        messageBox.SetActive(true);
        messageType = msgType;
        switch (msgType)
        {
            case TitleSceneCenterMsgType.BestRecord:
                saveloadWindow.gameObject.SetActive(false);
                modeSelectWindow.gameObject.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                centerMessageLC.tmp.text = string.Format(
              DataTableManager.StringTableList[(int)Variables.currentLanguage]
              .Get(999005), SaveLoadManager.BaseData.bestScore[0].ToString());
                break;
            case TitleSceneCenterMsgType.DevInfo:
                saveloadWindow.gameObject.SetActive(false);
                modeSelectWindow.gameObject.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999990);
                break;
            case TitleSceneCenterMsgType.SelectLoadSlot:
                saveloadWindow.gameObject.SetActive(true);
                modeSelectWindow.gameObject.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                centerMessage.SetActive(false);
                if (SaveLoadManager.LoadBase())
                {
                    saveloadWindow.SetSaveLoadSlots();
                }
                saveloadWindow.SetSaveLoadSlots();
                break;
            case TitleSceneCenterMsgType.SelectNewGameSlot:
                saveloadWindow.gameObject.SetActive(true);
                modeSelectWindow.gameObject.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                centerMessage.SetActive(false);
                if (SaveLoadManager.LoadBase())
                {
                    saveloadWindow.SetSaveLoadSlots();
                }
                saveloadWindow.SetSaveLoadSlots();
                break;
            case TitleSceneCenterMsgType.SelectOverwriteSlot:                
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(true);
                modeSelectWindow.gameObject.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999924);
                saveloadWindow.SetSaveLoadSlots();
                break;
            case TitleSceneCenterMsgType.SelectDeleteSlot:
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(true);
                upgradeWindow.gameObject.SetActive(false);
                modeSelectWindow.gameObject.SetActive(false);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999923);
                saveloadWindow.SetSaveLoadSlots();
                break;
            case TitleSceneCenterMsgType.InformDeleted:
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                modeSelectWindow.gameObject.SetActive(false);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999925);
                break;
            case TitleSceneCenterMsgType.SelectGameMode:
                saveloadWindow.gameObject.SetActive(false);
                centerMessage.SetActive(false);
                upgradeWindow.gameObject.SetActive(false);
                modeSelectWindow.gameObject.SetActive(true);
                modeSelectWindow.SetContents();
                break;
            case TitleSceneCenterMsgType.OpenUpgradeWindow:
                saveloadWindow.gameObject.SetActive(false);
                centerMessage.SetActive(false);
                modeSelectWindow.gameObject.SetActive(false);
                upgradeWindow.gameObject.SetActive(true);
                upgradeWindow.SetContents();                
                break;
            case TitleSceneCenterMsgType.IfReallyUpgrade:
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(true);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
                    .Get(999928);
                break;
            case TitleSceneCenterMsgType.CheckMerchantRank:
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                AllignMessageWithCurrentRank();
                break;
            default:
                break;
        }
        UpdateTitleSceneDisplay();
    }

    private void OnClickCenterMsgCheck()
    {
        switch (messageType)
        {           
            case TitleSceneCenterMsgType.SelectOverwriteSlot:
                GameManager.Instance.SetupNewGame(gameMode, selectedSlotIndex);
                SceneManager.LoadScene((int)SceneIds.MainScene);
                break;
            case TitleSceneCenterMsgType.SelectDeleteSlot:
                SaveLoadManager.DeleteSlot(selectedSlotIndex);
                OpenMessage(TitleSceneCenterMsgType.InformDeleted);
                break;
            case TitleSceneCenterMsgType.IfReallyUpgrade:
                centerMessage.SetActive(false);
                UpgradeStat();
                break;
        }
    }    

    public void OnClickDeleteSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        OpenMessage(TitleSceneCenterMsgType.SelectDeleteSlot);
    }

    public void UpdateTitleSceneDisplay()
    {
        diamonds.text = SaveLoadManager.BaseData.diamonds.ToString();
        UpdateRankData();   
    }

    private void UpdateRankData()
    {
        string filename = "";
        int stringId = 0;
        switch (SaveLoadManager.BaseData.MerchantRank)
        {
            case MerchantRanks.NoviceMerchant:
                filename = "shield_007";
                stringId = 999055;
                break;
            case MerchantRanks.PromisingMerchant:
                filename = "shield_010";
                stringId = 999056;
                break;
            case MerchantRanks.SeasonedMerchant:
                filename = "shield_019";
                stringId = 999057;
                break;
            case MerchantRanks.TradeMaestro:
                filename = "shield_026";
                stringId = 999058;
                break;
            case MerchantRanks.MerchantGod:
                filename = "shield_021";
                stringId = 999059;
                break;
        }
        rankImage.sprite = Resources.Load<Sprite>($"Sprites/Icon/ShieldIcons/Chosen/{filename}");
        rankText.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(stringId);
        expSlider.maxValue = GameManager.Instance.DiamondRequiredForEachRank;
        expSlider.value = GameManager.Instance.DiamondAccquiredForEachRank;
    }

    public void OnClickSaveLoadSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        if (slotIndex < 1 || slotIndex > 3)
        {
            Debug.Log($"Invalid SlotIndex: {slotIndex}");
            return;
        }
        if (messageType == TitleSceneCenterMsgType.SelectLoadSlot)
        {
            if (SaveLoadManager.BaseData.dateTimes[slotIndex-1] == default)
            {
                Debug.Log($"Load slot {slotIndex} failed (Base Date Default)");
                return;
            }
            if (!SaveLoadManager.Load(slotIndex))
            {
                Debug.Log($"Load slot {slotIndex} failed (No Save File)");
                return;
            }
            Debug.Log($"Load slot {slotIndex} successful");
            GameManager.Instance.LoadSavedSlot(slotIndex);
            SceneManager.LoadScene((int)SceneIds.MainScene);
        }
        else if (messageType == TitleSceneCenterMsgType.SelectNewGameSlot)
        {
            if (SaveLoadManager.BaseData.dateTimes[selectedSlotIndex - 1] == default)
            {
                GameManager.Instance.SetupNewGame(gameMode, slotIndex);
                SceneManager.LoadScene((int)SceneIds.MainScene);
            }
            else
            {
                OpenMessage(TitleSceneCenterMsgType.SelectOverwriteSlot);
            }
        }
    }

    public void OnClickModeSelect(int index)
    {
        gameMode = (GameModes)(index + 1);
        OpenMessage(TitleSceneCenterMsgType.SelectNewGameSlot);
    }

    public void OnClickUpgradeButton(int index)
    {
        upgradeItem = (UpgradeItems)index;
        OpenMessage(TitleSceneCenterMsgType.IfReallyUpgrade);
    }

    public void OnClickSaveLoadWindowClose()
    {
        messageBox.SetActive(false);
    }

    private void OnClickfCenterMessageClose()
    {
        messageBox.SetActive(false);
    }

    private void OnClickBestRecord()
    {
        OpenMessage(TitleSceneCenterMsgType.BestRecord);
    }

    private void OnClickDevIcon()
    {
        OpenMessage(TitleSceneCenterMsgType.DevInfo);
    }

    private void OnClickLimitedResource()
    {
        OpenMessage(TitleSceneCenterMsgType.SelectGameMode);
    }

    private void OnClickUpgradeWindow()
    {
        OpenMessage(TitleSceneCenterMsgType.OpenUpgradeWindow);
    }

    private void OnClickTemp()
    {

    }

    private void OnClickSlot(int slot)
    {
        //GameManager.Instance.currentSavedSlotIndex = slot;
        //SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    public void OnLanguageChange(int value)
    {
        Debug.Log($"Language Change Input value: {value}\nLanguage: {(Languages)value}");
        Variables.currentLanguage = (Languages)value;
        gameObject.BroadcastMessage("OnChangeLanguage", (Languages)value);
        SaveLoadManager.BaseData.lastLanguageSetting = Variables.currentLanguage;
        SaveLoadManager.SaveBase();
        UpdateRankData();
    }

    public void OnClickExitGame()
    {
        SaveLoadManager.BaseData.bgmVolume = SoundManager.Instance.BgmVolume;
        SaveLoadManager.BaseData.sfxVolume = SoundManager.Instance.SfxVolume;
        SaveLoadManager.SaveBase();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnValueChangeBGM(float val)
    {
        SoundManager.Instance.BgmVolume = val;
    }

    public void OnValueChangeSFX(float val)
    {
        SoundManager.Instance.SfxVolume = val;
    }

    private void UpgradeStat()
    {
        SaveLoadManager.BaseData.diamonds -=
            DataTableManager.UpgradeTable.Get(upgradeItem).UpgradeCost;
        SaveLoadManager.BaseData.diamondsSpent +=
            DataTableManager.UpgradeTable.Get(upgradeItem).UpgradeCost;
        SaveLoadManager.BaseData.upgradeCounts[(int)upgradeItem] = 
            Mathf.Min(SaveLoadManager.BaseData.upgradeCounts[(int)upgradeItem] + 1, 
            DataTableManager.UpgradeTable.Get(upgradeItem).MaxLv);
        upgradeWindow.SetContents();
        if (GameManager.Instance.DiamondAccquiredForEachRank >=
            GameManager.Instance.DiamondRequiredForEachRank)
        {
            MerchantRankUp();
        }
        SoundManager.Instance.PlaySfx(diamondSpentSfx);
        SaveLoadManager.SaveBase();
        UpdateTitleSceneDisplay();
    }

    private void MerchantRankUp()
    {
        SaveLoadManager.BaseData.MerchantRank = 
            (MerchantRanks)Mathf.Clamp(((int)SaveLoadManager.BaseData.MerchantRank + 1),
            0, (int)(MerchantRanks.Count));

        int stringId = 0;
        switch (SaveLoadManager.BaseData.MerchantRank)
        {
            case MerchantRanks.NoviceMerchant:
                break;
            case MerchantRanks.PromisingMerchant:
                stringId = 999956;
                break;
            case MerchantRanks.SeasonedMerchant:
                stringId = 999957;
                break;
            case MerchantRanks.TradeMaestro:
                stringId = 999958;
                break;
            case MerchantRanks.MerchantGod:
                stringId = 999959;
                break;
        }
        Debug.Log($"Rank up to: {SaveLoadManager.BaseData.MerchantRank}");
        SoundManager.Instance.PlaySfx(rankUpSfx);
        centerMessageCheckBoxArea.SetActive(false);
        centerMessage.SetActive(true);
        centerMessageLC.tmp.text =
            DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
            .Get(stringId);
    }

    private void OnClickRankButton()
    {
        OpenMessage(TitleSceneCenterMsgType.CheckMerchantRank);
    }

    private void AllignMessageWithCurrentRank()
    {
        switch (SaveLoadManager.BaseData.MerchantRank)
        {
            case MerchantRanks.NoviceMerchant:
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999962);
                break;
            case MerchantRanks.PromisingMerchant:
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999963);
                break;
            case MerchantRanks.SeasonedMerchant:
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999964);
                break;
            case MerchantRanks.TradeMaestro:
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999965);
                break;
            case MerchantRanks.MerchantGod:
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999966);
                break;         
        }

    }
}
