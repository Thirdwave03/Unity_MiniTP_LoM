using UnityEngine;
using UnityEngine.EventSystems;

public class ForwardPointerDown : MonoBehaviour, IPointerDownHandler
{
    // 이벤트를 전달할 대상 (예: 가려진 Button의 GameObject)
    public GameObject[] targetButtons;

    public void OnPointerDown(PointerEventData eventData)
    {
        // targetButton이 유효한지 확인
        foreach (var targetButton in targetButtons)
        {
            if (targetButton != null)
            {
                // targetButton에 pointer down 이벤트를 전달합니다.
                ExecuteEvents.Execute(targetButton, eventData, ExecuteEvents.pointerDownHandler);

                // 만약 버튼 클릭(Up 이후의 처리)까지 전달하려면
                // ExecuteEvents.Execute(targetButton, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
