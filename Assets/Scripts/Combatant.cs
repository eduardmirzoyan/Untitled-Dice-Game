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
    
    public Transform modelTransform;

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

    public void AddStatusEffect(StatusEffect statusEffect) {
        // TODO: Finish this

        // Debug
        Debug.Log(unit.name + " recieved the status effect: " + statusEffect.name);

        // Adding logic ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // If combatant already has this status effect...?
        if (statusEffects.Contains(statusEffect)) {
            // TODO: FIX THIS!!!
            statusEffects[0].Stack(statusEffect);
        }
        else {
            // Else add as new status effect
            statusEffects.Add(statusEffect);

            // Initialize effect with this combatant
            statusEffect.Initialize(this);
        }

        // Trigger event
        CombatEvents.instance.TriggerOnAddEffect(statusEffect, this);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect) {
        // TODO

        // Unintialize effect
        statusEffect.Uninitialize();
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
