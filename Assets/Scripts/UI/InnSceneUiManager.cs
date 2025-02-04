using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InnSceneUiManager : MonoBehaviour
{
    public Button inventoryButton;
    public Button settingButton;

    public Button mainSceneButton;
    public Button purchaseSceneButton;
    public Button salesSceneButton;

    public GameObject settingWindow;
    public Button settingCloseButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    public UiBulletinBoardManager bulletinBoard;

    public TextMeshProUGUI currentCoin;
    public TextMeshProUGUI inventoryStatus;

    private InnSceneMsgType messageType;

    public GameObject messageBox;
    public GameObject centerMsg;

    public GameObject centerMsgCheckBArea;
    public GameObject messageBoxReturnBArea;

    public Button messageBoxReturnB;
    public Button centerMsgCheckB;
    public Button centerMsgCloseB;

    public TextLocalizer centerMsgLC;

    public GameObject innWindow;
    public GameObject wholesaleWindow;
    public GameObject randomBoxWindow;

    public GameObject innWindowGetIntelBArea;
    public Button innWindowGetIntelB;
    public GameObject innWindowGetProfitBArea;
    public Button innWindowGetProfitB;
    public TextLocalizer innWindowInfoDisplay;
    public TextLocalizer innWindowTotalInvest;
    public TextLocalizer innWindowInvestTips;
    public TextMeshProUGUI innWindowProfitText;
    public Button innWindowInvestB_1000;
    public Button innWindowInvestB_5000;
    public Button innWindowInvestB_10000;
    public Button innWindowInvestB_50000;

    public GameObject wholesalesSlot1Blind;
    public Button wholesalesSlot1B;
    public GameObject wholesalesSlot1BBlind;
    public TextLocalizer wholesalesSlot1BLC;

    public TextMeshProUGUI wholesalesSlot1Price;
    public TextMeshProUGUI wholesalesSlot1TotalPrice;
    public TextMeshProUGUI wholesalesSlot1Occupancy;
    public TextMeshProUGUI wholesalesSlot1Count;
    public Image wholesalesSlot1ItemImage;

    public GameObject wholesalesSlot2Blind;
    public Button wholesalesSlot2B;
    public GameObject wholesalesSlot2BBlind;
    public TextLocalizer wholesalesSlot2BLC;

    public TextMeshProUGUI wholesalesSlot2Price;
    public TextMeshProUGUI wholesalesSlot2TotalPrice;
    public TextMeshProUGUI wholesalesSlot2Occupancy;
    public TextMeshProUGUI wholesalesSlot2Count;
    public Image wholesalesSlot2ItemImage;

    public GameObject randomBoxSlot1Blind;
    public Button randomBoxSlot1B;
    public GameObject randomBoxSlot1BBlind;
    public TextLocalizer randomBoxSlot1BLC;

    public TextMeshProUGUI randomBoxSlot1TotalPrice;
    public TextMeshProUGUI randomBoxSlot1Occupancy;
    public TextMeshProUGUI randomBoxSlot1Count;
    public Image randomBoxSlot1ItemImage;

    public GameObject randomBoxSlot2Blind;
    public Button randomBoxSlot2B;
    public GameObject randomBoxSlot2BBlind;
    public TextLocalizer randomBoxSlot2BLC;

    public TextMeshProUGUI randomBoxSlot2TotalPrice;
    public TextMeshProUGUI randomBoxSlot2Occupancy;
    public TextMeshProUGUI randomBoxSlot2Count;
    public Image randomBoxSlot2ItemImage;

    private void Start()
    {
        AddListeners();
        UpdateInnSceneDisplay();
        bulletinBoard.SetInitialPosition();
        messageBox.SetActive(false);
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
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
                if (!settingWindow.gameObject.activeSelf)
                {
                    hit.collider.gameObject.GetComponent<NpcButton>().InvokeOnClick();
                }
            }
        }
    }

    public void UpdateInnSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.coins.ToString();
        inventoryStatus.text = $"{GameManager.Instance.InventoryOccupancy} / {GameManager.Instance.inventoryCapacity}";
    }

    private void AddListeners()
    {
        //inventoryButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        purchaseSceneButton.onClick.RemoveAllListeners();
        settingCloseButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        //inventoryButton.onClick.AddListener(OnClickTemp);
        settingButton.onClick.AddListener(OnClickSetting);
        mainSceneButton.onClick.AddListener(OnClickMainScene);
        purchaseSceneButton.onClick.AddListener(OnClickPurchaseScene);
        salesSceneButton.onClick.AddListener(OnClickSalesScene);
        settingCloseButton.onClick.AddListener(OnClickSettingClose);
        restartButton.onClick.AddListener(OnClickRestartButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        quitButton.onClick.AddListener(OnClickQuit);
        messageBoxReturnB.onClick.AddListener(OnClickMsgBoxReturn);
        centerMsgCloseB.onClick.AddListener(OnClickCenterMsgClose);
        

        innWindowGetIntelB.onClick.AddListener(OnClickGetPriceInfo);
        innWindowGetProfitB.onClick.AddListener(OnClickRetrieveProfit);
        innWindowInvestB_1000.onClick.AddListener(OnClickInvest_1000);
        innWindowInvestB_5000.onClick.AddListener(OnClickInvest_5000);
        innWindowInvestB_10000.onClick.AddListener(OnClickInvest_10000);
        innWindowInvestB_50000.onClick.AddListener(OnClickInvest_50000);
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

    public void OpenInnMaster()
    {
        messageBox.SetActive(true);
        messageBoxReturnBArea.SetActive(true);
        innWindow.SetActive(true);
        wholesaleWindow.SetActive(false);
        randomBoxWindow.SetActive(false);
        centerMsg.SetActive(false);
        UpdatePriceInfoDisplay();
        UpdateInvestContents();
    }

    private void UpdatePriceInfoDisplay()
    {
        bool isInfoOpened = GameManager.Instance.isInfoOpened;
        innWindowGetIntelBArea.SetActive(!isInfoOpened);
        if (isInfoOpened)
        {
            var infoItem = GameManager.Instance.entireItemDict[GameManager.Instance.infoItemIndex];
            var infoItemPriceData = DataTableManager.PriceTable.Get(infoItem.priceID);
            var priceType = infoItemPriceData.MaxPrice;
            var priceTypeStringId = 999921;
            if (GameManager.Instance.isDisplayingMinPriceInfo)
            {
                priceType = infoItemPriceData.MinPrice;
                priceTypeStringId = 999920;
            }
            innWindowInfoDisplay.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999919), DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(infoItem.ItemData.StringId),
                DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(priceTypeStringId),
                priceType.ToString());
        }
        else
        {
            innWindowInfoDisplay.tmp.text = "";
        }
    }

    private void UpdateInvestContents()
    {
        innWindowGetProfitBArea.SetActive(GameManager.Instance.innProfit != 0);
        innWindowTotalInvest.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999020), GameManager.Instance.investedAmount.ToString());
        innWindowInvestTips.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999911), GameManager.Instance.investProfitRatio.ToString());
        innWindowProfitText.text = GameManager.Instance.innProfit.ToString();
    }

    public void OpenWholesales()
    {

    }

    public void OpenRandomBox()
    {

    }

    public void OpenMessage(InnSceneMsgType msgType)
    {
        messageBox.SetActive(true);
        messageBoxReturnBArea.SetActive(false);
        centerMsg.SetActive(true);
        centerMsgCheckBArea.SetActive(false);
        messageType = msgType;
        switch (msgType)
        {
            case InnSceneMsgType.InsufficientCoin:
                centerMsgLC.tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999902);
                break;
            case InnSceneMsgType.LackOfCapacity:
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

    private void OnClickMsgBoxReturn()
    {
        messageBox.SetActive(false);
    }

    private void OnClickGetPriceInfo()
    {
        if(GameManager.Instance.coins >= GameInfos.priceInfoCost)
        {
            GameManager.Instance.isInfoOpened = true;
            GameManager.Instance.coins -= GameInfos.priceInfoCost;
            UpdatePriceInfoDisplay();
            UpdateInnSceneDisplay();
        }
        else
        {
            OpenMessage(InnSceneMsgType.InsufficientCoin);
        }
    }

    private void OnClickInvest(int investAmount)
    {
        if(GameManager.Instance.coins >= investAmount)
        {
            GameManager.Instance.coins -= investAmount;
            GameManager.Instance.investedAmount += investAmount;
            UpdateInvestContents();
            UpdateInnSceneDisplay();
        }
        else
        {
            OpenMessage(InnSceneMsgType.InsufficientCoin);
        }
    }

    private void OnClickInvest_1000()
    {
        OnClickInvest(1000);
    }
    private void OnClickInvest_5000()
    {
        OnClickInvest(5000);
    }
    private void OnClickInvest_10000()
    {
        OnClickInvest(10000);
    }
    private void OnClickInvest_50000()
    {
        OnClickInvest(50000);
    }

    private void OnClickRetrieveProfit()
    {
        GameManager.Instance.coins += GameManager.Instance.innProfit;
        GameManager.Instance.innProfit = 0;
        UpdateInvestContents();
        UpdateInnSceneDisplay();
    }
}
