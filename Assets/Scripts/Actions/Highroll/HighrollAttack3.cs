using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deal base damage to target HIGHROLL: Deal base damage to adjacent units
[CreateAssetMenu(menuName = "Actions/Highroll/Attack 3")]
public class HighrollAttack3 : AttackAction
{
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Target is from 0-7, 0-3 are allies, 4-7 are enemies
        combatants[targetIndex].TakeDamage(sourceWeapon.baseDamage);

        // If die is highroll, target adjecency
        if (dice.IsHighroll()) {
            // If you're not crossing alliegence or out of bounds then you can do it
            if (targetIndex + 1 != 4 && targetIndex + 1 != 8)
            {
                combatants[targetIndex + 1].TakeDamage(sourceWeapon.baseDamage);
            }

            // If you're not crossing alliegence or out of bounds then you can do it
            if (targetIndex - 1 != 3 && targetIndex - 1 != -1)
            {
                combatants[targetIndex - 1].TakeDamage(sourceWeapon.baseDamage);
            }
        }
        
    }
}
