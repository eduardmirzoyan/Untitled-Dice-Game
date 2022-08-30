using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Basic Attack")]
public class BasicAttack : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Deal damage
        combatants[targetIndex].TakeDamage(FinalDamageValue());
    }
}
