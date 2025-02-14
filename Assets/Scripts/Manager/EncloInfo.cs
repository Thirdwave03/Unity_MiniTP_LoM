using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncloInfo : MonoBehaviour
{
    public EncloWindow encloWindow;

    public SavedItemData ItemData {  get; private set; }
    public Image itemIcon;
    public Image registeredIcon;

    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI highPrice;
    public TextMeshProUGUI lowPrice;
    public TextMeshProUGUI otherInfo;

    public Button registerButton;
    public TextLocalizer registerBText;

    public GameObject blinder;

    public TextMeshProUGUI registerCountIndicator;

    private readonly Color colorGrey = new Color(0.5f,0.5f,0.5f);
    private readonly Color colorRed = new Color(1f, 0, 0);
    private readonly Color colorGreen = new Color(0.05f, 0.35f, 0.03f);

    private void Awake()
    {
        blinder.SetActive(true);
        registerButton.interactable = false;
    }

    private void Start()
    {
        AddListeners();
    }

    private void OnEnable()
    {
        blinder.SetActive(true);
        registerButton.interactable = false;
    }

    public void SetEmpty()
    {
        ItemData = null;
        itemIcon.sprite = null;
        registeredIcon.sprite = null;
        itemDesc.text = "???";

        highPrice.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999953), "???");
        lowPrice.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999954), "???");

        otherInfo.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999929), "???", "???", "???", "???", "???"
            );

        registerButton.interactable = false;
        registerBText.tmp.text =
            DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
            .Get(999060);

        blinder.SetActive(true);
    }

    public void SetData(SavedItemData itemData)
    {
        if (itemData == null)
        {
            SetEmpty();
            return;
        }
        blinder.SetActive(false);

        ItemData = itemData;
        if (SaveLoadManager.BaseData.isItemRevealed[ItemData.ItemData.Id - ItemDataIndex.minPrimary])
        {
            if (SaveLoadManager.BaseData.MerchantRank < MerchantRanks.MerchantGod)
            {
                registerButton.interactable = false;
                registerBText.tmp.text =
                DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
                .Get(999060);
                registerButton.GetComponent<Image>().color = colorGrey;
            }
            else
            {
                registerButton.interactable = true;
                registerBText.tmp.text =
                DataTableManager.StringTableList[(int)(Variables.currentLanguage)]
                .Get(999063);
                registerButton.GetComponent<Image>().color = colorRed;
            }

            itemIcon.sprite = DataTableManager.ItemTable.Get(itemData.ItemData.Id).IconSprite;            
            registeredIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/Books/technical book_1");

            itemDesc.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(ItemData.ItemData.ItemDescription);

            highPrice.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999953), GameManager.Instance.entireItemDict[ItemData.ItemData.Id]
                .highestPrice.ToString());
            lowPrice.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999954), GameManager.Instance.entireItemDict[ItemData.ItemData.Id]
                .lowestPrice.ToString());

            int tempPurchasedCount = 1;
            int tempSoldCount = 1;
            if(GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalPurchasedCount != 0)
            {
                tempPurchasedCount = 
                    GameManager.Instance.entireItemDict[ItemData.ItemData.Id]
                    .totalPurchasedCount;
            }
            if(GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalSoldCount != 0)
            {
                tempSoldCount = GameManager.Instance.entireItemDict[ItemData.ItemData.Id]
                    .totalSoldCount;
            }

            otherInfo.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999929),
            GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalPurchasedAmount.ToString(),
            (GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalPurchasedAmount /
            tempPurchasedCount).ToString(),
            GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalSoldAmount.ToString(),
            (GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalSoldAmount /
            tempSoldCount).ToString(),
            GameManager.Instance.entireItemDict[ItemData.ItemData.Id].totalPurchasedCount.ToString()
            );
        }
        else
        {
            registerButton.interactable = true;
            registerBText.tmp.text = string.Format(
            DataTableManager.StringTableList[(int)(Variables.currentLanguage)].Get(999062),
            DataTableManager.StringTableList[(int)(Variables.currentLanguage)].Get(
                ItemData.ItemData.StringId),
            GameInfos.RequiredCountToReveal(ItemData.ItemData.ItemType).ToString()
            );
            registerButton.GetComponent<Image>().color = colorGreen;

            itemIcon.sprite = DataTableManager.ItemTable.Get(itemData.ItemData.Id).IconSprite;
            string enrolledIconPath = "blank";
            registeredIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/General/{enrolledIconPath}");

            itemDesc.text = "???";

            highPrice.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999953), "???");
            lowPrice.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999954), "???");

            otherInfo.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(999929), "???", "???", "???", "???", "???"
                );
        }
    }

    private void AddListeners()
    {
        registerButton.onClick.AddListener(OnClickRegisterButton);
    }

    private void OnClickRegisterButton()
    {
        if (SaveLoadManager.BaseData.isItemRevealed[ItemData.ItemData.Id-ItemDataIndex.minPrimary])
        {            
            MarketControl();
        }
        else
        {
            DoRegister();
        }
    }

    private void MarketControl()
    {
        encloWindow.bulletinBoardMgr.OpenMarketControl(ItemData);
    }

    private void DoRegister()
    {
        if (GameManager.Instance.entireItemDict[ItemData.ItemData.Id].count >=
           GameInfos.RequiredCountToReveal(ItemData.ItemData.ItemType))
        {
            GameManager.Instance.entireItemDict[ItemData.ItemData.Id].count -=
                GameInfos.RequiredCountToReveal(ItemData.ItemData.ItemType);

            SaveLoadManager.BaseData.isItemRevealed[ItemData.ItemData.Id - ItemDataIndex.minPrimary] = true;
            encloWindow.UpdateEnclopediaWindow();
            SetData(ItemData);
            SaveLoadManager.SaveBase();
        }
        else
        {
            encloWindow.bulletinBoardMgr.OpenMessage(BulletinBoardMsgType.LackOfItems);
            return;
        }
    }

    private void UpdateDisPlayedInfo()
    {

    }
}
