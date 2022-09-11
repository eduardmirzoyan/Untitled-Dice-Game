using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL: Deal base damage to enemy and heal the amount dealt
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 2")]
public class HighrollAttack2 : AttackAction
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
        // Deal damage
        combatants[targetIndex].TakeDamage(FinalDamageValue());

        // Find caster (Still need to OPTIMIZE)
        foreach (var combatant in combatants) {
            // Heal caster
            if (combatant.unit.GetActions().Contains(this)) {
                combatant.Heal(FinalDamageValue());
            }
        }
    }
}
