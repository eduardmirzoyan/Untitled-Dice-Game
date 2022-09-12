using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Mark")]
public class Mark : StatusEffect
{
    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        // Sub to event
        CombatEvents.instance.onRoundEnd += OnRoundEnd;
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
        CombatEvents.instance.onRoundEnd -= OnRoundEnd;
    }

    private void OnRoundEnd(int value) {
        // When round ends, decrease stacks by 1
        stacks--;
        if (stacks <= 0) {
            // Remove effect
            combatant.RemoveStatusEffect(this);
        }
    }
}
