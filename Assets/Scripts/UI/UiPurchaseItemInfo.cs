using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPurchaseItemInfo : MonoBehaviour
{
    public UiPurchasePanel purchaseItemPanel;
    public SavedSalesItemData ItemData { get; private set; }
    public Image itemIcon;

    public Slider purchaseSlider;
    public TextMeshProUGUI purchaseCountText;
    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemOccupancy;
    public TextLocalizer itemNameLC;
    public TextMeshProUGUI itemAvgCost;

    public TextLocalizer itemAvgCostLC;

    public GameObject blinder;

    public TextMeshProUGUI subtotalPrice;
    public TextMeshProUGUI subtotalOccupancy;

    public Button maxButton;
    public Button purchaseButton;

    public TextLocalizer maxButtonLC;
    public TextLocalizer purchaseButtonLC;

    private int purchaseCount;

    public void Awake()
    {
        blinder.SetActive(true);
    }

    public void SetEmpty()
    {
        ItemData = null;
        itemIcon.sprite = null;
        itemCount.text = string.Empty;
        itemPrice.text = string.Empty;
        itemOccupancy.text = string.Empty;
        itemNameLC.stringId = 0;
        itemNameLC.tmp.text = string.Empty;
        itemAvgCost.text = string.Empty;
        subtotalPrice.text = string.Empty;
        subtotalOccupancy.text = string.Empty;
        purchaseCountText.text = string.Empty;

        maxButton.interactable = false;
        purchaseButton.interactable = false;

        blinder.SetActive(true);
    }

    public void SetData(SavedSalesItemData salesItemData)
    {
        if (salesItemData == null)
        {
            SetEmpty();
            return;
        }
        maxButton.interactable = true;
        purchaseButton.interactable = true;

        ItemData = salesItemData;
        itemIcon.sprite = DataTableManager.ItemTable.Get(salesItemData.SalesItemData.SalesItemId).IconSprite;
        itemIcon.type = Image.Type.Simple;
        itemIcon.preserveAspect = true;

        itemCount.text = ItemData.stock.ToString();
        itemPrice.text = GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price.ToString();
        itemOccupancy.text = DataTableManager.ItemTable.Get(ItemData.SalesItemData.SalesItemId).InventoryOccupancy.ToString();
        itemNameLC.stringId = DataTableManager.ItemTable.Get(ItemData.SalesItemData.SalesItemId).StringId;
        itemNameLC.OnChangeLanguage(Variables.currentLanguage);
        itemAvgCost.text = GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].avgCost.ToString();
        itemAvgCostLC.OnChangeLanguage(Variables.currentLanguage);

        purchaseSlider.wholeNumbers = true;
        purchaseSlider.minValue = 0;
        purchaseSlider.maxValue = ItemData.stock;
        purchaseCount = 0;
        purchaseSlider.value = 0;

        purchaseCountText.text = purchaseCount.ToString();
        subtotalPrice.text = (GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount).ToString();
        subtotalOccupancy.text = (DataTableManager.ItemTable.Get(ItemData.SalesItemData.SalesItemId).InventoryOccupancy * purchaseCount).ToString();

        blinder.SetActive(false);
    }

    public void OnSliderValueChanged()
    {
        purchaseSlider.value = Mathf.Clamp(purchaseSlider.value, 0, purchaseSlider.maxValue);
        purchaseCount = (int)(purchaseSlider.value);
        //purchaseCount = (int)(value * ItemData.stock);

        purchaseCountText.text = purchaseCount.ToString();
        subtotalPrice.text = (GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount).ToString();
        subtotalOccupancy.text = (DataTableManager.ItemTable.Get(ItemData.SalesItemData.SalesItemId).InventoryOccupancy * purchaseCount).ToString();
    }

    public void OnClickMaxButton()
    {
        purchaseSlider.value = purchaseSlider.maxValue;
        OnSliderValueChanged();
    }

    public void OnClickPurchaseButton()
    {
        if (GameManager.Instance.coins >= 
            GameManager.Instance.
            entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount)
        {
            if (GameManager.Instance.inventoryCapacity -
                GameManager.Instance.InventoryOccupancy >=
                GameManager.Instance.
                entireItemDict[ItemData.SalesItemData.SalesItemId].
                ItemData.InventoryOccupancy * purchaseCount)
            { 
                Debug.Log($"Purchase Successful! " +
                    $"{DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(900000 + (ItemData.SalesItemData.SalesItemId))}" +
                    $"({purchaseCount})");
                GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].avgCost =
                    (GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].count *
                    GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].avgCost
                    + GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount)
                    / (GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].count + purchaseCount);
                GameManager.Instance.coins -= GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount;
                GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].count += purchaseCount;

                ItemData.stock -= purchaseCount;
                purchaseSlider.maxValue = ItemData.stock;
                GameManager.Instance.salesItemDict[ItemData.SalesItemData.Id].stock = ItemData.stock;

                UpdateDisplayedInfo();
                GameManager.Instance.CallSave();
                purchaseItemPanel.purchaseBoard.CallUpdateSlots();                    
                purchaseItemPanel.purchaseScene.UpdatePurchaseSceneDisplay();
            }
            else
            {
                purchaseItemPanel.purchaseScene.OpenMessage(PurchaseSceneCenterMsgType.LackOfCapacity);
                Debug.Log($"Purchase Failed.. inventory full {GameManager.Instance.coins - GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount} coins.");

                purchaseCount = 0;
                purchaseSlider.value = purchaseCount;
            }
        }
        else
        {
            purchaseItemPanel.purchaseScene.OpenMessage(PurchaseSceneCenterMsgType.InsufficientCoin);
            Debug.Log($"Purchase Failed.. lacking {GameManager.Instance.coins - GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].price * purchaseCount} coins.");

            purchaseCount = 0;
            purchaseSlider.value = purchaseCount;
        }
    }

    private void UpdateDisplayedInfo()
    {
        purchaseCount = 0;
        purchaseSlider.value = purchaseCount;
        OnSliderValueChanged();

        itemCount.text = ItemData.stock.ToString();
        itemAvgCost.text = GameManager.Instance.entireItemDict[ItemData.SalesItemData.SalesItemId].avgCost.ToString();//
    }
}
