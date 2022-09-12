using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HIGHROLL POOL: Deal 4x base damage to target
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 6")]
public class HighrollAttack6 : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Target is from 0-7, 0-3 are allies, 4-7 are enemies
        combatants[targetIndex].TakeDamage(sourceWeapon.baseDamage);



    }
}
