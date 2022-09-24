using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSlotUI : ItemSlotUI
{
    [Header("Data")]
    [SerializeField] private int partyIndex;
    [SerializeField] private int armorIndex;
    [SerializeField] private Unit unit;
    [SerializeField] private GameObject itemUIPrefab;

    private void Start()
    {
        GameEvents.instance.onAddUnitToParty += OnUnitAddToParty;
        isActive = false;
    }

    private void OnDestroy()
    {
        GameEvents.instance.onAddUnitToParty -= OnUnitAddToParty;
    }

    private void OnUnitAddToParty(Unit unit, int index)
    {
        // If unit was added to this slot's index
        if (partyIndex == index)
        {
            // If a new unit was added to this slot, replace item with new unit's item
            if (unit != null)
            {
                // If there was already a armor in this slot
                if (itemUI != null)
                {
                    // Remove that armor
                    Destroy(itemUI.gameObject);
                    itemUI = null;
                }

                // If the new unit has a armor in this index
                if (unit.armors[armorIndex] != null)
                {
                    // Create new armor in this slot 
                    itemUI = Instantiate(itemUIPrefab, transform).GetComponent<ItemUI>();
                    itemUI.Initialize(unit.armors[armorIndex], this);
                }

                // Make slot active
                isActive = true;
            }
            // If unit was removed
            else
            {
                // Delete any armor in this slot
                if (itemUI != null)
                {
                    // Remove that armor
                    Destroy(itemUI.gameObject);
                    itemUI = null;
                }

                // Make slot inactive
                isActive = false;
            }

            // Update unit
            this.unit = unit;
        }
    }

    public override void StoreItem(ItemUI itemUI)
    {
        if (itemUI != null)
        {
            // Debugging
            print("Armor Item " + itemUI.name + " has stored into slot: " + name);

            itemUI.SetParent(gameObject.transform);

            // Equip armor to unit
            var armor = (Armor)itemUI.GetItem();
            unit.EquipArmor(armor, armorIndex);
        }
        else
        {
            // Debugging
            print("Armor was removed from slot: " + name);

            // Unequip armor
            unit.EquipArmor(null, armorIndex);
        }

        this.itemUI = itemUI;
    }

    protected override bool PassesRestrictions(ItemUI itemUI)
    {
        return itemUI.GetItem() is Armor;
    }
}
