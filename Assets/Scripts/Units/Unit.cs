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
    public Weapon[] weapons;
    public int speed;
    public GameObject modelPrefab;
    public bool isNPC;
    public List<Passive> innatePassives;

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
        // Get innate passives

        // Get passives from equipment

        // Get passives from other sources
        // TODO

        return innatePassives;
    }

    public void TakeDamage(int amount) {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public virtual bool IsDead() {
        return currentHealth <= 0;
    }
}
