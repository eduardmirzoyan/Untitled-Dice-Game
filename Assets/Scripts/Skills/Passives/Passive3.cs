using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Round Start: If your POOL has no EVEN die, REROLL my die
[CreateAssetMenu(menuName = "Passives/Passive 3")]
public class Passive3 : Passive
{
    protected override void Init()
    {
        // Sub to event
        CombatEvents.instance.onSkillRoundStart += CheckPool;
    }

    protected override void Uninit()
    {
        // Unsub
        CombatEvents.instance.onSkillRoundStart -= CheckPool;
    }

    private void CheckPool(ActionInfo info)
    {
        Debug.Log("Passive 3 trigger!");
        // Set wait to 0
        info.waitTime = 0f;

        // Dice pool concept?
        if (sourceCombatant.dicePool.EvenCount() == 0)
        {
            // Reroll the die
            sourceCombatant.unit.dice.Roll();
            // Trigger event
            CombatEvents.instance.TriggerOnReroll(sourceCombatant.unit.dice);

            // If effect triggers, then increase wait time
            info.waitTime = 0.5f;
        }

        
    }
}
