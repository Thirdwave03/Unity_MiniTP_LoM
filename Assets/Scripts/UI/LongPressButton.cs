using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPointerDown = false;
    private float pointerDownTimer = 0f;

    public float requiredHoldTime = 1f;
    public UnityEvent onLongPress;
    public UnityEvent onButtonUp;
    public UnityEvent onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;

        if (pointerDownTimer >= requiredHoldTime)
        {
            OnButtonUpAfterLongPress();
        }
        else
        {

        }
        pointerDownTimer = 0f;
    }

    private void Update()
    {
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;
        }
        if (pointerDownTimer >= requiredHoldTime)
        {
            OnLongPress();
        }
    }

    private void OnLongPress()
    {
        onLongPress?.Invoke();
    }

    private void OnClick()
    {
        onClick?.Invoke();
    }

    private void OnButtonUpAfterLongPress()
    {
        onButtonUp?.Invoke();
    }
}
