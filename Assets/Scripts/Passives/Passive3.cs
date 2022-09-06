using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Round Start: If your POOL has no EVEN die, REROLL my die
[CreateAssetMenu(menuName = "Passives/Passive 3")]
public class Passive3 : Passive
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);
        // Sub to event
        CombatEvents.instance.onRoundStart += CheckPool;
    }

    public override void Terminate()
    {
        // Unsub
        CombatEvents.instance.onRoundStart -= CheckPool;
    }

    private void CheckPool(ActionInfo info)
    {
        Debug.Log("Passive 3 trigger!");
        // Set wait to 0
        info.waitTime = 0f;

        // Dice pool concept?
        if (combatant.dicePool.EvenCount() == 0)
        {
            // Reroll the die
            combatant.unit.dice.Roll();
            // Trigger event
            CombatEvents.instance.TriggerReroll(combatant.unit.dice);

            // If effect triggers, then increase wait time
            info.waitTime = 0.5f;
        }

        
    }
}
