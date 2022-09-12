using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL POOL: Deal 2x base damage to target PARTY
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 5")]
public class HighrollAttack5 : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Target is from 0-7, 0-3 are allies, 4-7 are enemies
        combatants[targetIndex].TakeDamage(sourceWeapon.baseDamage);



    }
}
