using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL: Deal 2x base damage to target
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 1")]
public class HighrollAttack1 : AttackAction
{
    public override bool checkDieConstraints(Dice dice) 
    {
        return dice.IsHighroll();
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice) 
    {
        if (dice.IsHighroll()) {
            // Deal damage
            combatants[targetIndex].TakeDamage(FinalDamageValue());
        }
    }
}
