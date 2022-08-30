using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LOWROLL: Deal 2x base damage to target
[CreateAssetMenu(menuName = "Actions/Lowroll/Attack 1")]
public class LowrollAttack1 : AttackAction
{
    public override bool checkDieConstraints(Dice dice)
    {
        return dice.isLowroll();
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        if (dice.isLowroll())
        {
            // Deal damage
            combatants[targetIndex].TakeDamage(FinalDamageValue());
        }
    }
}