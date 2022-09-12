using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL: REROLL target’s die
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 4")]
public class HighrollAttack4 : AttackAction
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

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Target is from 0-7, 0-3 are allies, 4-7 are enemies
        combatants[targetIndex].unit.dice.Roll();

        // Trigger event
        CombatEvents.instance.TriggerOnReroll(combatants[targetIndex].unit.dice);
    }
}
