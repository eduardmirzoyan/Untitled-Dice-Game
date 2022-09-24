using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image highlightImage;
    [SerializeField] protected ItemUI itemUI;

    [SerializeField] protected bool isActive = true;

    public virtual void OnDrop(PointerEventData eventData)
    {
        // Make sure slot is active
        if (!isActive) return;

        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out ItemUI newItemUI))
        {
            // Remove highlight
            var color = highlightImage.color;
            color.a = 0f;
            highlightImage.color = color;

            // Make sure that the same item isn't added to the same slot
            if (newItemUI == itemUI) return;

            if (PassesRestrictions(newItemUI)) {
                // If an item already exists, swap
                if (this.itemUI != null)
                {
                    // Debugging
                    print("An item exists in this slot, doing a swap :)");

                    // Set the old item to where the new one was
                    this.itemUI.ResetTo(newItemUI.GetParent());
                }

                // Set any previous slots to null;
                if (newItemUI.GetParent().TryGetComponent(out ItemSlotUI itemSlotUI))
                {
                    // Add item to slot, where this.itemUI could be null in which case you are un-equipping
                    itemSlotUI.StoreItem(this.itemUI);
                }

                // Store new item into this slot
                StoreItem(newItemUI);
            }            
        }
    }

    public virtual void StoreItem(ItemUI itemUI)
    {
        if (itemUI != null) {
            // Debugging
            print("Item " + itemUI.name + " has inserted into the slot: " + name);
            itemUI.SetParent(gameObject.transform);
        }
        else {
            // Debugging
            print("Item was removed from slot: " + name);
        }
            
        this.itemUI = itemUI;
    }

    protected virtual bool PassesRestrictions(ItemUI itemUI) {
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Make sure slot is active
        if (!isActive) return;

        if (eventData.pointerDrag != null && itemUI == null && eventData.pointerDrag.TryGetComponent(out ItemUI newItemUI))
        {
            if (highlightImage != null)
            {
                var color = highlightImage.color;
                color.a = 0.35f;
                highlightImage.color = color;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Make sure slot is active
        if (!isActive) return;
        
        if (eventData.pointerDrag != null && itemUI == null && eventData.pointerDrag.TryGetComponent(out ItemUI newItemUI))
        {
            if (highlightImage != null)
            {
                var color = highlightImage.color;
                color.a = 0f;
                highlightImage.color = color;
            }
        }
    }
}
