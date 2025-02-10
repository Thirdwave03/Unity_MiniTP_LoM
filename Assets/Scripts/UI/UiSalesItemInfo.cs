using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSalesItemInfo : MonoBehaviour
{
    public UiSalesPanel salesItemPanel;
    public SavedItemData ItemData { get; private set; }
    public Image itemIcon;

    public Slider salesSlider;
    public TextMeshProUGUI salesCountText;
    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemOccupancy;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemAvgCost;

    public GameObject blinder;

    public TextMeshProUGUI subtotalPrice;
    public TextMeshProUGUI subtotalOccupancy;

    public Button maxButton;
    public Button sellButton;

    private int sellCount;

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
        itemName.text = string.Empty;
        itemAvgCost.text = string.Empty;
        subtotalPrice.text = string.Empty;
        subtotalOccupancy.text = string.Empty;
        salesCountText.text = string.Empty;

        maxButton.interactable = false;
        sellButton.interactable = false;

        blinder.SetActive(true);
    }

    public void SetData(SavedItemData salesItemData)
    {
        if (salesItemData == null)
        {
            SetEmpty();
            return;
        }
        maxButton.interactable = true;
        sellButton.interactable = true;

        ItemData = salesItemData;
        itemIcon.sprite = DataTableManager.ItemTable.Get(salesItemData.ItemData.Id).IconSprite;
        itemIcon.type = Image.Type.Simple;
        itemIcon.preserveAspect = true;

        itemCount.text = ItemData.count.ToString();
        itemPrice.text = GameManager.Instance.entireItemDict[ItemData.ItemData.Id].price.ToString();
        itemOccupancy.text = DataTableManager.ItemTable.Get(ItemData.ItemData.Id).InventoryOccupancy.ToString();
        itemName.text = DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(ItemData.ItemData.StringId);
        itemAvgCost.text = GameManager.Instance.entireItemDict[ItemData.ItemData.Id].avgCost.ToString();

        salesSlider.wholeNumbers = true;
        salesSlider.minValue = 0;
        salesSlider.maxValue = ItemData.count;
        sellCount = 0;
        salesSlider.value = 0;

        salesCountText.text = sellCount.ToString();
        subtotalPrice.text = (GameManager.Instance.entireItemDict[ItemData.ItemData.Id].price * sellCount).ToString();
        subtotalOccupancy.text = (DataTableManager.ItemTable.Get(ItemData.ItemData.Id).InventoryOccupancy * sellCount).ToString();

        blinder.SetActive(false);
    }

    public void OnSliderValueChanged()
    {
        salesSlider.value = Mathf.Clamp(salesSlider.value, 0, salesSlider.maxValue);
        sellCount = (int)(salesSlider.value);
        //purchaseCount = (int)(value * ItemData.stock);

        salesCountText.text = sellCount.ToString();
        subtotalPrice.text = (GameManager.Instance.entireItemDict[ItemData.ItemData.Id].price * sellCount).ToString();
        subtotalOccupancy.text = (DataTableManager.ItemTable.Get(ItemData.ItemData.Id).InventoryOccupancy * sellCount).ToString();
    }

    public void OnClickMaxButton()
    {
        salesSlider.value = salesSlider.maxValue;
        OnSliderValueChanged();
    }

    public void OnClickSellButton()
    {
        if (ItemData == null)
            return;
        Debug.Log($"Sell Successful! {DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(ItemData.ItemData.StringId)}({sellCount})");
        GameManager.Instance.coins += GameManager.Instance.entireItemDict[ItemData.ItemData.Id].price * sellCount;
        GameManager.Instance.entireItemDict[ItemData.ItemData.Id].count -= sellCount;
        salesSlider.maxValue = ItemData.count;

        // Blocking multiple Luxury items.
        if (ItemData.ItemData.Id >= ItemDataIndex.minLuxury &&
            ItemData.ItemData.Id <= ItemDataIndex.maxLuxury)
        {
            
        }

            UpdateDisplayedInfo();
        GameManager.Instance.CallSave();
        if (GameManager.Instance.entireItemDict[ItemData.ItemData.Id].count == 0)
        {
            GameManager.Instance.entireItemDict[ItemData.ItemData.Id].avgCost = 0;
            SetEmpty();
        }
        salesItemPanel.salesInventory.UpdateSlots(salesItemPanel.salesInventory.inventoryItemData);
        salesItemPanel.salesSceneUi.UpdateSalesSceneDisplay();
    }

    private void UpdateDisplayedInfo()
    {
        sellCount = 0;
        salesSlider.value = sellCount;
        OnSliderValueChanged();

        itemCount.text = ItemData.count.ToString();
        itemAvgCost.text = GameManager.Instance.entireItemDict[ItemData.ItemData.Id].avgCost.ToString();
    }


}
