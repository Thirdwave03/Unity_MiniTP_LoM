using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPurchaseItemSlot : MonoBehaviour
{
    public int SlotIndex { get; set; }
    public SavedSalesItemData Data { get; private set; }

    public TextMeshProUGUI itemPriceText;
    public Image itemIcon;
    public TextMeshProUGUI itemCountText;
    public Button button;
    public Image priceTrendIcon;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetItem(SavedSalesItemData savedSalesItemData)
    {
        gameObject.SetActive(true);
        Data = savedSalesItemData;    
        itemIcon.sprite = DataTableManager.ItemTable.Get(savedSalesItemData.SalesItemData.SalesItemId).IconSprite;
        itemPriceText.text = GameManager.Instance.entireItemDict[savedSalesItemData.SalesItemData.SalesItemId].price.ToString();
        itemCountText.text = savedSalesItemData.stock.ToString();
        string trendImagePath;
        if(SaveLoadManager.BaseData.MerchantRank >= MerchantRanks.SeasonedMerchant)
        {
            if (GameManager.Instance.entireItemDict[Data.SalesItemData.SalesItemId].price ==
                GameManager.Instance.entireItemDict[Data.SalesItemData.SalesItemId].pricePrevDay)
            {
                trendImagePath = "blank";
            }
            else if (GameManager.Instance.entireItemDict[Data.SalesItemData.SalesItemId].price >
                GameManager.Instance.entireItemDict[Data.SalesItemData.SalesItemId].pricePrevDay)
            {
                trendImagePath = "PriceUp";
            }
            else
            {
                trendImagePath = "PriceDown";
            }
        }
        else
        {
            trendImagePath = "blank";
        }
        priceTrendIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/General/{trendImagePath}");
    }

    public void OnClick()
    {
        Debug.Log($"Slot Index: {SlotIndex}");
        if (Data != null)
        {
            Debug.Log($"Item Id: {Data.SalesItemData.Id}");
        }
    }
}
