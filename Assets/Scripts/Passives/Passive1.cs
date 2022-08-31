using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On Round Start: GROW your smallest die
[CreateAssetMenu(menuName = "Passives/Passive 1")]
public class Passive1 : Passive
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);
        // Sub to event
        CombatEvents.instance.onRoundStart += GrowSmallestDie;
    }

    public override void Terminate()
    {
        // Unsub
        CombatEvents.instance.onRoundStart += GrowSmallestDie;
    }

    private void GrowSmallestDie(ActionInfo info)
    {
        Debug.Log("Passive 1 trigger!");

        // Define range to check depending on alligence
        (int, int) range = combatant.isAlly() ? (0, 4) : (4, 8);

        // Find the smallest die by value
        DiceUI smallestUI = null;
        for (int i = range.Item1; i < range.Item2; i++)
        {
            if (smallestUI == null || CombatManagerUI.instance.dieUIs[i].GetDie().GetValue() < smallestUI.GetDie().GetValue()) {
                smallestUI = CombatManagerUI.instance.dieUIs[i];
            }
        }

        // After finding die, grow it
        if (smallestUI != null) {
            smallestUI.Grow();
        }
        else {
            throw new System.Exception("SMALLEST UI NOT FOUND?!");
        }

        // Update info to choose how long to wait
        info.waitTime = 0.5f;
    }
}
