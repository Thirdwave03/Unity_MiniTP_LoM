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

    public TextLocalizer continueLC;
    public TextLocalizer newGameLC;
    public TextLocalizer LimitedResourceLC;
    public TextLocalizer ExitGameLC;
    public TextLocalizer quitLC;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 999;
        AddListeners();
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;
    }

    private void OnEnable()
    {
        settingsWindow.SetActive(false);
        messageBox.SetActive(false);
        languagesDD.value = (int)Variables.currentLanguage;
        languagesDD.RefreshShownValue();
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
        // if (SaveLoadManager.GetAvailableSaveSlot() == -1)
        // {
        //     PopUpWindowChooseOverwriteSlot();
        //     return;
        // }
        // GameManager.Instance.currentSavedSlotIndex = SaveLoadManager.GetAvailableSaveSlot();
        //var availableSlot = SaveLoadManager.GetAvailableSaveSlot();
        //if(availableSlot == -1)
        //{
        //    PopUpWindowChooseOverwriteSlot();
        //}
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

    public void OpenMessage(TitleSceneCenterMsgType msgType)
    {
        messageBox.SetActive(true);
        messageType = msgType;
        switch (msgType)
        {
            case TitleSceneCenterMsgType.BestRecord:
                saveloadWindow.gameObject.SetActive(false);
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                centerMessageLC.tmp.text = string.Format(
              DataTableManager.StringTableList[(int)Variables.currentLanguage]
              .Get(999005), SaveLoadManager.BaseData.bestScore.ToString());
                break;
            case TitleSceneCenterMsgType.DevInfo:
                saveloadWindow.gameObject.SetActive(false);
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999990);
                break;
            case TitleSceneCenterMsgType.SelectLoadSlot:
                saveloadWindow.gameObject.SetActive(true);
                centerMessage.SetActive(false);
                if (SaveLoadManager.LoadBase())
                {
                    saveloadWindow.SetSaveLoadSlots();
                }
                break;
            case TitleSceneCenterMsgType.SelectNewGameSlot:
                saveloadWindow.gameObject.SetActive(true);
                centerMessage.SetActive(false);
                if (SaveLoadManager.LoadBase())
                {
                    saveloadWindow.SetSaveLoadSlots();
                }
                break;
            case TitleSceneCenterMsgType.SelectOverwriteSlot:                
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(true);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999924);
                break;
            case TitleSceneCenterMsgType.SelectDeleteSlot:
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(true);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999923);
                break;
            case TitleSceneCenterMsgType.InformDeleted:
                centerMessage.SetActive(true);
                centerMessageCheckBoxArea.SetActive(false);
                centerMessageLC.tmp.text =
                    DataTableManager.StringTableList[(int)Variables.currentLanguage]
                    .Get(999925);
                break;
            default:
                break;
        }
        UpdateTitleSceneDisplay();
        saveloadWindow.SetSaveLoadSlots();
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
        }
    }

    public void OnClickDeleteSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        OpenMessage(TitleSceneCenterMsgType.SelectDeleteSlot);
    }

    public void UpdateTitleSceneDisplay()
    {

    }

    private void OnClickContinue()
    {
        OpenMessage(TitleSceneCenterMsgType.SelectLoadSlot);
    }

    public void OnClickSaveLoadSlot(int slotIndex)
    {
        if (slotIndex < 1 || slotIndex > 3)
        {
            Debug.Log($"Invalid SlotIndex: {slotIndex}");
            return;
        }
        selectedSlotIndex = slotIndex;
        if (messageType == TitleSceneCenterMsgType.SelectLoadSlot)
        {            
            if (!SaveLoadManager.Load(slotIndex))
            {
                Debug.Log($"Load slot {slotIndex} failed");
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
        gameMode = GameModes.ShortGame;
        OpenMessage(TitleSceneCenterMsgType.SelectNewGameSlot);
    }

    private void OnClickTemp()
    {

    }

    private void OnClickSlot(int slot)
    {
        GameManager.Instance.currentSavedSlotIndex = slot;
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    public void OnLanguageChange(int value)
    {
        Debug.Log($"Language Change Input value: {value}\nLanguage: {(Languages)value}");
        Variables.currentLanguage = (Languages)value;
        gameObject.BroadcastMessage("OnChangeLanguage", (Languages)value);
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
}
