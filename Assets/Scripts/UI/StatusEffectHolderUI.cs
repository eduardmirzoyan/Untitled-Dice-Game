using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectHolderUI : MonoBehaviour
{

    [SerializeField] private Combatant combatant;
    [SerializeField] private GameObject statusEffectUIPrefab;
    [SerializeField] private Transform statusEffectHolder;

    private List<StatusEffectUI> statusEffectUIs;

    private void Awake() {
        statusEffectUIs = new List<StatusEffectUI>();
    }

    public void AddStatusEffect(StatusEffect statusEffect) {
        // TODO

        // Create status effect visual
        var statusUI = Instantiate(statusEffectUIPrefab, statusEffectHolder).GetComponent<StatusEffectUI>();
        statusUI.Initialize(statusEffect);
    }

    public void RemoveStatusEffect(StatusEffectUI statusEffectUI) {
        // TODO
        
        // Search through UIs are remove status effect
    }
}
