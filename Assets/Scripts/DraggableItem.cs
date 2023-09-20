using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Camera mainCamera;
    private Transform containerParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        originalParent = transform.parent;
        containerParent = originalParent.parent.parent.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        rectTransform.SetParent(containerParent, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPointerPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, mainCamera, out worldPointerPosition))
        {
            rectTransform.position = worldPointerPosition;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        bool attached = false;
        Transform newPositionMarker = null;

        // Check if the draggable item is over a player position using bounds
        foreach (Transform positionMarker in containerParent)
        {
            if (positionMarker.CompareTag("PlayerPosition"))
            {
                Collider2D positionCollider = positionMarker.GetComponent<Collider2D>();
                Bounds positionBounds = positionCollider.bounds;

                if (positionBounds.Contains(rectTransform.position))
                {
                    attached = true;
                    newPositionMarker = positionMarker;
                    break; // Exit the loop once a position is found
                }
            }
        }

        if (attached)
        {
            // Check if the new position marker already has a child
            if (newPositionMarker.childCount > 0)
            {
                // Store the old child and its original parent
                Transform oldChild = newPositionMarker.GetChild(0);
                Transform oldChildOriginalParent = oldChild.GetComponent<DraggableItem>().originalParent;

                // Set the new child (current draggable item) as the child of the position marker
                rectTransform.SetParent(newPositionMarker, true);
                rectTransform.localPosition = Vector3.zero;

                // Return the old child to its original parent
                oldChild.SetParent(oldChildOriginalParent, true);
                oldChild.localPosition = Vector3.zero;
            }
            else
            {
                // Attach the current draggable item to the position marker
                rectTransform.SetParent(newPositionMarker, true);
                rectTransform.localPosition = Vector3.zero;
            }
        }
        else
        {
            ReturnToOriginalParent();
        }
    }

    public void ReturnToOriginalParent()
    {
        rectTransform.SetParent(originalParent, true);
        rectTransform.localPosition = Vector3.zero;
    }
   
}
