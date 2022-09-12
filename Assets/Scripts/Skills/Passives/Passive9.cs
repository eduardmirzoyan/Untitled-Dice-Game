using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// // On Turn Start: REROLL my die
[CreateAssetMenu(menuName = "Passives/Passive 9")]
public class Passive9 : Passive
{
    protected override void Init()
    {
        // Sub to event
        CombatEvents.instance.onSkillTurnStart += RerollDie;
    }

    protected override void Uninit()
    {
        // Unsub
        CombatEvents.instance.onSkillTurnStart -= RerollDie;
    }

    private void RerollDie(ActionInfo info, Combatant combatant)
    {
        Debug.Log("Passive 9 trigger!");

        // If this is the combatant whose turn it is
        if (this.sourceCombatant == combatant) {
            // Reroll own die
            combatant.unit.dice.Roll();

            // Trigger event
            CombatEvents.instance.TriggerOnReroll(combatant.unit.dice);

            // Set wait to 0.5
            info.waitTime = CombatManager.instance.rollTime;
        }
        
    }
}
