using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Unit : ScriptableObject
{
    public string unitName;
    public Sprite icon;
    public int currentHealth;
    public int maxHealth;
    public Dice dice;
    public List<Weapon> weapons;
    public List<Armor> armors;
    public int speed;
    public GameObject modelPrefab;
    public GameObject maskPrefab;
    public List<Passive> innatePassives;
    public SlimeAI ai;

    public List<Action> GetActions() {
        List<Action> result = new List<Action>();

        // Get actions from weapons
        foreach (var weapon in weapons) {
            if (weapon != null)
                result.AddRange(weapon.actions);
        }
        
        // Get innate actions
        // TODO

        // Get actions from other sources
        // TODO

        return result;
    }

    public List<Passive> GetPassives() {
        List<Passive> result = new List<Passive>();

        // Get innate passives
        result.AddRange(innatePassives);

        // Get actions from armor
        foreach (var armor in armors) {
            if (armor != null)
                result.AddRange(armor.passives);
        }

        // Get passives from equipment

        // Get passives from other sources
        // TODO

        return result;
    }

    public void TakeDamage(int amount) {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public string GetHealthStatus() {
        return currentHealth + "/" + maxHealth;
    }

    public virtual bool IsDead() {
        return currentHealth <= 0;
    }

    public void EquipWeapon(Weapon weapon, int slot) {
        if (slot < 0 || slot >= weapons.Count) {
            throw new System.Exception("WEAPON ATTEMPTED TO BE EQUIPPED TO AN INVALID INDEX: " + slot);
        }

        // Weapon in slot is being removed
        if (weapon == null) {
            weapons[slot] = null;
        }

        // Check if weapon already exists
        int index = weapons.IndexOf(weapon);

        // If weapon is NOT already equipped
        if (index == -1) {
            // Check if there is already a weapon at the slot it wants to be at
            if (weapons[slot] != null) {
                // Replace the equipped weapon with this one
                // But idk how to make sure the previous weapon is not lost...
            }
            // Equip the weapon
            weapons[slot] = weapon;
        }
        // If the weapon IS equipped
        else {
            // If the slot is the same, then player wants to UN-equip
            if (slot == index) {
                // Unequip
                weapons[slot] = null;
            }
            // Else you are trying to move the weapon to a different slot 
            else {
                // Swap the values of the two slots then
                var temp = weapons[slot];
                weapons[slot] = weapons[index];
                weapons[index] = temp;
            }
        }
    }
    
    public void EquipArmor(Armor armor, int slot) {
        for (int i = 0; i < armors.Count; i++) {
            if (armors[i] == armor) {
                // You already have this armor equipped
            }
        }
    }
}
