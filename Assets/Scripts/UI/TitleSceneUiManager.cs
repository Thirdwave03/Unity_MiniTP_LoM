using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUiManager : MonoBehaviour
{
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
    }

    private void OnEnable()
    {
        settingsWindow.SetActive(false);
        messageBox.SetActive(false);
        languagesDD.value = (int)Variables.currentLanguage;
        languagesDD.RefreshShownValue();
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;
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
        limitedResourceButton.onClick.AddListener(OnClickTemp);
        exitGameButton.onClick.AddListener(OnClickExitGame);
        bestRecordButton.onClick.AddListener(OnClickTemp);
        devIconButton.onClick.AddListener(OnClickTemp);
        settingClose.onClick.AddListener(OnClickSettingClose);
        settingQuitButton.onClick.AddListener(OnClickExitGame);

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
        
        GameManager.Instance.currentSavedSlotIndex = 1;
        GameManager.Instance.SetupNewGame(GameModes.Default);
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickContinue()
    {
        int tempSlotIndex = 1;

        if (!SaveLoadManager.Load(tempSlotIndex))
        {
            Debug.Log($"Load slot {tempSlotIndex} failed");
            return;
        }
        Debug.Log($"Load slot {tempSlotIndex} successful");
        GameManager.Instance.LoadSavedSlot(tempSlotIndex);
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickTemp()
    {

    }

    private void PopUpWindowChooseOverwriteSlot()
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
