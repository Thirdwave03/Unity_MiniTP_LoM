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

    public GameObject blinder;

    public TextMeshProUGUI registerCountIndicator;

    public void Awake()
    {
        blinder.SetActive(true);
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
            registerButton.interactable = false;

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

    private void OnClickEnrollButton()
    {

    }

    private void UpdateDisPlayedInfo()
    {

    }
}
