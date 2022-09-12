using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectHolderUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameObject statusEffectUIPrefab;
    [SerializeField] private Combatant combatant;
    [SerializeField] private List<StatusEffectUI> statusEffectUIs;

    private void Awake() {
        statusEffectUIs = new List<StatusEffectUI>();
    }

    public void Initialize(Combatant combatant) {
        this.combatant = combatant;

        // Sub to events
        CombatEvents.instance.onAddEffect += AddStatusEffect;
        CombatEvents.instance.onRemoveEffect += RemoveStatusEffect;
    }

    public void AddStatusEffect(StatusEffect statusEffect, Combatant combatant) {
        // If this combatant gained an effect
        if (this.combatant == combatant) {
            // Create status effect visual
            var statusUI = Instantiate(statusEffectUIPrefab, transform).GetComponent<StatusEffectUI>();
            statusUI.Initialize(statusEffect);
            // Cache reference
            statusEffectUIs.Add(statusUI);
        }
    }

    public void RemoveStatusEffect(StatusEffect statusEffect, Combatant combatant) {
        // If this combatant lost an effect
        if (this.combatant == combatant) {
            // Search through UIs to remove status effect
            foreach (var statusEffectUI in statusEffectUIs) {
                // If effect was found
                if (statusEffectUI.GetStatusEffect() == statusEffect) {
                    // Uninitialize
                    statusEffectUI.Uninitialize();

                    // Remove reference
                    statusEffectUIs.Remove(statusEffectUI);

                    // Then destroy
                    Destroy(statusEffectUI.gameObject);

                    // Dip
                    return;
                }
            }
        }
        
        
    }
}
