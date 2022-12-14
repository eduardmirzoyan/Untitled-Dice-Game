using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Turn Start: GROW my die
[CreateAssetMenu(menuName = "Passives/Passive 5")]
public class Passive5 : Passive
{
    protected override void Init()
    {
        // Sub to event
        CombatEvents.instance.onSkillTurnStart += CheckPool;
    }

    protected override void Uninit()
    {
        // Unsub
        CombatEvents.instance.onSkillTurnStart -= CheckPool;
    }

    private void CheckPool(ActionInfo info, Combatant combatant)
    {
        info.waitTime = 0;
        if (this.sourceCombatant == combatant) {
            Debug.Log("Passive 5 trigger!");

            combatant.unit.dice.Grow();
            CombatEvents.instance.TriggerOnGrow(combatant.unit.dice);

            info.waitTime = 0.5f;
        }
        
    }
}
