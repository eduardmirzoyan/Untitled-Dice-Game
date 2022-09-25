using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitHolderUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private Image highlightImage;
    [SerializeField] protected RectTransform unitModelTransform;

    [Header("Data")]
    [SerializeField] protected GameObject unitUIPrefab;
    [SerializeField] protected UnitUI unitUI;
    
    public virtual void Initialize(Unit unit) {
        // Spawn model
        unitUI = Instantiate(unitUIPrefab, unitModelTransform).GetComponent<UnitUI>();
        // Initialize with -1 b/c not equipped
        unitUI.Initialize(unit, this, unitModelTransform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            Highlight(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            Highlight(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            // Make sure that the same unit isn't added to the same slot
            if (newUnitUI == unitUI) return;

            // If a unit already exists here, swap
            if (this.unitUI != null)
            {
                // Debugging
                print("An unit exists in this slot, doing a swap :)");

                // Set the old unit to where the new one was
                this.unitUI.ResetTo(newUnitUI.GetParent());
            }

            // Set any previous slots to what this had
            if (newUnitUI.GetHolder() != null)
            {
                // Add item to slot, where this.itemUI could be null in which case you are un-equipping
                newUnitUI.GetHolder().StoreUnit(this.unitUI);
            }

            // Store new item into this slot
            StoreUnit(newUnitUI);

            // Remove highlight
            Highlight(false);
        }
    }

    protected virtual void Highlight(bool enable) {
        if (highlightImage != null)
        {
            var color = highlightImage.color;
            color.a = enable ? 0.35f : 0f;
            highlightImage.color = color;
        }
    }

    protected virtual void StoreUnit(UnitUI unitUI) {
        if (unitUI != null)
        {
            // Debugging
            print("Unit " + unitUI.name + " has inserted into the slot: " + name);
            unitUI.SetParent(unitModelTransform);
            unitUI.SetHolder(this);
        }
        else
        {
            // Debugging
            print("Unit was removed from slot: " + name);
        }

        this.unitUI = unitUI;
    } 
}
