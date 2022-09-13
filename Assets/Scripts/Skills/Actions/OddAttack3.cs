using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deal 1x ODD: Gain 1 STRENGTH after
[CreateAssetMenu(menuName = "Actions/Odd/Attack 3")]
public class OddAttack3 : AttackAction
{
    public Strength strength;

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // If die is odd
        if (dice.IsOdd())
        {
            // Create copy
            Strength newStrength = Instantiate(strength);
            newStrength.stacks = 1;
            
            // Add effect
            sourceCombatant.AddStatusEffect(newStrength);
        }

        // Do regular damage
        base.Perform(targetIndex, combatants, dice);
    }
}
