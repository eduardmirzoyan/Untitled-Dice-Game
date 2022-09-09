using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDisplaySlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private Image icon;
    [SerializeField] private DiceUI containedDiceUI;

    [Header("States")]
    [SerializeField] private Action action;
    [SerializeField] private Passive passive;
    [SerializeField] private float highlightAlpha = 0.5f;
    [SerializeField] private bool isFunctional;


    public void Initialize(Action action, bool isFunctional = true) {
        this.action = action;
        this.isFunctional = isFunctional;
        icon.sprite = action.icon;

        if (isFunctional) {
            CombatEvents.instance.onDieInsert += OnDieInsert;
            CombatEvents.instance.onPreActionConfirm += ReleaseDieIfPossible;
        }
    }

    public void Deinitialize() {
        if (isFunctional) {
            CombatEvents.instance.onDieInsert -= OnDieInsert;
            CombatEvents.instance.onPreActionConfirm -= ReleaseDieIfPossible;
        }
    }

    public void Initialize(Passive passive, bool isFunctional = true) {
        this.passive = passive;
        this.isFunctional = false;
        icon.sprite = passive.icon;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // If the dropped data is a die
        // Make sure there isnt already a die in this slot
        if (isFunctional && eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI) && containedDiceUI == null) {
            var dice = diceUI.GetDie();

            // Make sure die is active
            if (dice.isExhausted) {
                CombatEvents.instance.TriggerOnFeedback("Must use NON-EXHAUSTED die.");
                return;
            }

            // Check for any constraints on the die
            if (!dice.isExhausted && action.checkDieConstraints(dice)) {
                // Stores UI
                containedDiceUI = diceUI;

                // Sets a new parent for the dice to snap back to
                diceUI.SetParent(gameObject.transform);

                // Update visuals
                icon.color = new Color(255, 255, 255, 1);

                // Select this
                CombatEvents.instance.TriggerOnDieInsert(action, dice);
            }
            else {
                print("Dice does not pass contraints of selected action");
            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If left click
        if (eventData.button == PointerEventData.InputButton.Left) {
            // Lock tooltip if possible
            SkillTooltipUI.instance.Lock();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // If this is not functional, then dip
        if (!isFunctional) return;

        // If a die is hovered over, then reduce alpha
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI)) {
            icon.color = new Color(255, 255, 255, highlightAlpha);
        }

        // Show tooltip
        if (action != null) {
            SkillTooltipUI.instance.Show(action, transform.position);
        }
        else if (passive != null) {
            SkillTooltipUI.instance.Show(passive, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Make icon fully opaque if a point exits
        icon.color = new Color(255, 255, 255, 1);
        
        // Hide tooltip
        SkillTooltipUI.instance.Hide();
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

    private void ReleaseDieIfPossible(Action action) {
        // If this action was confirmed, return die as unactive
        if (this.action == action) {
            if (containedDiceUI != null) {
                // Reset die
                containedDiceUI.ResetLocation();
                containedDiceUI = null;
            }
            else { throw new System.Exception("CONFIRMED ACTION HAD NO DIE??!"); }
        }
    }
}
