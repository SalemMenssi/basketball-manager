using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector3 originalLocalPointerPosition;
    private Vector3 originalPosition;
    private Canvas canvas;
    private Camera mainCamera;
    private Transform containerParent;

    

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
        originalParent = transform.parent;
        containerParent = originalParent.parent.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject SlotBox = GameObject.Find("stratigie");

        Debug.Log(SlotBox.GetComponentInChildren<DropPlayerSlot>());
        if (SlotBox.GetComponentInChildren<DropPlayerSlot>() != null)
        {
            SlotBox.GetComponentInChildren<DropPlayerSlot>().isOccupied = false;
        }

        originalPosition = rectTransform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, mainCamera, out originalLocalPointerPosition);

        // Set the parent to the container during the drag to reparent the item.
        rectTransform.SetParent(containerParent, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPointerPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, mainCamera, out worldPointerPosition))
        {
            Vector3 offsetToOriginal = worldPointerPosition - originalLocalPointerPosition;
            rectTransform.position = originalPosition + offsetToOriginal;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.SetParent(originalParent, true);
        rectTransform.localPosition = Vector3.zero;
    }

    public void ReturnToOriginalParent()
    {
        rectTransform.SetParent(originalParent, true);
        rectTransform.localPosition = Vector3.zero;
    }

    public void SetNewParent(Transform newParent)
    {
        rectTransform.SetParent(newParent, false);
    }
}
