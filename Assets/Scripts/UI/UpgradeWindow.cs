using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    public TitleSceneUiManager uiManager;

    public Button[] buttons;
    public TextMeshProUGUI[] upgradeCounts;
    public TextMeshProUGUI[] upgradeCosts;

    private void Awake()
    {
        AddListeners();
    }

    public void SetContents()
    {
        for (int i = 0; i < (int)UpgradeItems.Count; ++i)
        {
            upgradeCounts[i].text = string.Format(
                "( {0} / {1} )",
                SaveLoadManager.BaseData.upgradeCounts[0].ToString(),
                DataTableManager.UpgradeTable.Get((UpgradeItems)i)
                .MaxLv.ToString()
                );

            upgradeCosts[i].text =
                DataTableManager.UpgradeTable.Get((UpgradeItems)i)
                .UpgradeCost.ToString();
        }
        UpdateButtonInteractables();
    }

    private void UpdateButtonInteractables()
    {
        for (int i = 0; i < (int)UpgradeItems.Count; ++i)
        {
            if (SaveLoadManager.BaseData.diamonds >=
                DataTableManager.UpgradeTable.Get((UpgradeItems)i)
                .UpgradeCost)
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
        }
    }

    private void AddListeners()
    {
        buttons[0].onClick.AddListener(OnClickButton0);
        buttons[1].onClick.AddListener(OnClickButton1);
        buttons[2].onClick.AddListener(OnClickButton2);
        buttons[3].onClick.AddListener(OnClickButton3);
        buttons[4].onClick.AddListener(OnClickButton4);
        buttons[5].onClick.AddListener(OnClickButton5);
        buttons[6].onClick.AddListener(OnClickButton6);
    }

    private void OnClickButton0()
    {
        uiManager.OnClickModeSelect(0);
    }

    private void OnClickButton1()
    {
        uiManager.OnClickModeSelect(1);
    }

    private void OnClickButton2()
    {
        uiManager.OnClickModeSelect(2);
    }

    private void OnClickButton3()
    {
        uiManager.OnClickModeSelect(3);
    }

    private void OnClickButton4()
    {
        uiManager.OnClickModeSelect(4);
    }

    private void OnClickButton5()
    {
        uiManager.OnClickModeSelect(5);
    }

    private void OnClickButton6()
    {
        uiManager.OnClickModeSelect(6);
    }
}
