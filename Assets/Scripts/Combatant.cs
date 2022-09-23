using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : ScriptableObject
{
    public Unit unit;
    public int index;
    public Vector3Int hexPosition;
    public Vector3 worldPosition;
    public List<StatusEffect> statusEffects;
    public DicePool dicePool;
    public float baseDamageMultiplier = 1f;
    
    public Transform modelTransform;
    public Transform diceTransform;

    public void Initialize(Unit unit, DicePool dicePool, int index, Vector3Int hexPosition) {
        this.unit = unit;
        this.dicePool = dicePool;
        this.hexPosition = hexPosition;
        this.index = index;
        this.worldPosition = CombatManagerUI.instance.GetCellCenter(hexPosition);
        statusEffects = new List<StatusEffect>();
    }

    public bool isAlly() {
        return index < 4;
    }

    public int GetLocalIndex() {
        return index % 4;
    }

    public void AssignModel(Transform transform) {
        this.modelTransform = transform;
    }

    public void AssignDie(Transform transform) {
        this.diceTransform = transform;
    }

    public void AddStatusEffect(StatusEffect statusEffect) {
        
        // Check if combatant already has this status effect
        foreach (var effect in statusEffects) {
            // Is the effect the same type as the one being added
            if (effect.GetType() == statusEffect.GetType()) {
                // Debug
                Debug.Log(unit.name + " already status effect: " + statusEffect.name + " so it just stacked.");

                // Then just stack the effect instead of adding new one
                effect.Stack(statusEffect);

                // Trigger event
                CombatEvents.instance.TriggerOnEffectUpdate(effect);
                return;
            }
        }

        // Debug
        Debug.Log(unit.name + " recieved the status effect: " + statusEffect.name);

        // Else add as new status effect
        statusEffects.Add(statusEffect);

        // Initialize effect with this combatant
        statusEffect.Initialize(this);

        // Trigger event
        CombatEvents.instance.TriggerOnAddEffect(statusEffect, this);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect) {
        
        // Search through effects
        foreach (var effect in statusEffects) {
            // If effect was found
            if (effect == statusEffect) {
                // Debug
                Debug.Log(unit.name + " lost the status effect: " + statusEffect.name);

                // Uninitialize
                effect.Uninitialize();

                // Then remove
                statusEffects.Remove(effect);

                // Trigger event
                CombatEvents.instance.TriggerOnRemoveEffect(statusEffect, this);

                // Dip
                return;
            }
        }

        // Debug
        Debug.Log(statusEffect.name + " was not found?!");
    }

    public void TakeDamage(int amount) {
        // Calls unit's take damage
        unit.TakeDamage(amount);

        // Debugg
        Debug.Log(unit.name + " took " + amount + " damage.");

        // Trigger event
        CombatEvents.instance.TriggerOnTakeDamage(this, amount);

        // Trigger death if approperiate
        if (unit.IsDead()) {
            CombatEvents.instance.TriggerOnDie(this);
        }
    }

    public void Heal(int amount) {
        // Calls unit's heal
        unit.Heal(amount);

        // Debug
        Debug.Log(unit.name + " healed " + amount + " hp.");

        // Trigger event
        CombatEvents.instance.TriggerOnHeal(this, amount);
    }
}
