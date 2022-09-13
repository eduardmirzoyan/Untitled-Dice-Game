using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deal 1x EVEN: Deal 1x as BLEED instead
[CreateAssetMenu(menuName = "Actions/Even/Attack 2")]
public class EvenAttack2 : AttackAction
{
    public Bleed bleed;
    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // If die is even
        if (dice.IsEven()) {
            // Create copy
            Bleed newBleed = Instantiate(bleed);
            // Set stacks equal to damage value
            newBleed.stacks = FinalDamageValue();

            // Apply effect
            combatants[targetIndex].AddStatusEffect(newBleed);
        }
        else {
            // Do regular damage
            base.Perform(targetIndex, combatants, dice);
        }
    }
}
