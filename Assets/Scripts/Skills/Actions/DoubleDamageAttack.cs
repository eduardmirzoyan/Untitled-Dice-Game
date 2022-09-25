using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Basic/Double Damage")]
public class DoubleDamageAttack : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice) {
        combatants[targetIndex].TakeDamage(dice.GetValue() * 2);
    }
}
