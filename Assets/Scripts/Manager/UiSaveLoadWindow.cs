using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSaveLoadWindow : MonoBehaviour
{
    public TextLocalizer[] gameModeHeaders;
    public TextLocalizer[] gameModeBodies;

    public TextLocalizer[] dayHeaders;
    public TextMeshProUGUI[] dayBodies;

    public TextLocalizer[] coinHeaders;
    public TextMeshProUGUI[] coinBodies;

    public TextLocalizer[] dateHeaders;
    public TextMeshProUGUI[] dateBodies;

    public Button[] buttons;

    private void AddListeners()
    {
        buttons[0].onClick.AddListener(OnClickSlot0);
        buttons[1].onClick.AddListener(OnClickSlot1);
        buttons[2].onClick.AddListener(OnClickSlot2);
    }

    private void OnClickSlot0()
    {

    }

    private void OnClickSlot1()
    {

    }

    private void OnClickSlot2()
    {

    }


}
