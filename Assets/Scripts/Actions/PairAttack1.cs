using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PAIR: Deal 2x Base damage
[CreateAssetMenu(menuName = "Actions/Pair/Attack 1")]
public class PairAttack1 : AttackAction
{
    public override bool CheckDieConstraints(Dice dice)
    {
        return CombatManager.instance.currentCombatant.dicePool.HasPair();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!CombatManager.instance.currentCombatant.dicePool.HasPair()) {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires you to have a PAIR.");
        } 
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
