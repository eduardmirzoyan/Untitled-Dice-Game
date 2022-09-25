using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Basic/Inverse Damage")]
public class InverseDamageAttack : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        combatants[targetIndex].TakeDamage(dice.GetInvertedValue());
    }
}
