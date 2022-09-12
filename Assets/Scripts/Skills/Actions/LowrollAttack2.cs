using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LOWROLL: Deal base damage to 3 random enemies
[CreateAssetMenu(menuName = "Actions/Lowroll/Attack 2")]
public class LowrollAttack2 : AttackAction
{
    public int numberOfTargets = 3;

    public override bool CheckDieConstraints(Dice dice)
    {
        return dice.IsLowroll();
    }

    public override void ShowDieConstraintFeedback(Dice dice)
    {
        if (!dice.IsLowroll())
        {
            // Trigger event
            CombatEvents.instance.TriggerOnFeedback("This action requires a LOWROLL.");
        }
    }

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice) 
    {
        if (dice.IsLowroll()) {
            if (targetIndex < 4)
            {
                // Deal damage 3 times
                for (int i = 0; i < numberOfTargets; i++)
                {
                    int randomIndex = Random.Range(0, 4);
                    combatants[randomIndex].TakeDamage(ActionBaseDamage());
                }
            }
            else
            {
                for (int i = 0; i < numberOfTargets; i++)
                {
                    int randomIndex = Random.Range(4, 8);
                    combatants[randomIndex].TakeDamage(ActionBaseDamage());
                }
            }
        }
    }
}