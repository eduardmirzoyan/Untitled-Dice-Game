using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSlotUI : ItemSlotUI
{
    [Header("Data")]
    [SerializeField] private int partyIndex;
    [SerializeField] private int weaponIndex;
    [SerializeField] private Unit unit;
    [SerializeField] private GameObject itemUIPrefab;

    private void Start() {
        GameEvents.instance.onAddUnitToParty += OnUnitAddToParty;
        isActive = false;
    }

    private void OnDestroy() {
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
                // If there was already a weapon in this slot
                if (itemUI != null)
                {
                    // Remove that weapon
                    Destroy(itemUI.gameObject);
                    itemUI = null;
                }

                // If the new unit has a weapon in this index
                if (unit.weapons[weaponIndex] != null)
                {
                    // Create new weapon in this slot 
                    itemUI = Instantiate(itemUIPrefab, transform).GetComponent<ItemUI>();
                    itemUI.Initialize(unit.weapons[weaponIndex], this);
                }

                // Make slot active
                isActive = true;
            }
            // If unit was removed
            else
            {
                // Delete any weapon in this slot
                if (itemUI != null)
                {
                    // Remove that weapon
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
        if (itemUI != null) {
            // Debugging
            print("Weapon Item " + itemUI.name + " has stored into slot: " + name);

            itemUI.SetParent(gameObject.transform);

            // Equip weapon to unit
            var weapon = (Weapon)itemUI.GetItem();
            unit.EquipWeapon(weapon, weaponIndex);
        }
        else {
            // Debugging
            print("Weapon was removed from slot: " + name);

            // Unequip weapon
            unit.EquipWeapon(null, weaponIndex);
        }

        this.itemUI = itemUI;
    }

    protected override bool PassesRestrictions(ItemUI itemUI)
    {
        return itemUI.GetItem() is Weapon;
    }
}
