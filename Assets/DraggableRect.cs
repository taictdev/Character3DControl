using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableRect : MonoBehaviour, IDragHandler, IScrollHandler
{
    [SerializeField] private RectTransform rectTransform;

    public void OnDrag(PointerEventData eventData)
    {
        MoveToPointer(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        ScaleWithScroll(eventData);
    }

    private void MoveToPointer(PointerEventData eventData)
    {
        Vector2 newPosition = rectTransform.anchoredPosition + eventData.delta;
        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);
        rectTransform.anchoredPosition = newPosition;
    }

    private void ScaleWithScroll(PointerEventData eventData)
    {
        Vector3 newPosition = rectTransform.localScale;
        newPosition.x += eventData.scrollDelta.y;
        newPosition.y = newPosition.x;
        rectTransform.localScale = newPosition;
    }
}