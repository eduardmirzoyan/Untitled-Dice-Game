using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Turn Start: SHRINK my die
[CreateAssetMenu(menuName = "Passives/Passive 6")]
public class Passive6 : Passive
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
            Debug.Log("Passive 6 trigger!");

            combatant.unit.dice.Shrink();
            CombatEvents.instance.TriggerOnShrink(combatant.unit.dice);

            info.waitTime = 0.5f;
        }
        
    }
}

