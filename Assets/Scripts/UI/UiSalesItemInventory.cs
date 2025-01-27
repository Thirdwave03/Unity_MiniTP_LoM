using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSalesItemInventory : MonoBehaviour
{
    public UiSalesPanel salesItemPanel;

    public List<UiSalesItemSlot> slots = new List<UiSalesItemSlot>();
    public List<SavedItemData> inventoryItemData = new List<SavedItemData>();

    public UiSalesItemSlot prefabItemSlot;
    public ScrollRect scrollRect;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int activeSlotCount = 0;
    public int maxSlotCnt;

    private void Awake()
    {
        maxSlotCnt = DataTableManager.ItemTable.ItemDictionaryCount;

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

        inventoryItemData = new List<SavedItemData>();
        inventoryItemData.Clear();

        foreach (var data in SaveLoadManager.Data.savedItemList)
        {
            inventoryItemData.Add(data);
        }
        UpdateSlots(inventoryItemData);
    }

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in slots)
        {
            Debug.Log($"actionAdded to {slot.SlotIndex}");
            slot.button.onClick.AddListener(action);
        }
    }


    private void Update()
    {

    }

    public void UpdateSlots(List<SavedItemData> items)
    {
        int indexCount = 0;
        foreach (var item in items)
        {
            if (item.count > 0)
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

    public void OnDrag(PointerEventData eventData)
    {

    }




}
