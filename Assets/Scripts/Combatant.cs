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

    public void AssignHealthbar(HealthbarUI healthbar) {
        this.healthbar = healthbar;

        healthbar.Initialize(unit.maxHealth, unit.currentHealth);
    }

    public void TakeDamage(int amount) {
        // Calls unit
        unit.TakeDamage(amount);

        var position = CombatManagerUI.instance.GetCellCenter(worldPosition);
        // Spawn damage prefab
        CombatManagerUI.instance.SpawnFloatingNumber(amount.ToString(), position);

        // Updates health bar UI
        healthbar.UpdateHealth(unit.currentHealth);
    }
}
