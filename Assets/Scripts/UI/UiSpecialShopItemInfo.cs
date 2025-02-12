using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSpecialShopItemInfo : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int slotIndex = 0;

    public int itemId;

    public UiSpecialSalesWindow uiSpecialSalesWindow;

    public Image itemIcon;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemOccupancy;
    public TextMeshProUGUI itemCount;
    public TextLocalizer itemName;

    public Slider slider;
    public TextMeshProUGUI salesCountText;
    public TextMeshProUGUI itemAvgCost;

    private int salesCount;

    private void Start()
    {
        uiSpecialSalesWindow = GetComponentInParent<UiSpecialSalesWindow>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        ResetSlot();
    }

    public void SetItem(int id, bool init = true)
    {
        if (init)
        {
            itemId = id;
            itemIcon.sprite = DataTableManager.ItemTable.Get(id).IconSprite;
            itemName.stringId = DataTableManager.ItemTable.Get(id).StringId;
            itemName.OnChangeLanguage(Variables.currentLanguage);
        }
        itemPrice.text = ((int)(GameManager.Instance.entireItemDict[id].price 
            * GameManager.Instance.SpecialSalesAdvantagedPriceMultiplier)).ToString();
        itemOccupancy.text = GameManager.Instance.entireItemDict[id].ItemData.InventoryOccupancy.ToString();
        itemCount.text = GameManager.Instance.entireItemDict[id].count.ToString();
        slider.wholeNumbers = true;
        slider.minValue = slider.value = 0;
        slider.maxValue = GameManager.Instance.entireItemDict[id].count;
        salesCount = 0;
        salesCountText.text = salesCount.ToString();
        itemAvgCost.text = GameManager.Instance.entireItemDict[id].avgCost.ToString();
    }        

    public void OnSliderValueChanged(float value)
    {
        slider.value = Mathf.Clamp(slider.value, 0, slider.maxValue);
        salesCount = (int)(slider.value);
        salesCountText.text = salesCount.ToString();
        uiSpecialSalesWindow.OnSliderValueChangeInSlot(slotIndex, (int)(slider.value));
        
    }

    public void ResetSlot()
    {
        slider.value = 0;
        salesCount = 0;
        salesCountText.text = salesCount.ToString();  
    }
    
    public void OnClickMax()
    {
        slider.value = slider.maxValue;
        salesCount = (int)(slider.maxValue);
        salesCountText.text = salesCount.ToString();
    }

    public void OnClickSell()
    {
        GameManager.Instance.coins += (int)(GameManager.Instance.entireItemDict[itemId].price 
            * GameManager.Instance.SpecialSalesAdvantagedPriceMultiplier) * salesCount;
        GameManager.Instance.entireItemDict[itemId].count -= salesCount;

        GameManager.Instance.entireItemDict[itemId].totalSoldCount += salesCount;
        GameManager.Instance.entireItemDict[itemId].totalSoldAmount +=
            salesCount * (int)(GameManager.Instance.entireItemDict[itemId].price *
            GameManager.Instance.SpecialSalesAdvantagedPriceMultiplier);

        GameManager.Instance.CallSave();
        UpdateDisplayedInfo();
        uiSpecialSalesWindow.salesSceneUi.UpdateSalesSceneDisplay();
    }

    public void UpdateDisplayedInfo()
    {
        SetItem(itemId, false);
    }

    private void OnClickItemSlider()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        uiSpecialSalesWindow.OnClickSpecialItem(slotIndex);
    }


}
