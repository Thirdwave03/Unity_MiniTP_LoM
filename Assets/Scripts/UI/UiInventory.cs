using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour, IDragHandler
{
    public UiInventoryPanel inventoryPanel;

    public List<UiItemSlot> slots = new List<UiItemSlot>();
    public List<SavedItemData> inventoryItemData = new List<SavedItemData>();

    public UiItemSlot prefabItemSlot;
    public ScrollRect scrollRect;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int activeSlotCount = 0;
    public int maxSlotCnt;

    private void Awake()
    {
        maxSlotCnt = DataTableManager.ItemTable.ItemDictionaryCount;
        Debug.Log($"Inventory Max Slot cnt: {maxSlotCnt}");
        
        for (int i = 0; i < maxSlotCnt; ++i)
        {
            var slot = Instantiate(prefabItemSlot, scrollRect.content);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
                Debug.Log($"Slot clicked: {slot.SlotIndex}");
            });
            slot.SetEmpty();
            slots.Add(slot);
        }

        inventoryItemData = new List<SavedItemData>();
        inventoryItemData.Clear();

        foreach(var data in SaveLoadManager.GameData.savedItemList)
        {
            inventoryItemData.Add(data);
        }
        UpdateSlots(inventoryItemData);
    }

    public void AddListeners(UnityAction action)
    {
        Debug.Log($"AddListners called to Slots({slots.Count})");
        foreach(var slot in slots)
        {
            slot.button.onClick.AddListener(action);
            //Debug.Log($"Listener Added to slot: {slot.SlotIndex}");
        }
    }

    // Debugging*
    private void Update()
    {
        // Debugging*
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var itemId = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxLuxury);
        }
    }

    private void UpdateSlots(List<SavedItemData> items)
    {
        int indexCount = 0;
        foreach (var item in items)
        {
            if(item.count > 0)
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

    public void DefaultUpdateSlot()
    {
        UpdateSlots(inventoryItemData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }    
}
