using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL: Deal 2x base damage to target
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 1")]
public class HighrollAttack1 : AttackAction
{
    public override bool CheckDieConstraints(Dice dice) 
    {
        return dice.IsHighroll();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!dice.IsHighroll())
        {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires a HIGHROLL.");
        }
    }
}
