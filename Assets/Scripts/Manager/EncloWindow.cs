using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncloWindow : MonoBehaviour
{
    public UiBulletinBoardManager bulletinBoardMgr;
    public EncloPanel encloPanel;
    public EncloInfo encloInfo;
    public TextMeshProUGUI revealedContentsCount;

    private void Start()
    {
        encloPanel.AddListeners(OnClickEncloSlot);
        encloInfo.SetEmpty();
        UpdateEnclopediaWindow();
    }

    private void OnClickEncloSlot()
    {
        int index = encloPanel.SelectedSlotIndex;
        if(index != -1 && encloPanel.slots[index].Data != null)
        {
            encloInfo.SetData(encloPanel.slots[index].Data);
        }
        else
        {
            encloInfo.SetEmpty();
        }
    }

    public void UpdateEnclopediaWindow()
    {
        int revealedCnt = 0;
        foreach(var index in SaveLoadManager.BaseData.isItemRevealed)
        {
            if(index)
            {
                revealedCnt++;
            }
        }

        revealedContentsCount.text = string.Format(
            "{0} / {1}", revealedCnt, 50);
    }
}
