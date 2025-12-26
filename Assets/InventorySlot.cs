using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggedItem = dropped.GetComponent<DraggableItem>();

        if (transform.childCount == 0)
        {
            draggedItem.parentAfterDrag = transform;
        }

        DraggableItem currentItem = transform.GetChild(0).GetComponent<DraggableItem>();

        if (currentItem.item.Type != draggedItem.item.Type) return;

        if (currentItem.currentStack >= currentItem.maxStack) return;

    }
}
