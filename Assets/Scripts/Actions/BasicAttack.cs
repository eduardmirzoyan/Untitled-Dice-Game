using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Basic Attack")]
public class BasicAttack : Action
{
    // Deals dice damage to all targets
    public override void Perform(List<Combatant> targets, Dice dice) {
        // Deal damage
        foreach (var target in targets) {
            // Deal damage based on weapon's base damage
            target.TakeDamage(sourceWeapon.baseDamage);
            // target.unit.TakeDamage(dice.GetValue());
            Debug.Log(target.unit.name + " took " + sourceWeapon.baseDamage + " damage.");
        }
    }
}
