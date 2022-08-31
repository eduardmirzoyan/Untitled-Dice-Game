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
        CombatEvents.instance.onTurnStart += CheckPool;
    }

    public override void Terminate()
    {
        // Unsub
        CombatEvents.instance.onTurnStart += CheckPool;
    }

    private void CheckPool(ActionInfo info, Combatant combatant)
    {
        Debug.Log("Passive 3 trigger!");
        // Set wait to 0
        info.waitTime = 0f;

        if (this.combatant == combatant) {
            // Dice pool concept?

            // If effect triggers, then increase wait time
            info.waitTime = 0.5f;
        }
    }
}
