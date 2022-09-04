using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionEvents : MonoBehaviour
{
    public static SelectionEvents instance;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public event Action<Unit, int> onAddUnitToParty;
    public event Action<List<Unit>> onDisplayUnitOptions;
    public event Action<bool> onPartyFull;

    public event Action<Item> onItemEquip;
    public event Action<ItemUI, ItemSlotUI> onItemInsertIntoSlot;

    public void TriggerOnAddUnitToParty(Unit unit, int index) {
        if (onAddUnitToParty != null) {
            onAddUnitToParty(unit, index);
        }
    }

    public void TriggerOnDisplayUnitOptions(List<Unit> units) {
        if (onDisplayUnitOptions != null) {
            onDisplayUnitOptions(units);
        }
    }

    public void TriggerOnPartyFull(bool state) {
        if (onPartyFull != null) {
            onPartyFull(state);
        }
    }

    public void TriggerOnItemEquip(Item item) {
        if (onItemEquip != null) {
            onItemEquip(item);
        }
    }

    public void TriggerOnItemInsertIntoSlot(ItemUI itemUI, ItemSlotUI itemSlotUI) {
        if (onItemInsertIntoSlot != null) {
            onItemInsertIntoSlot(itemUI, itemSlotUI);
        }
    }
}
