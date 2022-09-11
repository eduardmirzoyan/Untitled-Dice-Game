using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmActionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button button;
    
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
    }

    private void Start() {
        CombatEvents.instance.onPlayerTurnStart += Show;
        CombatEvents.instance.onTargetSelect += ChangeState;
        CombatEvents.instance.onActionConfirm += Hide;
    }

    private void Show(int value) {
        button.interactable = false;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ChangeState(Combatant combatant) {
        // Set button to be interactable based on if a valid target was chosen
        if (combatant != null) {
            print("Confirm is active");
            button.interactable = true;
        }
        else {
            print("Confirm is NOT active");
            button.interactable = false;
        }
    }

    public void Hide(Action action) {
        button.interactable = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
