using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PurchaseSceneUiManager : MonoBehaviour
{
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

    public GameObject purchaseWindow;
    public Button purchaseWindowClose;

    public UiBulletinBoardManager bulletinBoard;

    public TextMeshProUGUI currentCoin;
    public TextMeshProUGUI inventoryStatus;

    private PurchaseSceneCenterMsgType messageType;

    public GameObject messageBox;
    public GameObject centerMsg;

    public GameObject centerMsgCheckBArea;

    public Button centerMsgCheckB;
    public Button centerMsgCloseB;

    public TextLocalizer centerMsgLC;

    public GameObject primaryShop;
    public GameObject secondaryShop;
    public GameObject luxuryShop;

    private void Start()
    {
        AddListeners();
        UpdatePurchaseSceneDisplay();
        bulletinBoard.SetInitialPosition();
        messageBox.gameObject.SetActive(false);
        UpdateNPC();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        purchaseWindow.SetActive(false);
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;
    }


    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Raycast blocked by UI");
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (!settingWindow.gameObject.activeSelf && !purchaseWindow.gameObject.activeSelf)
                {
                    hit.collider.gameObject.GetComponent<NpcButton>().InvokeOnClick();
                }
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("Raycast blocked by UI_Mobile");
                    return;
                }

                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

                if (hit.collider != null)
                {
                    if (!settingWindow.gameObject.activeSelf && !purchaseWindow.gameObject.activeSelf)
                    {
                        NpcButton npcButton = hit.collider.gameObject.GetComponent<NpcButton>();
                        if (npcButton != null)
                        {
                            npcButton.InvokeOnClick();
                        }
                    }
                }
            }
        }
#endif
    }

    public void UpdatePurchaseSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.coins.ToString();
        inventoryStatus.text = $"{GameManager.Instance.InventoryOccupancy} / {GameManager.Instance.InventoryCapacity}";
    }

    private void AddListeners()
    {
        //inventoryButton.onClick.AddListener(OnClickTemp);
        settingButton.onClick.AddListener(OnClickSetting);
        mainSceneButton.onClick.AddListener(OnClickMainScene);
        salesSceneButton.onClick.AddListener(OnClickSalesScene);
        innSceneButton.onClick.AddListener(OnClickInnScene);
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickRestartButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        quitButton.onClick.AddListener(OnClickQuit);
        purchaseWindowClose.onClick.AddListener(OnClickPurchaseWindowClose);
        centerMsgCheckB.onClick.AddListener(OnClickCenterMsgCheck);
        centerMsgCloseB.onClick.AddListener(OnClickCenterMsgClose);

        // sliders
        bgmSlider.onValueChanged.AddListener(OnValueChangeBGM);
        sfxSlider.onValueChanged.AddListener(OnValueChangeSFX);
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
        GameManager.Instance.Restart();
        SceneManager.LoadScene((int)SceneIds.MainScene);
    }

    private void OnClickMainMenu()
    {
        SceneManager.LoadScene((int)SceneIds.TitleScene);
    }
    private void OnClickQuit()
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

    public void OpenPrimaryShop()
    {
        purchaseWindow.SetActive(true);
        var purchaseBoard = purchaseWindow.GetComponent<UiPurchasePanel>().purchaseBoard;
        purchaseBoard.AllignIndexWithShopType
            (SalesItemDataIndex.minPrimary, SalesItemDataIndex.maxPrimary);
        purchaseBoard.CallUpdateSlots();
    }

    public void OpenSecondaryShop()
    {
        purchaseWindow.SetActive(true);
        var purchaseBoard = purchaseWindow.GetComponent<UiPurchasePanel>().purchaseBoard;
        purchaseBoard.AllignIndexWithShopType
            (SalesItemDataIndex.minSecondary, SalesItemDataIndex.maxSecondary);
        purchaseBoard.CallUpdateSlots();
    }

    public void OpenLuxuryShop()
    {
        purchaseWindow.SetActive(true);
        var purchaseBoard = purchaseWindow.GetComponent<UiPurchasePanel>().purchaseBoard;
        purchaseBoard.AllignIndexWithShopType
            (SalesItemDataIndex.minLuxury, SalesItemDataIndex.maxLuxury);
        purchaseBoard.CallUpdateSlots();
    }

    private void OnClickPurchaseWindowClose()
    {
        purchaseWindow.gameObject.GetComponent<UiPurchasePanel>().itemInfo.blinder.SetActive(true);
        purchaseWindow.SetActive(false);
    }


    public void OpenMessage(PurchaseSceneCenterMsgType msgType)
    {
        messageBox.SetActive(true);
        messageType = msgType;
        switch (msgType)
        {
            case PurchaseSceneCenterMsgType.InsufficientCoin:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999902);
                break;
            case PurchaseSceneCenterMsgType.LackOfCapacity:
                centerMsg.SetActive(true);
                centerMsgCheckBArea.SetActive(false);
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999903);
                break;
            default:
                break;
        }
    }

    private void OnClickCenterMsgCheck()
    {
        switch (messageType)
        {
            default:
                break;
        }
    }

    private void OnClickCenterMsgClose()
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

    private void UpdateNPC()
    {
        if(GameManager.Instance.isPrimaryShopAvailable)
        {
            primaryShop.SetActive(true);
        }
        else
        {
            primaryShop.SetActive(false);
        }

        if (GameManager.Instance.isSecondaryShopAvailable)
        {
            secondaryShop.SetActive(true);
        }
        else
        {
            secondaryShop.SetActive(false);
        }

        if(GameManager.Instance.isLuxuryShopAvailable)
        {
            luxuryShop.SetActive(true);
        }
        else
        {
            luxuryShop.SetActive(false);
        }
    }
}