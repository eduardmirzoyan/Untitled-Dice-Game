using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Combatant combatant;

    // Stores entity its tracking
    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        // Sub to relavent events
        CombatEvents.instance.onTakeDamage += UpdateHealth;
        CombatEvents.instance.onHeal += UpdateHealth;
    }

    public void Initialize(Combatant combatant) {
        this.combatant = combatant;

        slider.maxValue = combatant.unit.maxHealth;
        slider.value = combatant.unit.currentHealth;
        text.text = combatant.unit.GetHealthStatus();
    }

    public void UpdateHealth(Combatant combatant, int amount) {
        if (this.combatant == combatant) {
            // Update Heal values
            slider.maxValue = combatant.unit.maxHealth;
            slider.value = combatant.unit.currentHealth;
            text.text = combatant.unit.GetHealthStatus();
        }
    }

}
