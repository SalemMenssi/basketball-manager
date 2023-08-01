using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlayerSlot : MonoBehaviour, IDropHandler
{
    public bool isOccupied = false;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        Debug.Log("dropped item: " + draggableItem);

        if (draggableItem != null && !isOccupied)
        {
            if (transform.childCount > 0) // Check if the slot already has a player.
            {
                Transform currentChild = transform.GetChild(0);
                currentChild.GetComponent<DraggableItem>().ReturnToOriginalParent();
                Debug.Log("Returning the previous player to its original parent.");
            }

            draggableItem.SetNewParent(transform); // Set the item as the child of the slot.
            isOccupied = true;
        }
        else
        {
            Debug.Log("Failed to drop the item into the slot.");
        }
    }
}
