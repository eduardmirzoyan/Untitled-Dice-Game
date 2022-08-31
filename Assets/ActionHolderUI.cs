using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ActionInspectUI))]
public class ActionHolderUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image actionIcon;
    [SerializeField] private Action action;
    [SerializeField] private float highlightAlpha = 0.5f;
    [SerializeField] private DiceUI containedDiceUI;
    [SerializeField] private bool isFunctional;
    [SerializeField] private ActionInspectUI actionInspectUI;
    
    [Header("Preview Compoenets")]
    [SerializeField] private GameObject actionUIPrefab;
    private ActionUI actionUI;

    private void Awake() {
        actionInspectUI = GetComponent<ActionInspectUI>();
    }

    public void Initialize(Action action, bool isFunctional = true) {
        this.action = action;
        this.isFunctional = isFunctional;
        actionIcon.sprite = action.icon;
        actionInspectUI.Initalize(action);

        if (isFunctional) {
            CombatEvents.instance.onDieInsert += OnDieInsert;
            CombatEvents.instance.onActionConfirm += onActionConfirm;
        }
            
    }

    public void OnDrop(PointerEventData eventData)
    {
        // If the dropped data is a die
        // Make sure there isnt already a die in this slot
        if (isFunctional && eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI) && containedDiceUI == null) {
            var dice = diceUI.GetDie();

            // Check for any constraints on the die
            if (action.checkDieConstraints(dice)) {
                // Stores UI
                containedDiceUI = diceUI;

                // Sets a new parent for the dice to snap back to
                diceUI.SetParent(gameObject.transform);

                // Update visuals
                actionIcon.color = new Color(255, 255, 255, 1);

                // Select this
                CombatEvents.instance.TriggerDieInsert(action, dice);
            }
            else {
                print("Dice does not pass contraints of selected action");
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // If a die is hovered over, then reduce alpha
        if (isFunctional && eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI)) {
            actionIcon.color = new Color(255, 255, 255, highlightAlpha);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Make icon fully opaque if a point exits
        actionIcon.color = new Color(255, 255, 255, 1);
    }

    private void OnDieInsert(Action action, Dice dice) {
        // This means that die was inserted into a different holder
        if (this.action != action) {
            // If this slot has a die, reset it, PROBLEM HERE
            if (containedDiceUI != null) {
                // If the holder is not this one, but had the same die previous, remove referece, but don't reset
                if (containedDiceUI.GetDie() != dice) {
                    containedDiceUI.ResetLocation();
                }
                containedDiceUI = null;
            }
        }
    }

    private void onActionConfirm(Action action) {
        // If this action was confirmed, return die as unactive
        if (this.action == action) {
            if (containedDiceUI != null) {
                // Set die inactive and reset it
                containedDiceUI.SetActive(false);
                containedDiceUI.ResetLocation();
                containedDiceUI = null;
            }
            else { throw new System.Exception("CONFIRMED ACTION HAD NO DIE??!"); }
        }
    }
}
