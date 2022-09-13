using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// BOUNTY: Deal 2x damage
[CreateAssetMenu(menuName = "Actions/Bounty/Attack 1")]
public class BountyAttack : AttackAction
{
    public override bool CheckTargetConstraints(Combatant combatant) {
        // Return true if the target has a mark effect
        return combatant.statusEffects.Any(effect => effect is Mark);
    }

    public override void ShowTargetConstraintFeedback(Combatant combatant)
    {
        // If you don't pass contraints
        if (!CheckTargetConstraints(combatant)) {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This unit is not MARKED.");
        }
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        throw new System.NotImplementedException();
    }
}
