using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSaveLoadWindow : MonoBehaviour
{
    public TitleSceneUiManager uiManager;

    public TextLocalizer[] gameModeBodies;
    public TextMeshProUGUI[] dayBodies;
    public TextMeshProUGUI[] coinBodies;
    public TextMeshProUGUI[] dateBodies;

    public GameObject[] blinders;

    public GameObject[] deleteBAreas;
    public Button[] deleteButtons;

    public Button[] buttons;
    public Button closeButton;

    private void Awake()
    {
        AddListeners();
    }

    private void Start()
    {

    }

    private void OnEnable()
    {

    }

    private void AddListeners()
    {
        buttons[0].onClick.AddListener(OnClickSlot0);
        buttons[1].onClick.AddListener(OnClickSlot1);
        buttons[2].onClick.AddListener(OnClickSlot2);
        closeButton.onClick.AddListener(OnClickClose);
        deleteButtons[0].onClick.AddListener(OnClickDeleteSlot0);
        deleteButtons[1].onClick.AddListener(OnClickDeleteSlot1);
        deleteButtons[2].onClick.AddListener(OnClickDeleteSlot2);
    }

    private void OnClickSlot0()
    {
        uiManager.OnClickSaveLoadSlot(1);
    }

    private void OnClickSlot1()
    {
        uiManager.OnClickSaveLoadSlot(2);
    }

    private void OnClickSlot2()
    {
        uiManager.OnClickSaveLoadSlot(3);
    }
    private void OnClickDeleteSlot0()
    {
        uiManager.OnClickDeleteSlot(1);
    }

    private void OnClickDeleteSlot1()
    {
        uiManager.OnClickDeleteSlot(2);
    }

    private void OnClickDeleteSlot2()
    {
        uiManager.OnClickDeleteSlot(3);
    }


    private void OnClickClose()
    {
        uiManager.OnClickSaveLoadWindowClose();
    }

    public void SetSaveLoadSlots()
    {
        for (int i = 0; i < GameInfos.maxSaveSlot; ++i)
        {
            if(SaveLoadManager.BaseData.dateTimes[i] == default)
            {
                blinders[i].SetActive(true);
                deleteBAreas[i].SetActive(false);
            }
            else
            {
                blinders[i].SetActive(false);
                deleteBAreas[i].SetActive(true);
            }
            //if (SaveLoadManager.BaseData.gameModes[i] == GameModes.Default)
            //{
            //    gameModeBodies[i].tmp.text =
            //        DataTableManager.StringTableList[(int)Variables.currentLanguage]
            //        .Get(999034);
            //}
            gameModeBodies[i].tmp.text = DataTableManager.StringTableList[(int)Variables.currentLanguage]
                .Get(DataTableManager.GameModeTable.Get(SaveLoadManager.BaseData.gameModes[i]).StringId);

            dayBodies[i].text = SaveLoadManager.BaseData.days[i].ToString();
            coinBodies[i].text = SaveLoadManager.BaseData.coins[i].ToString();
            dateBodies[i].text = SaveLoadManager.BaseData.dateTimes[i].
                ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
