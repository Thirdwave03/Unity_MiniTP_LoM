using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    public UiInventoryPanel inventoryPanel;
    public SavedItemData ItemData {  get; private set; }
    public Image itemIcon;

    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemOccupancy;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemAvgCost;
    public TextMeshProUGUI itemDescription;


    public void SetEmpty()
    {
        ItemData = null;
        itemIcon.sprite = null;
        itemCount.text = string.Empty;
        itemPrice.text = string.Empty;
        itemOccupancy.text = string.Empty;
        itemName.text = string.Empty;
        itemAvgCost.text = string.Empty;
        itemDescription.text = string.Empty;
    }

    public void SetData(SavedItemData itemData)
    {
        if(itemData == null)
        {
            SetEmpty();
            return;
        }

        ItemData = itemData;
        itemIcon.sprite = ItemData.ItemData.IconSprite;
        itemIcon.type = Image.Type.Simple;
        itemIcon.preserveAspect = true;

        itemCount.text = ItemData.count.ToString();
        itemPrice.text = ItemData.price.ToString();
        itemOccupancy.text = ItemData.ItemData.InventoryOccupancy.ToString();
        itemName.text = ItemData.ItemData.ItemName;
        itemAvgCost.text = ItemData.avgCost.ToString();
        itemDescription.text = ItemData.ItemData.ItemDescription;
    }
}
