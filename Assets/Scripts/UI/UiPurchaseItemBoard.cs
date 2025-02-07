using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiPurchaseItemBoard : MonoBehaviour, IDragHandler
{
    public UiPurchasePanel purchaseItemPanel;

    public List<UiPurchaseItemSlot> slots = new List<UiPurchaseItemSlot>();
    public List<SavedSalesItemData> inventoryItemData = new List<SavedSalesItemData>();

    public UiPurchaseItemSlot prefabItemSlot;
    public ScrollRect scrollRect;

    public int minIndex;
    public int maxIndex;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int activeSlotCount = 0;
    public int maxSlotCnt;

    private void Awake()
    {
        maxSlotCnt = DataTableManager.SalesItemTable.SalesItemDictionaryCount;

        for (int i = 0; i < maxSlotCnt; ++i)
        {
            var slot = Instantiate(prefabItemSlot, scrollRect.content);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
            });
            slot.SetEmpty();
            slots.Add(slot);
        }

        inventoryItemData = new List<SavedSalesItemData>();
        inventoryItemData.Clear();

        foreach(var data in SaveLoadManager.GameData.savedSalesItemList)
        {
            inventoryItemData.Add(data);
        }
        UpdateSlots(inventoryItemData);
    }

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in slots)
        {
            slot.button.onClick.AddListener(action);
        }
    }


    private void Update()
    {

    }

    public void AllignIndexWithShopType(int minIdx, int maxIdx)
    {
        minIndex = minIdx;
        maxIndex = maxIdx;
    }

    public void UpdateSlots(List<SavedSalesItemData> items)
    {
        int indexCount = 0;
        foreach (var item in items)
        {
            if (item.isOnSale && item.stock > 0 && item.SalesItemData.Id >= minIndex && item.SalesItemData.Id <= maxIndex)
            {
                slots[indexCount++].SetItem(item);
            }
            else
            {
                slots[indexCount++].SetEmpty();
            }
        }
        SelectedSlotIndex = -1;
    }

    public void CallUpdateSlots()
    {
        inventoryItemData.Clear();
        foreach (var data in SaveLoadManager.GameData.savedSalesItemList)
        {
            inventoryItemData.Add(data);
        }
        UpdateSlots(inventoryItemData);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
