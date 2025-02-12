using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncloSlot : MonoBehaviour
{
    public int SlotIndex { get; set; }
    public SavedItemData Data { get; set; }

    public Image itemIcon;
    public Image isEnrolledIcon;
    public Button button;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetItem(SavedItemData savedItemData)
    {
        gameObject.SetActive (true);
        Data = savedItemData;
        itemIcon.sprite = DataTableManager.ItemTable.Get(savedItemData.ItemData.Id).IconSprite;


        if (SaveLoadManager.BaseData.isItemRevealed[Data.ItemData.Id - ItemDataIndex.minPrimary])
        {
            isEnrolledIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/Books/technical book_1");
        }
        else
        {
            string enrolledIconPath = "blank";
            isEnrolledIcon.sprite = Resources.Load<Sprite>($"Sprites/Icon/itemimg/General/{enrolledIconPath}");
        }
    }

    public void OnClick()
    {
        Debug.Log($"Slot Index: {SlotIndex}");
    }



}
