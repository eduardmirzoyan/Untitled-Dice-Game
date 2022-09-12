using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Round Start: SHRINK your largest die
[CreateAssetMenu(menuName = "Passives/Passive 2")]
public class Passive2 : Passive
{
    protected override void Init()
    {
        // Sub to event
        CombatEvents.instance.onSkillRoundStart += ShrinkLargestDie;
    }

    protected override void Uninit()
    {
        // Unsub
        CombatEvents.instance.onSkillRoundStart += ShrinkLargestDie;
    }

    private void ShrinkLargestDie(ActionInfo info)
    {
        Debug.Log("Passive 2 trigger!");

        var die = sourceCombatant.dicePool.GetLargest();
        die.Shrink();
        CombatEvents.instance.TriggerOnShrink(die);

        // Update info to choose how long to wait
        info.waitTime = 0.5f;
    }
}
