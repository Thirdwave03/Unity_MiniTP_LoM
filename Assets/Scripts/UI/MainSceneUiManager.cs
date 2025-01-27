using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Button settingCloseButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    public GameObject inventoryWindow;
    public Button inventoryReturnButton;

    public UiBulletinBoardManager bulletinBoard;

    public TextMeshProUGUI currentCoin;
    public TextMeshProUGUI daysProgress;

    public TextMeshProUGUI tips;

    public TextMeshProUGUI inventoryLevel;
    public TextMeshProUGUI inventoryStatus;

    private void Start()
    {
        //DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        AddListeners();
        UpdateMainSceneDisplay();
        bulletinBoard.SetInitialPosition();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        inventoryWindow.SetActive(false);
    }


    private void UpdateMainSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.Coins.ToString();
        daysProgress.text = GameManager.Instance.Days.ToString();
        //tips.text = GameManager.Instance. (Not Ready yet)
        inventoryLevel.text = GameManager.Instance.inventoryLevel.ToString();
        inventoryStatus.text = $"Capacity: (TBD)/{GameManager.Instance.inventoryCapacity}\n" +
            $"Rental Fee: {GameManager.Instance.inventoryFee}/Day";
    }        

    private void AddListeners()
    {
        // scene contents
        inventoryButton.onClick.RemoveAllListeners();
        inventoryUpgrade.onClick.RemoveAllListeners();
        inventoryDowngrade.onClick.RemoveAllListeners();
        informationButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        purchaseSceneButton.onClick.RemoveAllListeners();
        salesSceneButton.onClick.RemoveAllListeners();
        innSceneButton.onClick.RemoveAllListeners();
        sleepButton.onClick.RemoveAllListeners();

        // setting contents
        settingCloseButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(OnClickSettingQuit);

        // inventory contents
        inventoryReturnButton.onClick.RemoveAllListeners();

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
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickSettingRestart);
        mainMenuButton.onClick.AddListener(OnClickSettingMainMenu);
        quitButton.onClick.AddListener(OnClickSettingQuit);

        // inventory contents
        inventoryReturnButton.onClick.AddListener(OnClickInventoryReturn);
    }

    private void OnClickInventory()
    {
        inventoryWindow.SetActive(true);
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

    private void OnClickSleep()
    {
        GameManager.Instance.OnSleep();
        UpdateMainSceneDisplay();
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
        gameObject.SetActive(false);
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }

    private void OnClickSettingQuit()
    {
        
    }

    private void OnClickInventoryReturn()
    {
        inventoryWindow.SetActive(false);
    }
}
