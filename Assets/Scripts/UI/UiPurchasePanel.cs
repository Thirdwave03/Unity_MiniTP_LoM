using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiPurchasePanel : MonoBehaviour
{
    public PurchaseSceneUiManager purchaseScene;
    public UiPurchaseItemBoard purchaseBoard;
    public UiPurchaseItemInfo itemInfo;

    private void Start()
    {
        purchaseBoard.AddListeners(OnClickInventorySlot);
        itemInfo.SetEmpty();
    }

    private void OnEnable()
    {

    }


    private void OnClickInventorySlot()
    {
        int index = purchaseBoard.SelectedSlotIndex;
        if (index != -1 && purchaseBoard.slots[index].Data != null)
        {
            itemInfo.SetData(purchaseBoard.slots[index].Data);
        }
        else
        {
            itemInfo.SetEmpty();
        }
    }
}
