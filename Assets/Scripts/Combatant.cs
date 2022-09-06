using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : ScriptableObject
{
    public Unit unit;
    public int index;
    public Vector3Int worldPosition;
    public DicePool dicePool;
    
    public Transform modelTransform; // Change this to event based
    public HealthbarUI healthbar; // Change this to event based

    public void Initialize(Unit unit, DicePool dicePool, int index, Vector3Int worldPosition) {
        this.unit = unit;
        this.dicePool = dicePool;
        this.worldPosition = worldPosition;
        this.index = index;
    }

    public bool isAlly() {
        return index < 4;
    }

    public int GetLocalIndex() {
        return index % 4;
    }

    public void AssignHealthbar(HealthbarUI healthbar) {
        this.healthbar = healthbar;

        healthbar.Initialize(unit.maxHealth, unit.currentHealth);
    }

    public void AssignModel(Transform transform) {
        this.modelTransform = transform;
    }

    public void TakeDamage(int amount) {
        // Calls unit's take damage
        unit.TakeDamage(amount);

        // Feedback
        Debug.Log(unit.name + " took " + amount + " damage.");

        // Spawn floating number for visual feedback
        var position = CombatManagerUI.instance.GetCellCenter(worldPosition);
        CombatManagerUI.instance.SpawnFloatingNumber(amount.ToString(), position);

        // Updates health bar UI
        healthbar.UpdateHealth(unit.currentHealth);

        // Trigger approperiate event
        if (unit.IsDead()) {
            CombatEvents.instance.TriggerOnDie(this);
        }   
        else {
            CombatEvents.instance.TriggerOnTakeDamage(this);
        }
    }

    public void Heal(int amount) {
        // Calls unit's heal
        unit.Heal(amount);

        Debug.Log(unit.name + " healed " + amount + " hp.");

        // Spawn floating number for visual feedback
        var position = CombatManagerUI.instance.GetCellCenter(worldPosition);
        CombatManagerUI.instance.SpawnFloatingNumber(amount.ToString(), position);

        // Updates health bar UI
        healthbar.UpdateHealth(unit.currentHealth);
    }
}
