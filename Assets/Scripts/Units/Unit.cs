using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Unit : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public int currentHealth;
    public int maxHealth;
    public Dice dice;
    public List<Weapon> weapons;
    public List<Armor> armors;
    public int speed;
    public GameObject modelPrefab;
    public List<Skill> innateSkills;
    public SlimeAI ai;

    public Unit Copy() {
        // Make a copy of self
        Unit copy = Instantiate(this);

        // Set copies die to a copy of this unit's die
        copy.dice = Instantiate(dice);

        // Copy all weapons
        for (int i = 0; i < weapons.Count; i++) {
            if (weapons[i] != null) {
                // Get a copy of each weapon
                copy.weapons[i] = (Weapon) weapons[i].Copy();
            }
        }

        // Copy all armor
        for (int i = 0; i < armors.Count; i++) {
            if (armors[i] != null) {
                // Get a copy of each armor
                copy.armors[i] = (Armor) armors[i].Copy();
            }
        }

        return copy;
    }

    public List<Skill> GetSkills() {
        List<Skill> result = new List<Skill>();

        foreach (var weapon in weapons) {
            if (weapon != null) {
                result.AddRange(weapon.skills);
            }
        }

        foreach (var armor in armors) {
            if (armor != null) {
                result.AddRange(armor.skills);
            }
        }

        // Get innate skills

        return result;
    }

    public List<Action> GetActions() {
        List<Action> result = new List<Action>();

        // Get actions from weapons
        foreach (var weapon in weapons) {
            if (weapon != null) {
                foreach (var skill in weapon.skills) {
                    if (skill is Action) {
                        result.Add((Action) skill);
                    }
                }
            }
            
        }
        
        // Get innate actions
        // TODO

        // Get actions from other sources
        // TODO

        return result;
    }

    public List<Passive> GetPassives() {
        List<Passive> result = new List<Passive>();

        // Get actions from armor
        foreach (var armor in armors) {
            if (armor != null) {
                foreach (var skill in armor.skills) {
                    if (skill is Passive) {
                        result.Add((Passive) skill);
                    }
                }
            }
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

    public void ClearEquipment() {
        // Set weapons to null
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i] = null;
        }

        // Set armors to null
        for (int i = 0; i < armors.Count; i++)
        {
            armors[i] = null;
        }
    }

    public void EquipWeapon(Weapon weapon, int slot) {
        if (slot < 0 || slot >= weapons.Count) {
            throw new System.Exception("WEAPON ATTEMPTED TO BE EQUIPPED TO AN INVALID INDEX: " + slot);
        }

        // Main logic is handled in UI... idk if this is good

        // Set weapon
        weapons[slot] = weapon;
        return;

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
        if (slot < 0 || slot >= armors.Count)
        {
            throw new System.Exception("ARMOR ATTEMPTED TO BE EQUIPPED TO AN INVALID INDEX: " + slot);
        }

        // Main logic is handled in UI... idk if this is good

        // Set armor
        armors[slot] = armor;
        return;

        // armor in slot is being removed
        if (armor == null)
        {
            armors[slot] = null;
        }

        // Check if armor already exists
        int index = armors.IndexOf(armor);

        // If armor is NOT already equipped
        if (index == -1)
        {
            // Check if there is already a armor at the slot it wants to be at
            if (armors[slot] != null)
            {
                // Replace the equipped armor with this one
                // But idk how to make sure the previous armor is not lost...
            }
            // Equip the armor
            armors[slot] = armor;
        }
        // If the armor IS equipped
        else
        {
            // If the slot is the same, then player wants to UN-equip
            if (slot == index)
            {
                // Unequip
                armors[slot] = null;
            }
            // Else you are trying to move the armor to a different slot 
            else
            {
                // Swap the values of the two slots then
                var temp = armors[slot];
                armors[slot] = armors[index];
                armors[index] = temp;
            }
        }
    }
}
