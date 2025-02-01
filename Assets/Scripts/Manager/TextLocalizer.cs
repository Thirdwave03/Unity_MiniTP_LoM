using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizer : MonoBehaviour
{
    public int stringId = 999999;
    public List<string> formatContents;

    public TextMeshProUGUI tmp;

    public UnityAction customizedFormat;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if(Application.isPlaying)
        {
            OnChangeLanguage(Variables.currentLanguage);
        }
    }

    public void OnChangeLanguage(Languages language)
    {
        UpdateFormatContents();
        Debug.Log($"{(int)language}, {language}");
        var stringTable = DataTableManager.StringTableList[(int)language];

        if (formatContents != null && formatContents.Count > 0)
        {
            tmp.text = string.Format(stringTable.Get(stringId), formatContents.ToArray());
        }
        else
        {
            tmp.text = stringTable.Get(stringId);
        }
    }

    public void UpdateFormatContents()
    {
        customizedFormat?.Invoke();        
    }
}
