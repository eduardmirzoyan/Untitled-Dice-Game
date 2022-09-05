using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : Item
{
    public int baseDamage;
    public List<Action> actions;

    public void Initialize(int baseDamage, List<Action> actions) {
        // Make new list of actions
        this.actions = new List<Action>();

        // Get base damage
        this.baseDamage = baseDamage;

        foreach (var action in actions) {
            // Update the weapon source if the action is an attack
            if (action is AttackAction) (action as AttackAction).sourceWeapon = this;

            // Add action to weapon's list
            this.actions.Add(action);
        }
    }
}
