using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
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


    private void Start()
    {
        AddListeners();
        UpdatePurchaseSceneDisplay();
        bulletinBoard.SetInitialPosition();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
        purchaseWindow.SetActive(false);
    }


    private void Update()
    {
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
    }

    public void UpdatePurchaseSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.Coins.ToString();
        inventoryStatus.text = $"{GameManager.Instance.InventoryOccupancy} / {GameManager.Instance.inventoryCapacity}";
    }

    private void AddListeners()
    {
        //inventoryButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        innSceneButton.onClick.RemoveAllListeners();
        mainSceneButton.onClick.RemoveAllListeners();
        salesSceneButton.onClick.RemoveAllListeners();
        settingCloseButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        purchaseWindowClose.onClick.RemoveAllListeners();

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
}
