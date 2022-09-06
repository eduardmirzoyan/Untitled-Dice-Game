using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// // On Turn Start: REROLL my die
[CreateAssetMenu(menuName = "Passives/Passive 9")]
public class Passive9 : Passive
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);
        // Sub to event
        CombatEvents.instance.onTurnStart += RerollDie;
    }

    public override void Terminate()
    {
        // Unsub
        CombatEvents.instance.onTurnStart -= RerollDie;
    }

    private void RerollDie(ActionInfo info, Combatant combatant)
    {
        Debug.Log("Passive 9 trigger!");

        // If this is the combatant whose turn it is
        if (this.combatant == combatant) {
            // Reroll own die
            combatant.unit.dice.Roll();

            // Trigger event
            CombatEvents.instance.TriggerReroll(combatant.unit.dice);

            // Set wait to 0.5
            info.waitTime = CombatManager.instance.rollTime;
        }
        
    }
}
