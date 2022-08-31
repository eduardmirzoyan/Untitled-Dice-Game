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

        // Define range to check depending on alligence
        (int, int) range = combatant.isAlly() ? (0, 4) : (4, 8);

        // Find the smallest die by value
        DiceUI largestUI = null;
        for (int i = range.Item1; i < range.Item2; i++)
        {
            if (largestUI == null || CombatManagerUI.instance.dieUIs[i].GetDie().GetValue() > largestUI.GetDie().GetValue())
            {
                largestUI = CombatManagerUI.instance.dieUIs[i];
            }
        }

        // After finding die, grow it
        if (largestUI != null)
        {
            largestUI.Shrink();
        }
        else
        {
            throw new System.Exception("LARGEST UI NOT FOUND?!");
        }

        // Update info to choose how long to wait
        info.waitTime = 0.5f;
    }
}
