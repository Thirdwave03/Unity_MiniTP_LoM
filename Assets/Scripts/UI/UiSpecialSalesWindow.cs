using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSpecialSalesWindow : MonoBehaviour
{
    public SalesSceneUiManager salesSceneUi;
    public int CurrentSlotIndex {  get; private set; }
    public List<UiSpecialShopItemInfo> specialSalesItems;

    private int[] slidervals = new int[3];

    public TextMeshProUGUI coinsTMP;
    public TextMeshProUGUI occupancyTMP;

    public Button maxB;
    public Button sellB;

    public TextLocalizer upperTextLC;

    private void Start()
    {
        CurrentSlotIndex = -1;
        SetSpecialSalesItem();
        AddListeners();
        AddFormatContents();
        UpdateUpperText();
    }

    private void OnEnable()
    {        

    }

    private void AddListeners()
    {
        maxB.onClick.AddListener(OnClickMax);
        sellB.onClick.AddListener(OnClickSell);
    }

    private void SetSpecialSalesItem()
    {
        for(int i = 0; i < specialSalesItems.Count; ++i)
        {
            specialSalesItems[i].SetItem(GameManager.Instance.specialPriceItemIndexes[i]);
            Debug.Log($"ItemId Set to index{i}: {GameManager.Instance.specialPriceItemIndexes[i]}");
        }
    }

    public void OnClickMax()
    {
        //if (CurrentSlotIndex < 0 || CurrentSlotIndex > 2)
        //{
        //    return;
        //}
        foreach (var slot in specialSalesItems)
        {
            slot.OnClickMax();
        }
        UpdateSumValue();
    }

    public void OnClickSell()
    {
        //if (CurrentSlotIndex < 0 || CurrentSlotIndex > 2)
        //{
        //    return;
        //}
        foreach (var slot in specialSalesItems)
        {
            slot.OnClickSell();
        }
        UpdateSumValue();
        //specialSalesItems[CurrentSlotIndex].OnClickSell();
    }

    public void OnClickSpecialItem(int index)
    {
        CurrentSlotIndex = index;
        coinsTMP.text = "0";
        occupancyTMP.text = "0";
        for (int i = 0; i < specialSalesItems.Count; ++i)
        {
            if(i != index)
            {
                specialSalesItems[i].ResetSlot();
            }
        }
    }

    public void OnSliderValueChangeInSlot(int index, int val)
    {
        if(index < 0 || index > 2)
        {
            return;
        }

        slidervals[index] = val;

        //coinsTMP.text = ((int)(GameManager.Instance.entireItemDict[specialSalesItems[index].itemId].price * 1.2f)
        //    * val).ToString();
        //occupancyTMP.text = (GameManager.Instance.entireItemDict[specialSalesItems[index].itemId].ItemData.InventoryOccupancy
        //    * val).ToString();
        UpdateSumValue();
    }

    private void UpdateSumValue()
    {
        int sum = 0;
        int occupancy = 0;
        for (int i = 0; i < specialSalesItems.Count; ++i)
        {
            sum += (int)(GameManager.Instance.entireItemDict[specialSalesItems[i].itemId].price *
                    slidervals[i] * 1.2f);
            occupancy += GameManager.Instance.entireItemDict[specialSalesItems[i].itemId].ItemData.InventoryOccupancy
            * slidervals[i];
        }
        coinsTMP.text = sum.ToString();
        occupancyTMP.text = occupancy.ToString();
    }

    private void UpdateUpperText()
    {
        upperTextLC.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999922), GameManager.Instance.SpecialSalesAdvantageRatio.ToString());
    }

    private void AddFormatContents()
    {
        upperTextLC.formatContents.Add(GameManager.Instance.SpecialSalesAdvantageRatio.ToString());
    }




}
