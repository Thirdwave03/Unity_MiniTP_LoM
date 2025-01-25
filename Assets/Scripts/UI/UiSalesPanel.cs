using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSalesPanel : MonoBehaviour
{
    public UiSalesItemInventory salesInventory;
    public UiSalesItemInfo salesItemInfo;

    private void Start()
    {
        salesInventory.AddListeners(OnClickInventorySlot);
    }

    private void OnClickInventorySlot()
    {
        int index = salesInventory.SelectedSlotIndex;
        if (index != -1 && salesInventory.slots[index].Data != null)
        {
            Debug.Log($"Info Update Successful. index : {index}");
            salesItemInfo.SetData(salesInventory.slots[index].Data);
        }
        else
        {
            Debug.Log($"Info Update failed. index : {index}");
            salesItemInfo.SetEmpty();
        }
    }



}
