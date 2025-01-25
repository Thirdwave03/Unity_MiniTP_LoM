using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSalesItemSlot : MonoBehaviour
{
    public int SlotIndex { get; set; }
    public SavedSalesItemData Data { get; private set; }

    public TextMeshProUGUI itemPriceText;
    public Image itemIcon;
    public Image priceTrendIcon;
    public TextMeshProUGUI itemCountText;
    public Button button;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetItem(SavedSalesItemData savedSalesItemData)
    {
        gameObject.SetActive(true);
        Data = savedSalesItemData;
        itemIcon.sprite = DataTableManager.ItemTable.Get(savedSalesItemData.SalesItemData.SalesItemId).IconSprite;
        string tempfilePath;
        if (GameManager.Instance.entireItemDict[savedSalesItemData.SalesItemData.SalesItemId].price > 
            GameManager.Instance.entireItemDict[savedSalesItemData.SalesItemData.SalesItemId].avgCost)
        {
            tempfilePath = "up";
        }
        else if (GameManager.Instance.entireItemDict[savedSalesItemData.SalesItemData.SalesItemId].price <
            GameManager.Instance.entireItemDict[savedSalesItemData.SalesItemData.SalesItemId].avgCost)
        {
            tempfilePath = "down";
        }
        else
        {
            tempfilePath = "blank";
        }
        priceTrendIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/Genenral/{tempfilePath}");
        itemPriceText.text = GameManager.Instance.entireItemDict[savedSalesItemData.SalesItemData.SalesItemId].price.ToString();
        itemCountText.text = savedSalesItemData.stock.ToString();
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

