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
    }

    private void OnClickInventorySlot()
    {
        int index = inventory.SelectedSlotIndex;
        if (index != -1 && inventory.slots[index].Data != null)
        {
            Debug.Log($"Info Update Successful. index : {index}");
            itemInfo.SetData(inventory.slots[index].Data);
        }
        else
        {
            Debug.Log($"Info Update failed. index : {index}");
            itemInfo.SetEmpty();
        }
    }
}
