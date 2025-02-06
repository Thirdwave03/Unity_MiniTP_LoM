using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBulletinContentController : MonoBehaviour
{
    public RectTransform viewPortRect;
    private LayoutElement layoutElement;
    private TextLocalizer textLocalizer;
    public Image displayOnNew;

    public int itemId;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        viewPortRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        textLocalizer = GetComponentInChildren<TextLocalizer>();
    }

    private void OnEnable()
    {
        layoutElement = GetComponent<LayoutElement>();
        layoutElement.minHeight = viewPortRect.rect.height * 0.125f;
    }

    public void ResetSize()
    {
        viewPortRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        layoutElement.minHeight = viewPortRect.rect.height * 0.125f;
    }

    public void SetContentWithId(int itemIdInput)
    {
        itemId = itemIdInput;
        var savedItemData = GameManager.Instance.entireItemDict[itemId];
        if (savedItemData.ItemData.IsKoreanWithSuffix == 1)
        {
            textLocalizer.stringId = savedItemData.bulletinBoardId; 
        }
        else
        {
            textLocalizer.stringId = savedItemData.bulletinBoardId + 500;
        }
        textLocalizer.formatContents.Add(
            DataTableManager.StringTableList[(int)Variables.currentLanguage]
            .Get(savedItemData.ItemData.StringId));
        textLocalizer.OnChangeLanguage(Variables.currentLanguage);

        if(savedItemData.isOnBoardRecently)
        {
            displayOnNew.sprite = Resources.Load<Sprite>("Sprites/Icon/itemimg/General/ExclamationMark");
        }
        else
        {
            displayOnNew.sprite = Resources.Load<Sprite>("Sprites/Icon/itemimg/General/blank");
        }
        displayOnNew.preserveAspect = true;
        ResetSize();
    }
}
