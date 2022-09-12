using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillDisplaySlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private Animator animator;

    [Header("Uses Info")]
    [SerializeField] private CanvasGroup usesCanvasGroup;
    [SerializeField] private TextMeshProUGUI usesText;

    [Header("Cooldown Info")]
    [SerializeField] private CanvasGroup cooldownCanvasGroup;
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Data")]
    [SerializeField] private Action action;
    [SerializeField] private Passive passive;
    [SerializeField] private Skill skill;
    [SerializeField] private DiceUI containedDiceUI;
    [SerializeField] private float highlightAlpha = 0.5f;
    [SerializeField] private bool isFunctional;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Initialize(Action action, bool isFunctional = true) {
        this.action = action;
        this.isFunctional = isFunctional;
        skillIcon.sprite = action.icon;

        if (isFunctional) {
            CombatEvents.instance.onDieStartDrag += OnDieDragStart;
            CombatEvents.instance.onDieEndDrag += OnDieDragEnd;
            CombatEvents.instance.onActionSelect += OnDieInsert;
            CombatEvents.instance.onPreActionConfirm += ReleaseDieIfPossible;

            // Visuals based on uses
            if (action.hasUses > 0) {
                // If you have no uses left
                if (action.uses == 0) {
                    // Enable canvas group
                    usesCanvasGroup.alpha = 1f;
                }

                // Disply uses left
                usesText.text = action.uses.ToString();
            }
            else {
                usesText.text = "";
            }
            

            // Visuals based on cooldown
            if (action.hasCooldown > 0 && action.cooldown > 0) {
                // Enable canvas group
                cooldownCanvasGroup.alpha = 1f;

                // Disply cooldown
                cooldownText.text = action.cooldown.ToString();
            }
        }
    }

    public void Deinitialize() {
        if (isFunctional) {
            CombatEvents.instance.onDieStartDrag -= OnDieDragStart;
            CombatEvents.instance.onDieEndDrag -= OnDieDragEnd;
            CombatEvents.instance.onActionSelect -= OnDieInsert;
            CombatEvents.instance.onPreActionConfirm -= ReleaseDieIfPossible;
        }
    }

    public void Initialize(Passive passive, bool isFunctional = true) {
        this.passive = passive;
        this.isFunctional = false;
        skillIcon.sprite = passive.icon;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // If the dropped data is a die
        // Make sure there isnt already a die in this slot
        if (isFunctional && eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DiceUI diceUI) && containedDiceUI == null) {
            // Get the die
            var dice = diceUI.GetDie();

            // Make sure die is active
            if (dice.isExhausted) {
                // Give feedback
                CombatEvents.instance.TriggerOnFeedback("Must use NON-EXHAUSTED die.");
                return;
            }

            // Check for any action constraints on the action
            if (!action.CheckActionConstraints()) {
                // Display feedback
                action.ShowActionConstraintFeedback();
                // If contraint then dip
                return;
            }

            // Check for any die constraints on the die
            if (!action.CheckDieConstraints(dice)) {
                // Display feedback
                action.ShowDieConstraintFeedback(dice);
                // If contraint then dip
                return;
            }

            // Else all is good

            // Stores UI
            containedDiceUI = diceUI;

            // Sets a new parent for the dice to snap back to
            diceUI.SetParent(gameObject.transform);

            // Update visuals
            skillIcon.color = new Color(255, 255, 255, 1);

            // Trigger event
            CombatEvents.instance.TriggerOnActionSelect(action, dice);
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
            skillIcon.color = new Color(255, 255, 255, highlightAlpha);
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
        skillIcon.color = new Color(255, 255, 255, 1);
        
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

    private void OnDieDragStart(Dice dice) {
        // Check if the dragged die passes this action's constraints
        if (action != null && action.CheckActionConstraints() && action.CheckDieConstraints(dice)) {
            // Show highlight animation
            animator.Play("Highlight");
        }
    }

    private void OnDieDragEnd(Dice dice) {
        // Stop any animations
        animator.Play("Idle");
    }
}
