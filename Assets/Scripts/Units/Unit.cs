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
    public Armor[] armors;
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
}
