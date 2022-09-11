using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UNIQUE POOL: Deal 2.5x Base damage
[CreateAssetMenu(menuName = "Actions/Unique/Attack 1")]
public class UniqueAttack1 : AttackAction
{
    public override bool CheckDieConstraints(Dice dice)
    {
        return CombatManager.instance.currentCombatant.dicePool.IsUnique();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!CombatManager.instance.currentCombatant.dicePool.IsUnique())
        {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires you to have a UNIQUE die pool.");
        }
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
