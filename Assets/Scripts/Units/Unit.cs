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
    public GameObject prefab;
    public bool isNPC;

    public List<Action> GetActions() {
        List<Action> result = new List<Action>();

        // Get actions from weapons
        foreach (var weapon in weapons) {
            result.AddRange(weapon.actions);
        }
        
        // Get actions from passives
        // TODO

        // Get actions from other sources
        // TODO

        // Get default action, Pass
        // TODO

        return result;
    }

    public void TakeDamage(int value) {
        currentHealth = Mathf.Max(currentHealth - value, 0);
    }

    public virtual bool IsDead() {
        return currentHealth <= 0;
    }
}
