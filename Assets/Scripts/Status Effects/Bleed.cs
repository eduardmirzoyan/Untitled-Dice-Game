using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Bleed")]
public class Bleed : StatusEffect
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        // Sub to event
        CombatEvents.instance.onTurnStart += OnTurnStart;
    }

    public override void Stack(StatusEffect statusEffect)
    {
        // Add the stacks from the new bleed effect
        stacks += statusEffect.stacks;
    }

    public override void Uninitialize()
    {
        base.Uninitialize();

        // Unsub to event
        CombatEvents.instance.onTurnStart -= OnTurnStart;
    }

    private void OnTurnStart(int value) {
        // If it is this combatant's turn
        if (CombatManager.instance.currentCombatant == combatant) {
            // Deal damage equal to stacks
            combatant.TakeDamage(stacks);

            // Then half stacks, rounding down
            stacks /= 2;

            // Then if stacks equal 0, remove effect
            if (stacks == 0) {
                // Remove this effect
                combatant.RemoveStatusEffect(this);
            }
        }
    }
}
