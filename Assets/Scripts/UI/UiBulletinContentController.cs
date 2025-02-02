using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBulletinContentController : MonoBehaviour
{
    public RectTransform viewPortRect;
    private LayoutElement layoutElement;


    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        viewPortRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
    }
        
    private void OnEnable()
    {
        layoutElement = GetComponent<LayoutElement>();
        layoutElement.minHeight = viewPortRect.rect.height * 0.125f;

    }

    


}
