using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PAIR: Deal 2x Base damage
[CreateAssetMenu(menuName = "Actions/Pair/Attack 1")]
public class PairAttack1 : AttackAction
{
    public override bool checkDieConstraints(Dice dice)
    {
        return CombatManager.instance.currentCombatant.dicePool.HasPair();
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
