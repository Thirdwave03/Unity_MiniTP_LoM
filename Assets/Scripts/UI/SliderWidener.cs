using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SliderWidener : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Slider slider;
    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        slider = GetComponentInChildren<Slider>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        ((IPointerDownHandler)slider).OnPointerDown(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)slider).OnDrag(eventData);
    }
}
