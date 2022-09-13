using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deal 1x EVEN: Gain 1 STRENGTH
[CreateAssetMenu(menuName = "Actions/Even/Attack 3")]
public class EvenAttack3 : AttackAction
{
    public Strength strength;

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // If die is even
        if (dice.IsEven())
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
