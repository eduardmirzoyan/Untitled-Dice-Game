using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EVEN: Deal 1.5x base damage to target
[CreateAssetMenu(menuName = "Actions/Even/Attack 1")]
public class EvenAttack1 : AttackAction
{
    public override bool CheckDieConstraints(Dice dice)
    {
        return dice.IsEven();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!dice.IsEven()) {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires an EVEN die.");
        }
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
