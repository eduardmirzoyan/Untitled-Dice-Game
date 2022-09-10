using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStationUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Combatant combatant;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Initialize(Combatant combatant) {
        this.combatant = combatant;

        // Sub to events
        CombatEvents.instance.onActionSelect += OnActionSelect;
        CombatEvents.instance.onTargetSelect += OnTargetSelect;
        CombatEvents.instance.onActionConfirm += OnActionConfirm;
    }

    private void OnActionSelect(Action action, Dice dice) {
        // If an action is selected, and this slot is a possible target
        
        // If not action was selected, then remove animation
        if (action == null) {
            animator.Play("Idle");
            return;
        }
        
        // Else check if this combatant is a valid target
        if (action.canTargetSelf && CombatManager.instance.currentCombatant == this.combatant) {
            animator.Play("Highlight");
        }
        else if (action.canTargetEnemies && CombatManager.instance.currentCombatant.isAlly() && !this.combatant.isAlly()) {
            animator.Play("Highlight");
        }
        else if (action.canTargetAllies && CombatManager.instance.currentCombatant.isAlly() && this.combatant.isAlly()) {
            animator.Play("Highlight");
        }
        else {
            animator.Play("Idle");
        }
    }

    private void OnTargetSelect(Combatant combatant) {
        
        // If this combatant was not selected, then go back to idle
        if (this.combatant != combatant) {
            animator.Play("Idle");
        }
        else {
            // Play target animation
            animator.Play("Target");
        }   
    }

    private void OnActionConfirm(Action action) {
        // When an action is confirmed, go back to idle
        animator.Play("Idle");
    }
}
