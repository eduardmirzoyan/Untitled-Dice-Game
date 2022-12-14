using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button button;
    
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
    }

    private void Start() {
        CombatEvents.instance.onPlayerTurnStart += Show;
        CombatEvents.instance.onActionConfirm += Hide;
    }

    private void Show(int value) {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide(Combatant source, Action action, Dice dice, Combatant target) {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
