using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Turn Start: SHRINK my die
[CreateAssetMenu(menuName = "Passives/Passive 6")]
public class Passive6 : Passive
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
        CombatEvents.instance.onTurnStart -= CheckPool;
    }

    private void CheckPool(ActionInfo info, Combatant combatant)
    {
        info.waitTime = 0;
        if (this.combatant == combatant) {
            Debug.Log("Passive 6 trigger!");

            combatant.unit.dice.Shrink();
            CombatEvents.instance.TriggerOnShrink(combatant.unit.dice);

            info.waitTime = 0.5f;
        }
        
    }
}

