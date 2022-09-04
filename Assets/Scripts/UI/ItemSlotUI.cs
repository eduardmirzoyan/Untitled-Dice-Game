using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isEquipSlot;
    [SerializeField] private ItemUI itemUI;

    // PartyMember, blah blah

    // TODO finish the class

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out ItemUI itemUI)) {
            if (this.itemUI == null) {
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

    public void RemoveItem() {
        itemUI = null;
    }
}
