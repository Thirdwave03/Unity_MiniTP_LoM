using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSalesItemSlot : MonoBehaviour
{
    public int SlotIndex { get; set; }
    public SavedItemData Data { get; private set; }

    public TextMeshProUGUI itemPriceText;
    public Image itemIcon;
    public Image priceTrendIcon;
    public TextMeshProUGUI itemCountText;
    public Button button;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetItem(SavedItemData savedItemData)
    {
        gameObject.SetActive(true);
        Data = savedItemData;
        itemIcon.sprite = DataTableManager.ItemTable.Get(savedItemData.ItemData.Id).IconSprite;
        string tempfilePath;
        if (GameManager.Instance.entireItemDict[savedItemData.ItemData.Id].price > 
            GameManager.Instance.entireItemDict[savedItemData.ItemData.Id].avgCost)
        {
            tempfilePath = "PriceUp";
        }
        else if (GameManager.Instance.entireItemDict[savedItemData.ItemData.Id].price <
            GameManager.Instance.entireItemDict[savedItemData.ItemData.Id].avgCost)
        {
            tempfilePath = "PriceDown";
        }
        else
        {
            tempfilePath = "blank";
        }
        priceTrendIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/General/{tempfilePath}");
        itemPriceText.text = GameManager.Instance.entireItemDict[savedItemData.ItemData.Id].price.ToString();
        itemCountText.text = savedItemData.count.ToString();
    }

    public void OnClick()
    {
        Debug.Log($"Slot Index: {SlotIndex}");
        if (Data != null)
        {
            Debug.Log($"Item Id: {Data.ItemData.Id}");
        }
    }
}

