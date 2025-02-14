using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBulletinContentController : MonoBehaviour
{
    public RectTransform viewPortRect;
    private LayoutElement layoutElement;
    public TextLocalizer textLocalizer;
    public Image displayOnNew;
    public Image background;

    public int itemId;

    private void Awake()
    {
        ResetSize();
    }

    private void OnEnable()
    {

        ResetSize();
    }

    public void ResetSize()
    {
        layoutElement = GetComponent<LayoutElement>();
        viewPortRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        layoutElement.minHeight = viewPortRect.rect.height * 0.125f;
    }

    public void SetContentWithId(int itemIdInput)
    {
        ResetSize();

        itemId = itemIdInput;
        var savedItemData = GameManager.Instance.entireItemDict[itemId];
                
        if (savedItemData.ItemData.IsKoreanWithSuffix == 1)
        {
            textLocalizer.stringId = savedItemData.bulletinBoardId; 
        }
        else
        {
            textLocalizer.stringId = savedItemData.bulletinBoardId + 500;
            if(textLocalizer.stringId > 999999)
            {
                textLocalizer.stringId -= 500;
            }
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

        if (GameManager.Instance.entireItemDict[itemId].bulletinBoardId == 999960 ||
            GameManager.Instance.entireItemDict[itemId].bulletinBoardId == 999961)
        {
            background.color = new Color(0f, 0.2f, 0.4f);
        }
        ResetSize();
    }
}
