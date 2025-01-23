using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour, IDragHandler
{
    public List<UiItemSlot> slots = new List<UiItemSlot>();
    public List<SavedItemData> inventoryItemData = new List<SavedItemData>();

    public UiItemSlot prefabItemSlot;
    public ScrollRect scrollRect;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int activeSlotCount = 0;
    public int maxSlotCnt;

    private void Awake()
    {
        maxSlotCnt = DataTableManager.Get<ItemTable>(DataTableIds.Item[0]).ItemDictionaryCount;
        activeSlotCount = 0;

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
    }

    public void AddListeners(UnityAction action)
    {
        foreach(var slot in slots)
        {
            slot.button.onClick.AddListener(action);
        }
    }

    // Debugging*
    private void Update()
    {
        // Debugging*
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var itemId = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxLuxury);
        }
    }

    private void UpdateSlots(List<SavedItemData> items)
    {      
        for(int i = 0; i < maxSlotCnt; ++i)
        {
            if(i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
        SelectedSlotIndex = -1;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }    
}
