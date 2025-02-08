using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiModeSelectWindow : MonoBehaviour
{
    public TitleSceneUiManager uiManager;

    public Button[] buttons;
    public TextMeshProUGUI[] tmps;

    private void Awake()
    {
        AddListeners();
    }

    public void SetContents()
    {
        for(int i = 1; i < (int)GameModes.Count; ++i)
        {
            tmps[i-1].text = SaveLoadManager.BaseData.bestScore[i].ToString(); 
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
        buttons[7].onClick.AddListener(OnClickButton7);
        buttons[8].onClick.AddListener(OnClickButton8);
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

    private void OnClickButton7()
    {
        uiManager.OnClickModeSelect(7);
    }

    private void OnClickButton8()
    {
        uiManager.OnClickModeSelect(8);
    }
}