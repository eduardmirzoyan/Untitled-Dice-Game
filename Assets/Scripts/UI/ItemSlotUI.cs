using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image highlightImage;
    [SerializeField] private ItemUI itemUI;
    [SerializeField] private bool isWeaponSlot;
    [SerializeField] private bool isArmorSlot;

    // PartyMember, blah blah

    // TODO finish the class

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out ItemUI itemUI)) {
            // Remove highlight
            var color = highlightImage.color;
            color.a = 0f;
            highlightImage.color = color;

            if (this.itemUI == null) {
                // Check to see if it's a weapon slot
                if (isWeaponSlot && !(itemUI.GetItem() is Weapon)) return;
                // Check to see if it's an armor slot
                if (isArmorSlot && !(itemUI.GetItem() is Armor)) return;

                // Cache info
                this.itemUI = itemUI;
                itemUI.SetParent(gameObject.transform);

                print("Item " + itemUI.name + " is requesting to join this slot");
                // Trigger requestInsertItemIntoItemSlot
                SelectionEvents.instance.TriggerOnItemInsertIntoSlot(itemUI, this);

                // Check if item was previously in a different slot
                var itemSlot = itemUI.GetItemSlotUI();
                if (itemSlot != null) {
                    // Remove from that slot
                    SelectionEvents.instance.TriggerOnItemInsertIntoSlot(null, itemSlot);
                    itemSlot.RemoveItem();
                }

                itemUI.SetItemSlot(this);
            }
            else {
                // TODO: Swapping
                
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && itemUI == null && eventData.pointerDrag.TryGetComponent(out ItemUI newItemUI))
        {
            if (highlightImage != null) {
                var color = highlightImage.color;
                color.a = 0.35f;
                highlightImage.color = color;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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

    public void RemoveItem() {
        itemUI = null;
    }
}
