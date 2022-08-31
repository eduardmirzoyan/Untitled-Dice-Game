using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deal base damage to target LOWROLL: Do it again
[CreateAssetMenu(menuName = "Actions/Lowroll/Attack 3")]
public class LowrollAttack3 : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice) {
        // Deal damage
        combatants[targetIndex].TakeDamage(FinalDamageValue());

        // Do it again
        if (dice.IsLowroll()) {
            combatants[targetIndex].TakeDamage(FinalDamageValue());
        }
    }
}