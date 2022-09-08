using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : ScriptableObject
{
    public Unit unit;
    public int index;
    public Vector3Int hexPosition;
    public Vector3 worldPosition;
    public DicePool dicePool;
    
    public Transform modelTransform; // Change this to event based

    public void Initialize(Unit unit, DicePool dicePool, int index, Vector3Int hexPosition) {
        this.unit = unit;
        this.dicePool = dicePool;
        this.hexPosition = hexPosition;
        this.index = index;
        this.worldPosition = CombatManagerUI.instance.GetCellCenter(hexPosition);
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

    public void TakeDamage(int amount) {
        // Calls unit's take damage
        unit.TakeDamage(amount);

        // Debugging
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

        // Debugging
        Debug.Log(unit.name + " healed " + amount + " hp.");

        // Trigger event
        CombatEvents.instance.TriggerOnHeal(this, amount);
    }
}
