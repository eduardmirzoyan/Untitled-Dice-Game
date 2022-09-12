using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Strength")]
public class Strength : StatusEffect
{
    [SerializeField] private float percentDamagePerStack = 0.2f;

    public override void Initialize(Combatant combatant)
    {
        base.Initialize(combatant);

        // Give combatant a mulitplier
        combatant.baseDamageMultiplier += percentDamagePerStack * stacks;

        // Sub to event
        CombatEvents.instance.onActionPerformed += OnAttackActionPerformed;
    }

    public override void Stack(StatusEffect statusEffect)
    {
        // Sum the stacks
        stacks += statusEffect.stacks;

        // Increase multiplier
        combatant.baseDamageMultiplier += percentDamagePerStack * statusEffect.stacks;
    }

    public override void Uninitialize()
    {
        // Remove multiplier
        combatant.baseDamageMultiplier -= percentDamagePerStack * stacks;

        // Unsub to event
        CombatEvents.instance.onActionPerformed -= OnAttackActionPerformed;

        base.Uninitialize();
    }

    private void OnAttackActionPerformed(Action action) {
        // If it is this combatant's turn and they used an attack action
        if (CombatManager.instance.currentCombatant == this.combatant && action is AttackAction) {
            Debug.Log("removed!");
            // Set stacks to 0
            stacks = 0;

            // Trigger events
            CombatEvents.instance.TriggerOnEffectProc(this);
            CombatEvents.instance.TriggerOnEffectUpdate(this);

            // Remove this effect
            combatant.RemoveStatusEffect(this);
        }
    }
}
