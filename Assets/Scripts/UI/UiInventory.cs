using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
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


    private void UpdateSlots(List<SavedItemData> items)
    {
        //int index = 0;
        //int cnt = 0;
        //foreach(var item in items)
        //{
        //    if(item.count > 0)
        //    {
        //        cnt++;
        //    }
        //}
        //foreach(var slot in slots)
        //{
        //    slot.SetEmpty();
        //}
        //slots.Clear();
        
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
