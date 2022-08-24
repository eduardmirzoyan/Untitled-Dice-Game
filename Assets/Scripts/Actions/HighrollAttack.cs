using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/HIGHROLL Attack")]
public class HighrollAttack : Action
{
    [SerializeField] private float damageMulitplier = 2f;

    public override bool passesContraints(Dice dice)
    {
        // Only checks if dice is highroll
        return dice.isHighroll();
    }

    // Deals dice damage to target if highroll
    public override void Perform(List<Combatant> targets, Dice dice) {
        if (dice.isHighroll()) {
            // Deal damage
            foreach (var target in targets) {
                // Generate damage
                int damage = (int) (sourceWeapon.baseDamage * damageMulitplier);
                // Deal damage based on weapon's base damage
                target.TakeDamage(damage);
                // target.unit.TakeDamage(dice.GetValue());
                Debug.Log(target.unit.name + " took " + damage + " damage.");
            }
        }
        else {
            Debug.Log("Die was not highroll.");
        }
        
    }
}
