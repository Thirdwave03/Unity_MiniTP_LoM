using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizer : MonoBehaviour
{
    public int stringId;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if(Application.isPlaying)
        {
            
        }
    }

    public void OnChangeLanguage(Languages language)
    {
        var stringTable = DataTableManager.StringTableList[(int)language];

        text.text = stringTable.Get(stringId);
    }
}
