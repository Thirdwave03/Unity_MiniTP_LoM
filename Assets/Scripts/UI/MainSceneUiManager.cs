using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneUiManager : MonoBehaviour
{
    public Button inventoryButton;
    public Button inventoryUpgrade;
    public Button inventoryDowngrade;
    public Button informationButton;
    public Button settingButton;
    public Button purchaseSceneButton;
    public Button salesSceneButton;
    public Button innSceneButton;
    public Button sleepButton;

    public GameObject settingWindow;
    public Button closeButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        AddListeners();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
    }

    private void AddListeners()
    {
        // scene contents
        inventoryButton.onClick.AddListener(OnClickInventory);
        inventoryUpgrade.onClick.AddListener(OnClickInventoryUpgrade);
        inventoryDowngrade.onClick.AddListener(OnClickInventoryDowngrade);
        informationButton.onClick.AddListener(OnClickInformation);
        settingButton.onClick.AddListener(OnClickSettings);
        purchaseSceneButton.onClick.AddListener(OnClickPurchaseScene);
        salesSceneButton.onClick.AddListener(OnClickSalesScene);
        innSceneButton.onClick.AddListener(OnClickInnScene);
        sleepButton.onClick.AddListener(OnClickSleep);

        // setting contents
        closeButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickSettingRestart);
        mainMenuButton.onClick.AddListener(OnClickSettingMainMenu);
        quitButton.onClick.AddListener(OnClickSettingQuit);
    }

    private void OnClickInventory()
    {
      
    }

    private void OnClickInventoryUpgrade()
    {
      
    }

    private void OnClickInventoryDowngrade()
    {
      
    }

    private void OnClickInformation()
    {
      
    }

    private void OnClickSettings()
    {
      
        settingWindow.SetActive(true);
    }

    private void OnClickPurchaseScene()
    {
      
    }

    private void OnClickSalesScene()
    {
      
    }

    private void OnClickInnScene()
    {
      
    }

    private void OnClickSleep()
    {
      
    }

    private void OnClickSettingClose()
    {
      
        settingWindow.SetActive(false);
    }

    private void OnClickSettingRestart()
    {
      
    }

    private void OnClickSettingMainMenu()
    {
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }

    private void OnClickSettingQuit()
    {
      
    }
}
