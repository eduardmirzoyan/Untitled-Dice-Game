using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Turn Start: Set my die to a HIGHROLL
[CreateAssetMenu(menuName = "Passives/Passive 10")]
public class Passive10 : Passive
{
    protected override void Init()
    {
        // Sub to event
        CombatEvents.instance.onSkillTurnStart += SetDieToHighRoll;
    }

    protected override void Uninit()
    {
        // Unsub
        CombatEvents.instance.onSkillTurnStart -= SetDieToHighRoll;
    }

    private void SetDieToHighRoll(ActionInfo info, Combatant combatant)
    {
        Debug.Log("Passive 10 trigger!");

        // If this is the combatant whose turn it is
        if (this.sourceCombatant == combatant) {
            // Set die to max value
            combatant.unit.dice.SetValue(combatant.unit.dice.maxValue);

            // Trigger event
            CombatEvents.instance.TriggerOnSet(combatant.unit.dice);

            // Set wait to 0
            info.waitTime = 0f;
        }
        
    }
}
