using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PurchaseSceneUiManager : MonoBehaviour
{
    public Button inventoryButton;
    public Button settingButton;

    public Button mainSceneButton;
    public Button salesSceneButton;
    public Button innSceneButton;

    public GameObject settingWindow;
    public Button settingCloseButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    public TextMeshProUGUI currentCoin;
    public TextMeshProUGUI inventoryStatus;



    private void Start()
    {
        AddListeners();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        AddListeners();
        UpdatePurchaseSceneDisplay();
    }

    private void UpdatePurchaseSceneDisplay()
    {

    }

    private void AddListeners()
    {
        //inventoryButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        innSceneButton.onClick.RemoveAllListeners();
        settingCloseButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();



        //inventoryButton.onClick.AddListener(OnClickTemp);
        settingButton.onClick.AddListener(OnClickSetting);
        mainSceneButton.onClick.AddListener(OnClickMainScene);
        salesSceneButton.onClick.AddListener(OnClickSalesScene);
        innSceneButton.onClick.AddListener(OnClickInnScene);
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickRestartButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    private void OnClickTemp()
    {

    }

    private void OnClickSetting()
    {
        settingWindow.SetActive(true);
    }
    private void OnClickSettingClose()
    {
        settingWindow.SetActive(false);
    }
    private void OnClickMainScene()
    {
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickPurchaseScene()
    {
        SceneManager.LoadScene((int)SceneIds.PurchaseScene);
    }

    private void OnClickSalesScene()
    {
        SceneManager.LoadScene((int)SceneIds.SalesScene);
    }

    private void OnClickInnScene()
    {
        SceneManager.LoadScene((int)SceneIds.InnScene);
    }

    private void OnClickRestartButton()
    {

    }

    private void OnClickMainMenu()
    {
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }
    private void OnClickQuit()
    {

    }
}
