using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventoryPanel : MonoBehaviour
{
    public UiInventory inventory;
    public UiItemInfo itemInfo;

    private void Start()
    {
        inventory.AddListeners(OnClickInventorySlot);
        GameManager.Instance.onSleepEvent.AddListener(inventory.DefaultUpdateSlot);
    }

    private void OnClickInventorySlot()
    {
        int index = inventory.SelectedSlotIndex;
        if (index != -1 && inventory.slots[index].Data != null)
        {
            itemInfo.SetData(inventory.slots[index].Data);
        }
        else
        {
            itemInfo.SetEmpty();
        }
    }
}
