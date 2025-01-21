using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        AddListeners();
    }

    private void OnEnable()
    {
        settingsWindow.SetActive(false);
    }

    private void AddListeners()
    {
        settingsButton.onClick.AddListener(OnClickSettings);
        continueButton.onClick.AddListener(OnClickTemp);
        newGameButton.onClick.AddListener(OnClickNewGame);
        limitedResourceButton.onClick.AddListener(OnClickTemp);
        exitGameButton.onClick.AddListener(OnClickTemp);
        bestRecordButton.onClick.AddListener(OnClickTemp);
        devIconButton.onClick.AddListener(OnClickTemp);
        settingClose.onClick.AddListener(OnClickSettingClose);
        settingQuitButton.onClick.AddListener(OnClickTemp);


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
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickTemp()
    {

    }
}
