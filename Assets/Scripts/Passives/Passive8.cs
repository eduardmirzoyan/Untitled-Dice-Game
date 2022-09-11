using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IF you have 2+ EVEN die, SHRINK the first one
[CreateAssetMenu(menuName = "Passives/Passive 8")]
public class Passive8 : Passive
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);
        // Sub to event
        CombatEvents.instance.onSkillTurnStart += Shrink2Even;
    }

    public override void Terminate()
    {
        // Unsub
        CombatEvents.instance.onSkillTurnStart -= Shrink2Even;
    }

    private void Shrink2Even(ActionInfo info, Combatant combatant)
    {
        Debug.Log("Passive 8 trigger!");

        // Set wait to 0
        info.waitTime = 0f;

        // Make sure it's this guy's turn
        if (this.combatant == combatant && combatant.dicePool.EvenCount() >= 2) {
            // Get first Even
            var die = combatant.dicePool.GetFirstEven();
            
            // Make sure it exists
            if (die == null) throw new System.Exception("NO EVEN FOUND?!");

            // Grow chosen die
            die.Shrink();

            // Trigger event
            CombatEvents.instance.TriggerOnShrink(die);

            // If effect triggers, then increase wait time
            info.waitTime = CombatManager.instance.rollTime;
        }

    }
}
