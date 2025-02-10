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

    public TextMeshProUGUI currentCoin;
    public TextMeshProUGUI daysProgress;

    public TextLocalizer tipsLC;

    public TextLocalizer inventoryLevelLC;
    public TextLocalizer inventoryStatusLC;

    private MainMenuCenterMsgType messageType;

    public GameObject messageBox;
    public GameObject centerMsg;
    public GameObject nextDay;

    public GameObject centerMsgCheckBArea;

    public Button centerMsgCheckB;
    public Button centerMsgCloseB;

    public Button nextDayCheckB;
    public Button nextDayCloseB;

    public TextLocalizer centerMsgLC;
    public TextLocalizer nextDayLC;

    public Button tutorialWindow;
    public TextMeshProUGUI tutorialText;
    public Button tutorialSkipB;
    public Button tutorialPrevB;
    private int tutorialStringId = 0;
    private bool ifDefaultFiftythDay = false;


    private void Start()
    {
        //DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        AddListeners();
        AddLocalizerActions();
        UpdateMainSceneDisplay();
        GameManager.Instance.Init();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        inventoryWindow.SetActive(false);
        messageBox.SetActive(false);
        centerMsg.SetActive(false);
        nextDay.SetActive(false);
        CheckTutorialNeccesity();
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;
    }

    private void UpdateMainSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.coins.ToString();
        daysProgress.text = GameManager.Instance.days.ToString();
        tipsLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(GameManager.Instance.tipIndex);
        inventoryLevelLC.OnChangeLanguage(Variables.currentLanguage);
        inventoryStatusLC.OnChangeLanguage(Variables.currentLanguage);
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
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickSettingRestart);
        mainMenuButton.onClick.AddListener(OnClickSettingMainMenu);
        quitButton.onClick.AddListener(OnClickSettingQuit);        

        // inventory contents
        inventoryReturnButton.onClick.AddListener(OnClickInventoryReturn);

        centerMsgCheckB.onClick.AddListener(OnClickCenterMsgCheck);
        centerMsgCloseB.onClick.AddListener(OnClickCenterMsgClose);
        nextDayCloseB.onClick.AddListener(OnClickNextdayClose);
        nextDayCheckB.onClick.AddListener(OnClickNextdayCheck);

        // tutorial contents
        tutorialWindow.onClick.AddListener(OnClickTutorialWindow);
        tutorialSkipB.onClick.AddListener(OnClickTutorialSkip);
        tutorialPrevB.onClick.AddListener(OnClickTutorialPrev);

        // sliders
        bgmSlider.onValueChanged.AddListener(OnValueChangeBGM);
        sfxSlider.onValueChanged.AddListener(OnValueChangeSFX);
    }

    private void CheckTutorialNeccesity()
    {
        if (GameManager.Instance.isDisplayTutorial)
        {
            tutorialStringId = GameInfos.tutorialStringIdBegin;
            tutorialWindow.gameObject.SetActive(true);
            tutorialText.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(tutorialStringId);
            tutorialPrevB.interactable = false;
            if(GameManager.Instance.isFirstTimeEver)
            {
                tutorialSkipB.gameObject.SetActive(false);
            }
            else
            {
                tutorialSkipB.gameObject.SetActive(true);
            }
        }
        else
        {
            tutorialWindow.gameObject.SetActive(false);
        }
        GameManager.Instance.isFirstTimeEver = false;
    }

    private void OnClickTutorialWindow()
    {
        if (tutorialStringId < GameInfos.tutorialStringIdEnd)
        {
            tutorialText.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(++tutorialStringId);
            if(tutorialStringId == GameInfos.tutorialStringIdEnd)
            {
                tutorialSkipB.gameObject.SetActive(true);
            }
        }
        else
        {            
            GameManager.Instance.isDisplayTutorial = false;           
        }
        UpdatePrevButtonAvailability();
    }

    private void OnClickTutorialSkip()
    {
        GameManager.Instance.isDisplayTutorial = false;
        tutorialWindow.gameObject.SetActive(false);
    }

    private void OnClickTutorialPrev()
    {
        //if (tutorialStringId < GameInfos.tutorialStringIdEnd)
        //{
        //    tutorialText.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
        //        .Get(++tutorialStringId);
        //}
        //else
        //{
        //    GameManager.Instance.isDisplayTutorial = false;
        //    tutorialWindow.gameObject.SetActive(false);
        //}
        if (tutorialStringId > GameInfos.tutorialStringIdBegin)
        {
            tutorialText.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(--tutorialStringId);
        }
        UpdatePrevButtonAvailability();
    }

    private void UpdatePrevButtonAvailability()
    {
        if(tutorialStringId > GameInfos.tutorialStringIdBegin)
        {
            tutorialPrevB.interactable = true;
        }
        else
        {
            tutorialPrevB.interactable = false;
        }
    }

    private void OnClickInventory()
    {
        inventoryWindow.SetActive(true);
    }

    private void OnClickInventoryUpgrade()
    {
        if(GameManager.Instance.inventoryLevel != GameManager.Instance.inventoryMaxLevel)
        {
            OpenMessage(MainMenuCenterMsgType.InventoryUpgrade);
        }
        else
        {
            OpenMessage(MainMenuCenterMsgType.InventoryLevelMax);
        }
    }

    private void OnClickInventoryDowngrade()
    {
        if (GameManager.Instance.inventoryLevel != GameManager.Instance.inventoryMinLevel)
        {
            if (GameManager.Instance.InventoryOccupancy <=
                DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel - 1).Capacity)
            {
                OpenMessage(MainMenuCenterMsgType.InventoryDowngrade);
            }
            else
            {
                OpenMessage(MainMenuCenterMsgType.LackOfCapacity);
            }
        }
        else
        {
            OpenMessage(MainMenuCenterMsgType.InventoryLevelMin);
        }
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
        if (GameManager.Instance.days >= GameManager.Instance.lastDay)
        {
            OpenMessage(MainMenuCenterMsgType.LastDay);
            return;
        }
        if(GameManager.Instance.days == 50 && 
            GameManager.Instance.CurrentGameMode == GameModes.Default &&
            GameManager.Instance.coins < GameManager.Instance.inventoryFee + 20000)
        {
            ifDefaultFiftythDay = true;
            OpenMessage(MainMenuCenterMsgType.CannotProceed);
        }
        else if (GameManager.Instance.coins >= GameManager.Instance.inventoryFee)
        { 
            OpenMessage(MainMenuCenterMsgType.CanProceed); 
        }
        else
        {
            ifDefaultFiftythDay = false;
            OpenMessage(MainMenuCenterMsgType.CannotProceed);
        }
    }

    private void OnClickSettingClose()
    {
      
        settingWindow.SetActive(false);
    }

    private void OnClickSettingRestart()
    {
        GameManager.Instance.Restart();
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickSettingMainMenu()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }

    private void OnClickSettingQuit()
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

    private void OnClickInventoryReturn()
    {
        inventoryWindow.gameObject.GetComponent<UiInventoryPanel>().itemInfo.blinder.SetActive(true);
        inventoryWindow.SetActive(false);
    }

    private void AddLocalizerActions()
    {
        inventoryLevelLC.customizedFormat += LocalizerActionInventoryLvl;
        inventoryStatusLC.customizedFormat += LocalizerActionInventoryStatus;
    }

    private void LocalizerActionInventoryLvl()
    {
        //inventoryLevelLC.stringId = -1;
        inventoryLevelLC.formatContents.Clear();
        inventoryLevelLC.formatContents.Add(GameManager.Instance.inventoryLevel.ToString());
    }

    private void LocalizerActionInventoryStatus()
    {
        //inventoryStatusLC.stringId = -1;
        inventoryStatusLC.formatContents.Clear();
        inventoryStatusLC.formatContents.Add(GameManager.Instance.InventoryOccupancy.ToString());
        inventoryStatusLC.formatContents.Add(GameManager.Instance.inventoryCapacity.ToString());
        inventoryStatusLC.formatContents.Add(GameManager.Instance.inventoryFee.ToString());
    }

    private void OpenMessage(MainMenuCenterMsgType msgType)
    {
        messageBox.SetActive(true);
        messageType = msgType;
        switch (msgType)
        {
            case MainMenuCenterMsgType.InventoryLevelMax:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999913);
                break;
            case MainMenuCenterMsgType.InventoryLevelMin:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999914);
                break;
            case MainMenuCenterMsgType.InventoryUpgrade:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(true);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999905),
                DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel + 1).UpgradeCost, 
                DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel + 1).DailyCost);
                break;
            case MainMenuCenterMsgType.InventoryDowngrade:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(true);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999906),                
                DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel - 1).DailyCost);
                break;
            case MainMenuCenterMsgType.InsufficientCoin:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999902);
                break;
            case MainMenuCenterMsgType.LackOfCapacity:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999915),
                DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel - 1).Capacity);
                break;
            case MainMenuCenterMsgType.CannotProceed:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(true);
                nextDay.SetActive(false);
                if (ifDefaultFiftythDay)
                {
                    centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999927);
                }
                else
                {
                    centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999901);
                }
                break;
            case MainMenuCenterMsgType.CanProceed:
                centerMsg.SetActive(false);
                nextDay.SetActive(true);
                nextDayLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999904);
                break;
            case MainMenuCenterMsgType.LastDay:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(true);
                nextDay.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999926);
                break;
            default:
                break;
        }
    }

    private void OnClickCenterMsgCheck()
    {
        switch (messageType)
        {
            case MainMenuCenterMsgType.InventoryUpgrade:
                if(GameManager.Instance.coins >= 
                    DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel+1).UpgradeCost)
                {
                    GameManager.Instance.coins -= DataTableManager.InventoryTable.Get(GameManager.Instance.inventoryLevel + 1).UpgradeCost;
                    GameManager.Instance.ChangeInventoryLevel(GameManager.Instance.inventoryLevel + 1);
                    GameManager.Instance.CallSave();
                    UpdateMainSceneDisplay();
                    messageBox.SetActive(false);
                }
                else
                {
                    OpenMessage(MainMenuCenterMsgType.InsufficientCoin);
                }
                break;
            case MainMenuCenterMsgType.InventoryDowngrade:
                GameManager.Instance.ChangeInventoryLevel(GameManager.Instance.inventoryLevel - 1);
                GameManager.Instance.CallSave();
                UpdateMainSceneDisplay();
                messageBox.SetActive(false);
                break;
            case MainMenuCenterMsgType.CannotProceed:
                SaveLoadManager.DeleteSlot(GameManager.Instance.currentSavedSlotIndex);
                SceneManager.LoadScene((int)SceneIds.TitleScene);
                break;
            case MainMenuCenterMsgType.LastDay:
                GameManager.Instance.OnSleepLastDay();
                SaveLoadManager.DeleteSlot(GameManager.Instance.currentSavedSlotIndex);
                SceneManager.LoadScene((int)SceneIds.TitleScene);
                break;
            default:
                break;  
        }
    }

    private void OnClickCenterMsgClose()
    {
        messageBox.SetActive(false);
    }

    private void OnClickNextdayCheck()
    {      
        GameManager.Instance.OnSleep();
        UpdateMainSceneDisplay();
        messageBox.SetActive(false);
    }
    private void OnClickNextdayClose()
    {
        messageBox.SetActive(false);
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
