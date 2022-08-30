using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemUI itemUI;

    // TODO finish the class

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out ItemUI itemUI) && this.itemUI == null) {
            itemUI.SetParent(gameObject.transform);
            this.itemUI = itemUI;
        }
    }
}
