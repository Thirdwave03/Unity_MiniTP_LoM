using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketControlWindow : MonoBehaviour
{
    public UiBulletinBoardManager bulletinBoardMgr;

    public SavedItemData ItemData { get; set; }

    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemOccupancy;

    public TextMeshProUGUI upperText;

    public TextMeshProUGUI totalCost;

    public Toggle[] upperToggles;
    public Toggle[] lowerToggles;

    public Button controlButton;
    public Button closeButton;

    private bool isRaise;
    private int controlDays;
    private int controlPrice;

    private void Start()
    {
        AddListeners();
    }

    public void ResetToggles()
    {
        ResetUpperToggles();
        ResetLowerToggles();
    }

    private void ResetUpperToggles()
    {
        foreach (Toggle toggle in upperToggles)
        {
            toggle.isOn = false;
        }
    }

    private void ResetLowerToggles()
    {
        foreach (Toggle toggle in lowerToggles)
        {
            toggle.isOn = false;
            toggle.interactable = false;
        }
    }

    private void EnableLowerToggles()
    {
        foreach (Toggle toggle in lowerToggles)
        {
            toggle.isOn = false;
            toggle.interactable = true;
        }
    }

    public void SetData(SavedItemData itemData)
    {
        if(itemData == null)
        {
            Debug.LogWarning("MarketControl ItemData Null");
            return;
        }

        ItemData = itemData;

        itemIcon.sprite = DataTableManager.ItemTable.Get(itemData.ItemData.Id).IconSprite;
        itemName.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(ItemData.ItemData.StringId);
        itemPrice.text = GameManager.Instance.entireItemDict[ItemData.ItemData.Id]
            .price.ToString();
        itemOccupancy.text = ItemData.ItemData.InventoryOccupancy.ToString();
        upperText.text = string.Format(
            DataTableManager.StringTableList[(int)Variables.currentLanguage].Get(999931),
            DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(ItemData.ItemData.StringId));
        totalCost.text = "0";
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(OnClickCloseButton);
        controlButton.onClick.AddListener(OnClickControlButton);
        foreach (var toggle in upperToggles)
        {
            toggle.onValueChanged.AddListener(OnUpperToggleSelect);
        }
        lowerToggles[0].onValueChanged.AddListener(OnLowerToggle1Select);
        lowerToggles[1].onValueChanged.AddListener(OnLowerToggle2Select);
        lowerToggles[2].onValueChanged.AddListener(OnLowerToggle3Select);
        lowerToggles[3].onValueChanged.AddListener(OnLowerToggle4Select);
        lowerToggles[4].onValueChanged.AddListener(OnLowerToggle5Select);
    }

    private void OnClickCloseButton()
    {
        bulletinBoardMgr.CloseMarketControl();
    }
    
    private void OnUpperToggleSelect(bool val)
    {
        if (!upperToggles[0].isOn && !upperToggles[1].isOn)
        {
            ResetLowerToggles();
        }
        else
        {
            if(upperToggles[0].isOn)
            {
                isRaise = true;
            }
            else
            {
                isRaise = false;
            }
            EnableLowerToggles();
        }     
    }

    private void OnLowerToggle1Select(bool val)
    {
        if (val)
        {
            controlDays = 1;
            controlButton.interactable = true;
        }
        if (!IfAnyLowerToggleOn())
        {
            controlDays = 0;
            controlButton.interactable = false;
        }
        OnLowerToggleSelected();
    }

    private void OnLowerToggle2Select(bool val)
    {
        if (val)
        {
            controlDays = 2;
            controlButton.interactable = true;
        }
        if(!IfAnyLowerToggleOn())
        {
            controlDays = 0;
            controlButton.interactable = false;
        }
        OnLowerToggleSelected();
    }

    private void OnLowerToggle3Select(bool val)
    {
        if (val)
        {
            controlDays = 3;
            controlButton.interactable = true;
        }
        if (!IfAnyLowerToggleOn())
        {
            controlDays = 0;
            controlButton.interactable = false;
        }
        OnLowerToggleSelected();
    }

    private void OnLowerToggle4Select(bool val)
    {
        if (val)
        {
            controlDays = 4;
            controlButton.interactable = true;
        }
        if (!IfAnyLowerToggleOn())
        {
            controlDays = 0;
            controlButton.interactable = false;
        }
        OnLowerToggleSelected();
    }

    private void OnLowerToggle5Select(bool val)
    {
        if (val)
        {
            controlDays = 5;
            controlButton.interactable = true;
        }
        if (!IfAnyLowerToggleOn())
        {
            controlDays = 0;
            controlButton.interactable = false;
        }
        OnLowerToggleSelected();
    }

    private bool IfAnyLowerToggleOn()
    {
        for (int i = 0; i < lowerToggles.Length; ++i)
        {
            if (lowerToggles[i].isOn)
            {
                return true;
            }
        }
        return false;
    }

    private void OnLowerToggleSelected()
    {
        int multiplier = 1;
        if(isRaise)
        {
            multiplier = 3;
        }
        controlPrice = GameInfos.RequiredCoinToControl(ItemData.ItemData.ItemType) *
            controlDays * multiplier;

        totalCost.text = controlPrice.ToString();
    }

    private void OnClickControlButton()
    {    

        if(GameManager.Instance.coins >= controlPrice)
        {
            if (controlPrice == 0)
            {
                // Do nothing
            }
            else
            {
                GameManager.Instance.coins -= controlPrice;
                ControlPrice();
            }
        }
        else
        {
            bulletinBoardMgr.OpenMessage(BulletinBoardMsgType.InsufficientCoin);
        }
    }

    private void ControlPrice()
    {
        GameManager.Instance.BulletinBoardUpdateOnMarketControl(ItemData.ItemData.Id, isRaise, controlDays);
        bulletinBoardMgr.OpenMessage(BulletinBoardMsgType.ControlSuccessful);
        ResetToggles();
        bulletinBoardMgr.uiSceneMgr.UpdateSceneDisplay();
        //bulletinBoardMgr.ResetContents();
        GameManager.Instance.CallSave();
    }
}
