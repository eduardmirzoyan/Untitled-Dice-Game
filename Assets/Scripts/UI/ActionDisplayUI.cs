using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplayUI : MonoBehaviour
{
    [SerializeField] private SkillDisplayDescriptionUI skillDisplayDescriptionUI;
    [SerializeField] private CanvasGroup canvasGroup;
    private Action selectedAction;

    private void Start() {
        // Sub to event
        CombatEvents.instance.onActionHover += DisplayRawAction;
        CombatEvents.instance.onActionSelect += DisplayDynamicAction;
        CombatEvents.instance.onTargetSelect += DisplayFinalAction;
        CombatEvents.instance.onActionConfirm += DisplayHide;
    }

    private void DisplayRawAction(Action action) {

        // Only display raw if an action hasn't been selected
        if (selectedAction == null) {
            // Display or hide action
            if (action != null) {
                canvasGroup.alpha = 1f;
                skillDisplayDescriptionUI.InitializeRaw(action);
            }
            // If there is no selected Action
            else {
                canvasGroup.alpha = 0f;
            }
        }
        
    }

    private void DisplayDynamicAction(Action action, Dice dice) {
        if (action != null && dice != null) {
            canvasGroup.alpha = 1f;
            skillDisplayDescriptionUI.InitializeDynamic(action, dice);
            selectedAction = action;
        }
        else {
            selectedAction = null;
            canvasGroup.alpha = 0f;
        }
    }

    private void DisplayFinalAction(Combatant combatant) {
        if (selectedAction != null) {
            canvasGroup.alpha = 1f;
            skillDisplayDescriptionUI.InitializeFinal(selectedAction);
        }
        else {
            selectedAction = null;
            canvasGroup.alpha = 0f;
        }
    }

    private void DisplayHide(Combatant source, Action action, Dice dice, Combatant target) {
        // Hide the UI
        selectedAction = null;
        canvasGroup.alpha = 0f;
    }
}
