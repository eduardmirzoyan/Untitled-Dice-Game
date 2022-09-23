using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Burn")]
public class Burn : StatusEffect
{
    /*
    Description: When this unit’s die is used, deal (stacks) damage to the user and then half stacks.
    
    Damage is dealt to the unit USING the die!

    */

    public GameObject fireParticlesPrefab;
    public int decrementAmount = 2;

    private ParticleSystem fireParticles;

    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        // Spawn particles
        fireParticles = Instantiate(fireParticlesPrefab, combatant.diceTransform).GetComponent<ParticleSystem>();

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

        if (fireParticles != null) {
            // Destroy particles
            Destroy(fireParticles.gameObject);
        }

        // Unsub to event
        CombatEvents.instance.onActionConfirm -= TriggerEffect;
    }

    private void TriggerEffect(Combatant source, Action action, Dice dice, Combatant target) {
        // Check if this combatant's die was used
        if (combatant.unit.dice == dice) 
        {
            // Deal damage to die USER equal to stacks
            source.TakeDamage(stacks);

            // The reduce stacks by half
            stacks /= decrementAmount;

            // Trigger events
            CombatEvents.instance.TriggerOnEffectProc(this);
            CombatEvents.instance.TriggerOnEffectUpdate(this);

            // Then if stacks equal 0, remove effect
            if (stacks == 0)
            {
                // Remove this effect
                combatant.RemoveStatusEffect(this);
            }
        }
    }
}
