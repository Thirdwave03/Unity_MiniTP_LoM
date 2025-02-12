using System.Resources;
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

    public TextLocalizer wholesalesUpperText;
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

    private readonly string randomBox1Path = "Sprites/Icon/itemimg/General/RandomBox_1";
    private readonly string randomBox2Path = "Sprites/Icon/itemimg/General/RandomBox_2";
    private readonly string blankImagePath = "Sprites/Icon/itemimg/General/blank";

    public GameObject randomBoxSlot1Blind;
    public GameObject randomBoxCount1;
    public Button randomBoxSlot1B;
    public TextLocalizer randomBoxSlot1BLC;

    public TextMeshProUGUI randomBoxSlot1Price;
    public TextMeshProUGUI randomBoxSlot1Occupancy;
    public TextMeshProUGUI randomBoxSlot1Count;
    public Image randomBoxSlot1ItemImage;

    public GameObject randomBoxSlot2Blind;
    public GameObject randomBoxCount2;
    public Button randomBoxSlot2B;
    public TextLocalizer randomBoxSlot2BLC;

    public TextMeshProUGUI randomBoxSlot2Price;
    public TextMeshProUGUI randomBoxSlot2Occupancy;
    public TextMeshProUGUI randomBoxSlot2Count;
    public Image randomBoxSlot2ItemImage;

    public GameObject innMaster;

    private void Start()
    {
        AddListeners();
        UpdateInnSceneDisplay();
        bulletinBoard.SetInitialPosition();
        messageBox.SetActive(false);
        UpdateNPC();
    }

    private void OnEnable()
    {
        settingWindow.SetActive(false);
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
                Debug.Log("Raycast blocked by UI_PC");
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
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
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
                    if (!settingWindow.gameObject.activeSelf)
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

    public void UpdateInnSceneDisplay()
    {
        currentCoin.text = GameManager.Instance.coins.ToString();
        inventoryStatus.text = $"{GameManager.Instance.InventoryOccupancy} / {GameManager.Instance.InventoryCapacity}";
    }

    private void AddListeners()
    {
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
        wholesalesSlot1B.onClick.AddListener(OnClickWholesalesItemPickUp1);
        wholesalesSlot2B.onClick.AddListener(OnClickWholesalesItemPickUp2);
        randomBoxSlot1B.onClick.AddListener(OnClickRandomBoxButton1);
        randomBoxSlot2B.onClick.AddListener(OnClickRandomBoxButton2);

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
            var priceType = infoItemPriceData.MaxPrice * infoItem.ItemData.InventoryOccupancy;
            var priceTypeStringId = 999921;
            if (GameManager.Instance.isDisplayingMinPriceInfo)
            {
                priceType = infoItemPriceData.MinPrice * infoItem.ItemData.InventoryOccupancy;
                priceTypeStringId = 999920;
            }
            innWindowInfoDisplay.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999919), DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(infoItem.ItemData.StringId),
                DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(priceTypeStringId)
                ,
                priceType.ToString());
            GameManager.Instance.CallSave();
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
            .Get(999911), GameManager.Instance.InnProfitabilityRatio.ToString());
        innWindowProfitText.text = GameManager.Instance.innProfit.ToString();
    }

    public void OpenWholesales()
    {
        messageBox.SetActive(true);
        messageBoxReturnBArea.SetActive(true);
        innWindow.SetActive(false);
        wholesaleWindow.SetActive(true);
        randomBoxWindow.SetActive(false);
        centerMsg.SetActive(false);
        UpdateWholesalesContents();
    }

    private void UpdateWholesalesContents()
    {
        wholesalesUpperText.tmp.text = string.Format(
            DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999907),
            GameManager.Instance.WholesalesDiscountRatio.ToString()
            );
            

        //slot 1
        wholesalesSlot1BBlind.SetActive(GameManager.Instance.isItem1Purchased && 
            !GameManager.Instance.isItem1Pickupable);
        wholesalesSlot1Blind.SetActive(GameManager.Instance.isItem1PickedUp);

        var itemData = DataTableManager.ItemTable.Get(GameManager.Instance.wholesaleItem1);
        wholesalesSlot1ItemImage.sprite = itemData.IconSprite;
        wholesalesSlot1Count.text = GameManager.Instance.wholesaleItem1Cnt.ToString();
        wholesalesSlot1Occupancy.text = 
            (GameManager.Instance.wholesaleItem1Cnt * itemData.InventoryOccupancy).ToString();
        wholesalesSlot1Price.text = GameManager.Instance.wholesaleItem1Cost.ToString();
        wholesalesSlot1TotalPrice.text = (GameManager.Instance.wholesaleItem1Cost 
            * GameManager.Instance.wholesaleItem1Cnt).ToString();
        wholesalesSlot1BLC.stringId = GameManager.Instance.isItem1Pickupable ? 999029 : 999011;
        wholesalesSlot1BLC.OnChangeLanguage(Variables.currentLanguage);

        // slot 2
        wholesalesSlot2BBlind.SetActive(GameManager.Instance.isItem2Purchased &&
    !GameManager.Instance.isItem2Pickupable);
        wholesalesSlot2Blind.SetActive(GameManager.Instance.isItem2PickedUp);

        itemData = DataTableManager.ItemTable.Get(GameManager.Instance.wholesaleItem2);
        wholesalesSlot2ItemImage.sprite = itemData.IconSprite;
        wholesalesSlot2Count.text = GameManager.Instance.wholesaleItem2Cnt.ToString();
        wholesalesSlot2Occupancy.text =
            (GameManager.Instance.wholesaleItem2Cnt * itemData.InventoryOccupancy).ToString();
        wholesalesSlot2Price.text = GameManager.Instance.wholesaleItem2Cost.ToString();
        wholesalesSlot2TotalPrice.text = (GameManager.Instance.wholesaleItem2Cost
            * GameManager.Instance.wholesaleItem2Cnt).ToString();
        wholesalesSlot2BLC.stringId = GameManager.Instance.isItem2Pickupable ? 999029 : 999011;
        wholesalesSlot2BLC.OnChangeLanguage(Variables.currentLanguage);
    }

    public void OpenRandomBox()
    {
        messageBox.SetActive(true);
        messageBoxReturnBArea.SetActive(true);
        innWindow.SetActive(false);
        wholesaleWindow.SetActive(false);
        randomBoxWindow.SetActive(true);
        centerMsg.SetActive(false);
        UpdateRandomBoxContents();
    }

    private void UpdateRandomBoxContents()
    {
        randomBoxSlot1Blind.SetActive(GameManager.Instance.isRandomBox1PickedUp);
        randomBoxSlot1BLC.stringId = GameManager.Instance.isRandomBox1Purchased ? 999029 : 999011;
        randomBoxSlot1BLC.OnChangeLanguage(Variables.currentLanguage);
        randomBoxSlot1Price.text = GameManager.Instance.randomBox1Price.ToString();

        var randomItem = DataTableManager.ItemTable.Get(GameManager.Instance.randomBox1Item);        

        if (!GameManager.Instance.isRandomBox1Purchased)
        {
            var sprite = Resources.Load<Sprite>(randomBox1Path);
            if (sprite == null)
            {
                Debug.LogError($"Sprite at path {randomBox1Path} could not be loaded.");
            }
            else
            {
                randomBoxSlot1ItemImage.sprite = sprite;
            }
            // price of random box.
            randomBoxSlot1Occupancy.text = "???";
            randomBoxSlot1ItemImage.sprite = Resources.Load<Sprite>(randomBox1Path);
            randomBoxCount1.SetActive(false);
        }
        else
        {
            // price of random box contents subtotal.
            randomBoxSlot1Occupancy.text = (GameManager.Instance.randomBox1Cnt *
                randomItem.InventoryOccupancy)
                .ToString();
            if (!GameManager.Instance.isItem1PickedUp)
            {
                randomBoxSlot1ItemImage.sprite = randomItem.IconSprite;
            }
            else
            {
                randomBoxSlot1ItemImage.sprite = Resources.Load<Sprite>(blankImagePath);
            }
            randomBoxCount1.SetActive(true);
            randomBoxSlot1Count.text = GameManager.Instance.randomBox1Cnt.ToString();
        }
        

        // slot 2
        randomBoxSlot2Blind.SetActive(GameManager.Instance.isRandomBox2PickedUp);
        randomBoxSlot2BLC.stringId = GameManager.Instance.isRandomBox2Purchased ? 999029 : 999011;
        randomBoxSlot2BLC.OnChangeLanguage(Variables.currentLanguage);
        randomBoxSlot2Price.text = GameManager.Instance.randomBox2Price.ToString();

        randomItem = DataTableManager.ItemTable.Get(GameManager.Instance.randomBox2Item);

        if (!GameManager.Instance.isRandomBox2Purchased)
        {
            // price of random box.
            randomBoxSlot2Occupancy.text = "???";
            randomBoxSlot2ItemImage.sprite = Resources.Load<Sprite>(randomBox2Path);
            randomBoxCount2.SetActive(false);
        }
        else
        {
            // price of random box contents subtotal.
            randomBoxSlot2Occupancy.text = (GameManager.Instance.randomBox2Cnt *
                randomItem.InventoryOccupancy)
                .ToString();
            if (!GameManager.Instance.isItem2PickedUp)
            {
                randomBoxSlot2ItemImage.sprite = randomItem.IconSprite;
            }
            else
            {
                randomBoxSlot2ItemImage.sprite = Resources.Load<Sprite>(blankImagePath);
            }
            randomBoxCount2.SetActive(true);
            randomBoxSlot2Count.text = GameManager.Instance.randomBox2Cnt.ToString();
        }
    }

    public void OpenMessage(InnSceneMsgType msgType)
    {
        messageBox.SetActive(true);
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
        centerMsg.SetActive(false);
        if(!(innWindow.activeSelf || wholesaleWindow.activeSelf || randomBoxWindow.activeSelf))
        {
            messageBox.SetActive(false);
        }
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
            GameManager.Instance.CallSave();
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
        GameManager.Instance.CallSave();
    }

    private void OnClickWholesalesItemPickUp1()
    {
        if(!GameManager.Instance.isItem1Pickupable)
        {
            // purchase
            if (GameManager.Instance.coins >=
            GameManager.Instance.wholesaleItem1Cost * GameManager.Instance.wholesaleItem1Cnt)
            {
                GameManager.Instance.coins -=
                    GameManager.Instance.wholesaleItem1Cost * GameManager.Instance.wholesaleItem1Cnt;
                GameManager.Instance.isItem1Purchased = true;
                UpdateWholesalesContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.InsufficientCoin);
            }
        }
        else
        {
            // pickup
            if(GameManager.Instance.InventoryCapacity - GameManager.Instance.InventoryOccupancy
                >= GameManager.Instance.wholesaleItem1Cnt *
                DataTableManager.ItemTable.Get(GameManager.Instance.wholesaleItem1).InventoryOccupancy)
            {             
                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].avgCost =
                    (GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].count *
                    GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].avgCost +
                    GameManager.Instance.wholesaleItem1Cost * GameManager.Instance.wholesaleItem1Cnt) /
                    (GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].count +
                    GameManager.Instance.wholesaleItem1Cnt);

                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].count +=
                    GameManager.Instance.wholesaleItem1Cnt;

                GameManager.Instance.isItem1PickedUp = true;

                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].totalPurchasedCount +=
                GameManager.Instance.wholesaleItem1Cnt;
                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem1].totalPurchasedAmount +=
                    GameManager.Instance.wholesaleItem1Cnt * GameManager.Instance.wholesaleItem1Cost;

                UpdateWholesalesContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.LackOfCapacity);
            }
        }
    }

    private void OnClickWholesalesItemPickUp2()
    {
        if (!GameManager.Instance.isItem2Pickupable)
        {
            // purchase
            if (GameManager.Instance.coins >=
            GameManager.Instance.wholesaleItem2Cost * GameManager.Instance.wholesaleItem2Cnt)
            {
                GameManager.Instance.coins -=
                    GameManager.Instance.wholesaleItem2Cost * GameManager.Instance.wholesaleItem2Cnt;
                GameManager.Instance.isItem2Purchased = true;
                               
                UpdateWholesalesContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.InsufficientCoin);
            }
        }
        else
        {
            // pickup
            if (GameManager.Instance.InventoryCapacity - GameManager.Instance.InventoryOccupancy
                >= GameManager.Instance.wholesaleItem2Cnt *
                DataTableManager.ItemTable.Get(GameManager.Instance.wholesaleItem2).InventoryOccupancy)
            {
                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].avgCost =
                    (GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].count *
                    GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].avgCost +
                    GameManager.Instance.wholesaleItem2Cost * GameManager.Instance.wholesaleItem2Cnt) /
                    (GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].count +
                    GameManager.Instance.wholesaleItem2Cnt);

                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].count +=
                    GameManager.Instance.wholesaleItem2Cnt;

                GameManager.Instance.isItem2PickedUp = true;

                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].totalPurchasedCount +=
                    GameManager.Instance.wholesaleItem2Cnt;
                GameManager.Instance.entireItemDict[GameManager.Instance.wholesaleItem2].totalPurchasedAmount +=
                    GameManager.Instance.wholesaleItem2Cnt * GameManager.Instance.wholesaleItem2Cost;

                UpdateWholesalesContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.LackOfCapacity);
            }
        }
    }

    private void OnClickRandomBoxButton1()
    {
        if(!GameManager.Instance.isRandomBox1Purchased)
        {
            // Purchase
            if(GameManager.Instance.coins >= GameManager.Instance.randomBox1Price)
            {
                GameManager.Instance.coins -= GameManager.Instance.randomBox1Price;
                GameManager.Instance.isRandomBox1Purchased = true;
                UpdateRandomBoxContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.InsufficientCoin);
            }
        }
        else
        {
            // Pick up
            if(GameManager.Instance.InventoryCapacity - GameManager.Instance.InventoryOccupancy
                >= GameManager.Instance.randomBox1Cnt * DataTableManager.ItemTable.
                Get(GameManager.Instance.randomBox1Item).InventoryOccupancy)
            {
                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].avgCost =
                    (GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].avgCost
                    * GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].count +
                    GameManager.Instance.randomBox1Price) /
                    (GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].count
                    + GameManager.Instance.randomBox1Cnt);

                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].count +=
                    GameManager.Instance.randomBox1Cnt;
                GameManager.Instance.isRandomBox1PickedUp = true;

                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].totalPurchasedCount +=
                    GameManager.Instance.randomBox1Cnt;
                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox1Item].totalPurchasedAmount +=
                    GameManager.Instance.randomBox1Price;

                UpdateRandomBoxContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.LackOfCapacity);
            }
        }
    }

    private void OnClickRandomBoxButton2()
    {
        if (!GameManager.Instance.isRandomBox2Purchased)
        {
            // Purchase
            if (GameManager.Instance.coins >= GameManager.Instance.randomBox2Price)
            {
                GameManager.Instance.coins -= GameManager.Instance.randomBox2Price;
                GameManager.Instance.isRandomBox2Purchased = true;
                UpdateRandomBoxContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.InsufficientCoin);
            }
        }
        else
        {
            // Pick up
            if (GameManager.Instance.InventoryCapacity - GameManager.Instance.InventoryOccupancy
                >= GameManager.Instance.randomBox2Cnt * DataTableManager.ItemTable.
                Get(GameManager.Instance.randomBox2Item).InventoryOccupancy)
            {
                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].avgCost =
                    (GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].avgCost
                    * GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].count +
                    GameManager.Instance.randomBox2Price) /
                    (GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].count
                    + GameManager.Instance.randomBox2Cnt);

                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].count +=
                    GameManager.Instance.randomBox2Cnt;
                GameManager.Instance.isRandomBox2PickedUp = true;


                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].totalPurchasedCount +=
                    GameManager.Instance.randomBox2Cnt;
                GameManager.Instance.entireItemDict[GameManager.Instance.randomBox2Item].totalPurchasedAmount +=
                    GameManager.Instance.randomBox2Price;

                UpdateRandomBoxContents();
                UpdateInnSceneDisplay();
                GameManager.Instance.CallSave();
            }
            else
            {
                OpenMessage(InnSceneMsgType.LackOfCapacity);
            }
        }
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
        if (GameManager.Instance.isInnMasterAvailable)
        {
            innMaster.SetActive(true);
        }
        else
        {
            innMaster.SetActive(false);
        }       
    }
}
