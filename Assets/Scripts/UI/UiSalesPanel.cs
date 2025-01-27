using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSalesPanel : MonoBehaviour
{
    public SalesSceneUiManager salesSceneUi;
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
            Debug.Log($"SalesSlot Info Update Successful. index : {index}");
            salesItemInfo.SetData(salesInventory.slots[index].Data);
        }
        else
        {
            Debug.Log($"Sales Info Update failed. index : {index}");
            salesItemInfo.SetEmpty();
        }
    }



}
