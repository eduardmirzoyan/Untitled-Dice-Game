using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL: Apply MARK
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 5")]
public class HighrollAttack5 : AttackAction
{
    public Mark mark;

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
        // Create copy
        Mark newMark = Instantiate(mark);

        // Add effect
        sourceCombatant.AddStatusEffect(newMark);
    }
}
