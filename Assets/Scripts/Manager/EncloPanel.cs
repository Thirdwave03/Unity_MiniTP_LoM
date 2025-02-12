using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EncloPanel : MonoBehaviour, IDragHandler
{
    public EncloWindow encloWindow;

    public List<EncloSlot> slots = new List<EncloSlot>();
    //public List<SavedItemData> inventoryItemdata = new List<SavedItemData>();

    public EncloSlot prefabEncloSlot;
    public ScrollRect scrollRect;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int activeSlotCount = 0;
    public int maxSlotCnt;

    private void Awake()
    {
        maxSlotCnt = DataTableManager.ItemTable.ItemDictionaryCount;

        for(int i = 0; i < maxSlotCnt; ++i)
        {
            var slot = Instantiate(prefabEncloSlot, scrollRect.content);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
            });
            slot.SetEmpty();            
            slots.Add(slot);
        }
        //inventoryItemdata = new List<SavedItemData>();
        //inventoryItemdata.Clear();
        UpdateSlots(GameManager.Instance.entireItemDict.Values.ToList());
    }

    public void AddListeners(UnityAction action)
    {
        foreach(var slot in slots)
        {
            slot.button.onClick.AddListener(action);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void UpdateSlots(List<SavedItemData> items)
    {
        int indexCount = 0;
        foreach (var item in items)
        {
            slots[indexCount++].SetItem(item);
        }
        SelectedSlotIndex = -1;
    }
}
