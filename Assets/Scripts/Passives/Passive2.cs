using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Round Start: SHRINK your largest die
[CreateAssetMenu(menuName = "Passives/Passive 12")]
public class Passive2 : Passive
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);
        // Sub to event
        CombatEvents.instance.onRoundStart += ShrinkLargestDie;
    }

    public override void Terminate()
    {
        // Unsub
        CombatEvents.instance.onRoundStart += ShrinkLargestDie;
    }

    private void ShrinkLargestDie(ActionInfo info)
    {
        Debug.Log("Passive 2 trigger!");

        var die = combatant.dicePool.GetLargest();
        die.Shrink();
        CombatEvents.instance.TriggerOnShrink(die);

        // Update info to choose how long to wait
        info.waitTime = 0.5f;
    }
}