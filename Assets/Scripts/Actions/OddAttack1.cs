using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EVEN: Deal 1.5x base damage to target
[CreateAssetMenu(menuName = "Actions/Odd/Attack 1")]
public class OddAttack1 : AttackAction
{
    public override bool checkDieConstraints(Dice dice)
    {
        return dice.isOdd();
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
