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
        CombatEvents.instance.onPlayerTurnEnd += Hide;
    }

    private void Show(int value) {
        
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ChangeState(Combatant combatant) {
        // print("Change : " +combatant.name);

        button.interactable = combatant != null;
    }

    public void Hide(int value) {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}