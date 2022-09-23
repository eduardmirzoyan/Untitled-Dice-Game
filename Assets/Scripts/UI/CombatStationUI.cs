using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStationUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private Animator turnAnimator;
    [SerializeField] private HealthbarUI healthbarUI;
    [SerializeField] private StatusEffectHolderUI statusEffectHolderUI;

    [Header("Data")]
    [SerializeField] private Combatant combatant;

    private void Awake() {
        healthbarUI = GetComponentInChildren<HealthbarUI>();
        statusEffectHolderUI = GetComponentInChildren<StatusEffectHolderUI>();
    }

    public void Initialize(Combatant combatant) {
        this.combatant = combatant;

        // Initialize healthbar
        healthbarUI.Initialize(combatant);

        // Initialize holder
        statusEffectHolderUI.Initialize(combatant);

        // Sub to events
        CombatEvents.instance.onTurnStart += OnTurnStart;
        CombatEvents.instance.onActionSelect += OnActionSelect;
        CombatEvents.instance.onTargetSelect += OnTargetSelect;
        CombatEvents.instance.onActionConfirm += OnActionConfirm;
    }

    private void OnActionSelect(Action action, Dice dice) {
        // If an action is selected, and this slot is a possible target
        
        // If not action was selected, then remove animation
        if (action == null) {
            targetAnimator.Play("Idle");
            return;
        }
        
        // Else check if this combatant is a valid target
        if (action.canTargetSelf && CombatManager.instance.currentCombatant == this.combatant) {
            targetAnimator.Play("Highlight");
        }
        else if (action.canTargetEnemies && CombatManager.instance.currentCombatant.isAlly() && !this.combatant.isAlly()) {
            targetAnimator.Play("Highlight");
        }
        else if (action.canTargetAllies && CombatManager.instance.currentCombatant.isAlly() && this.combatant.isAlly()) {
            targetAnimator.Play("Highlight");
        }
        else {
            targetAnimator.Play("Idle");
        }
    }

    private void OnTargetSelect(Combatant combatant) {
        // If a combatant was selected, but not this one
        if (this.combatant != combatant) {
            targetAnimator.Play("Idle");
        }
        else {
            // Play target animation
            targetAnimator.Play("Select");
        }   
    }

    private void OnActionConfirm(Combatant source, Action action, Dice dice, Combatant target) {
        // Remove any target indicator
        targetAnimator.Play("Idle");

        // Remove any turn indicator
        turnAnimator.Play("Idle");
    }

    private void OnTurnStart(int value) {
        // If it is this unit's turn
        if (CombatManager.instance.currentCombatant == this.combatant) {
            // Show that it is this unit's turn
            turnAnimator.Play("Select");
        }
        else {
            // Or just do nothing
            turnAnimator.Play("Idle");
        }
    }
}
