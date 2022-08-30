using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : ScriptableObject
{
    public Unit unit;
    public int partyIndex;
    public Vector3Int worldPosition;
    public bool isAllyAllegiance;
    
    public HealthbarUI healthbar;

    public Combatant(Unit unit, int partyIndex, Vector3Int worldPosition, bool isAllyAllegiance) {
        this.unit = unit;
        this.partyIndex = partyIndex;
        this.worldPosition = worldPosition;
        this.isAllyAllegiance = isAllyAllegiance;
    }

    public void Initialize(Unit unit, int index, Vector3Int worldPosition) {
        this.unit = unit;
        this.worldPosition = worldPosition;
        this.partyIndex = index;
    }

    public bool isAlly() {
        return partyIndex < 4;
    }

    public int GetLocalIndex() {
        return partyIndex % 4;
    }

    public void AssignHealthbar(HealthbarUI healthbar) {
        this.healthbar = healthbar;

        healthbar.Initialize(unit.maxHealth, unit.currentHealth);
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
