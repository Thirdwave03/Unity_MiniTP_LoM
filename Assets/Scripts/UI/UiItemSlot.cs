using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiItemSlot : MonoBehaviour
{
    public int SlotIndex { get; set; }
    public ItemData Data { get; private set; }

    public TextMeshProUGUI itemPriceText;
    public Image itemIcon;
    public TextMeshProUGUI itemCountText;
    public Button button;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetItem(SavedItemData itemData)
    {
        Data = itemData.ItemData;
        itemIcon.sprite = itemData.ItemData.IconSprite;
        itemPriceText.text = itemData.price.ToString();
        itemCountText.text = itemData.count.ToString();
    }

    public void OnClick()
    {
        Debug.Log($"Slot Index: {SlotIndex}");
        if (Data != null)
        {
            Debug.Log($"Item Id: {Data.Id}");
        }
    }    
}
