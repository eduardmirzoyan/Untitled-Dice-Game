using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LOWROLL: Deal 2x base damage to target
[CreateAssetMenu(menuName = "Actions/Lowroll/Attack 1")]
public class LowrollAttack1 : AttackAction
{
    public override bool CheckDieConstraints(Dice dice)
    {
        return dice.IsLowroll();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!dice.IsLowroll())
        {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires a LOWROLL.");
        }
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        if (dice.IsLowroll())
        {
            // Deal damage
            combatants[targetIndex].TakeDamage(FinalDamageValue());
        }
    }
}