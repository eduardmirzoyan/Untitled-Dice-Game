using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Poison")]
public class Poison : StatusEffect
{
    /*
    Description: When this unit’s die is used, deal (stacks) to this unit then add 1 to stacks then transfer this status to the user.
    
    NOTE: Doesn’t jump to DEAD units. If there is only one party member, then it stays on the same unit. 
    If a POISON attaches to a unit that already has POISON, combines with existing POISON and sums stacks.
    
    Damage is dealt to the unit HOLDING the die!

    */

    public GameObject poisonParticlesPrefab;
    public int incrementAmount = 1;
    private ParticleSystem poisonParticles;

    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        // Spawn particles
        poisonParticles = Instantiate(poisonParticlesPrefab, combatant.diceTransform).GetComponent<ParticleSystem>();

        // Sub to event
        CombatEvents.instance.onActionConfirm += TriggerEffect;
    }

    public override void Stack(StatusEffect statusEffect)
    {
        // Add the stacks
        stacks += statusEffect.stacks;
    }

    public override void Uninitialize()
    {
        base.Uninitialize();

        if (poisonParticles != null)
        {
            // Destroy particles
            Destroy(poisonParticles.gameObject);
        }

        // Unsub to event
        CombatEvents.instance.onActionConfirm -= TriggerEffect;
    }

    private void TriggerEffect(Combatant source, Action action, Dice dice, Combatant target)
    {
        // Check if this combatant's die was used
        if (combatant.unit.dice == dice) {
            // Deal damage to die HOLDER equal to stacks
            combatant.TakeDamage(stacks);

            // Then increase stacks
            stacks += incrementAmount;

            // Trigger events
            CombatEvents.instance.TriggerOnEffectProc(this);
            CombatEvents.instance.TriggerOnEffectUpdate(this);

            // If you are not using your own die
            if (source != combatant) {
                // Now transfer effect to source
                source.AddStatusEffect(Instantiate(this));

                // Remove it from this guy
                combatant.RemoveStatusEffect(this);
            }
        }
    }
}
