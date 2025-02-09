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
        UpdateUpperText();
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
        if (CurrentSlotIndex < 0 || CurrentSlotIndex > 2)
        {
            return;
        }
        specialSalesItems[CurrentSlotIndex].OnClickMax();
    }

    public void OnClickSell()
    {
        if (CurrentSlotIndex < 0 || CurrentSlotIndex > 2)
        {
            return;
        }
        specialSalesItems[CurrentSlotIndex].OnClickSell();
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
                specialSalesItems[i].OnSelectOtherSlot();
            }
        }
    }

    public void OnSliderValueChangeInSlot(int index, int val)
    {
        if(index < 0 || index > 2)
        {
            return;
        }
        
        coinsTMP.text = ((int)(GameManager.Instance.entireItemDict[specialSalesItems[index].itemId].price * 1.2f)
            * val).ToString();
        occupancyTMP.text = (GameManager.Instance.entireItemDict[specialSalesItems[index].itemId].ItemData.InventoryOccupancy
            * val).ToString();
    }

    private void UpdateUpperText()
    {
        upperTextLC.tmp.text = string.Format(DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(999922), GameManager.Instance.specialSalesAdvantageRatio.ToString());
    }






}
