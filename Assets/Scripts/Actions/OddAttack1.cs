using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EVEN: Deal 1.5x base damage to target
[CreateAssetMenu(menuName = "Actions/Odd/Attack 1")]
public class OddAttack1 : AttackAction
{
    public override bool CheckDieConstraints(Dice dice)
    {
        return dice.IsOdd();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!dice.IsOdd()) {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires an ODD die.");
        }
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
