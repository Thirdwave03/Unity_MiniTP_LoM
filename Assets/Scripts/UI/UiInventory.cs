using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public List<UiItemSlot> slots = new List<UiItemSlot>();    

    public UiItemSlot prefabItemSlot;
    public ScrollRect scrollRect;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int maxSlotCnt;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        maxSlotCnt = DataTableManager.Get<ItemTable>(DataTableIds.Item[0]).ItemDictionaryCount;
        
        
        
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
}
