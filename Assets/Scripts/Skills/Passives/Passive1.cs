using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Round Start: GROW your smallest die
[CreateAssetMenu(menuName = "Passives/Passive 1")]
public class Passive1 : Passive
{
    protected override void Init() {
        // Sub to event
        CombatEvents.instance.onSkillRoundStart += GrowSmallestDie;
    }

    protected override void Uninit() {
        // Unsub
        CombatEvents.instance.onSkillRoundStart += GrowSmallestDie;
    }

    private void GrowSmallestDie(ActionInfo info)
    {
        Debug.Log("Passive 1 trigger!");

        var die = sourceCombatant.dicePool.GetSmallest();
        die.Grow();
        CombatEvents.instance.TriggerOnGrow(die);

        // Update info to choose how long to wait
        info.waitTime = 0.5f;
    }
}
