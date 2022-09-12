using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Turn Start: If you have 2+ ODD die, GROW the first one
[CreateAssetMenu(menuName = "Passives/Passive 7")]
public class Passive7 : Passive
{
    protected override void Init()
    {
        // Sub to event
        CombatEvents.instance.onSkillTurnStart += Grow2Odd;
    }

    protected override void Uninit()
    {
        // Unsub
        CombatEvents.instance.onSkillTurnStart -= Grow2Odd;
    }

    private void Grow2Odd(ActionInfo info, Combatant combatant)
    {
        Debug.Log("Passive 7 trigger!");

        // Set wait to 0
        info.waitTime = 0f;

        // Dice pool concept?
        if (this.sourceCombatant == combatant && combatant.dicePool.OddCount() >= 2)
        {
            // Get first Odd
            var die = combatant.dicePool.GetFirstOdd();
            
            // Make sure it exists
            if (die == null) throw new System.Exception("NO ODD FOUND?!");

            // Reroll chosen die
            die.Grow();

            // Trigger event
            CombatEvents.instance.TriggerOnGrow(die);

            // If effect triggers, then increase wait time
            info.waitTime = CombatManager.instance.rollTime;
        }

    }
}
