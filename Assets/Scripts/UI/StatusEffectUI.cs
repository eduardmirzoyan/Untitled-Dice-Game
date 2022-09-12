using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image statusEffectIcon;
    [SerializeField] private TextMeshProUGUI stacksText;
    [SerializeField] private TooltipTrigger tooltipTrigger;
    [SerializeField] private Animator spriteAnimator;
    [SerializeField] private Animator stackAnimator;

    [Header("Status Effect")]
    [SerializeField] private StatusEffect statusEffect;

    private void Awake() {
        statusEffectIcon = GetComponentInChildren<Image>();
        stacksText = GetComponentInChildren<TextMeshProUGUI>();
        tooltipTrigger = GetComponentInChildren<TooltipTrigger>();
    }

    public void Initialize(StatusEffect statusEffect) {
        this.statusEffect = statusEffect;

        statusEffectIcon.sprite = statusEffect.icon;
        tooltipTrigger.SetTooltip(statusEffect.name, statusEffect.description);

        // Show stacks if more than 1
        if (statusEffect.stacks > 1) {
            stacksText.text = statusEffect.stacks.ToString();
        } 
        else {
            stacksText.text = "";
        }

        // Play animation
        spriteAnimator.Play("Proc");
        stackAnimator.Play("Update");

        // Sub to events
        CombatEvents.instance.onEffectProc += OnEffectProc;
        CombatEvents.instance.onEffectUpdate += OnEffectUpdate;
    }

    public StatusEffect GetStatusEffect() {
        return statusEffect;
    }

    public void Uninitialize() {
        // Unsub to events
        CombatEvents.instance.onEffectProc -= OnEffectProc;
        CombatEvents.instance.onEffectUpdate -= OnEffectUpdate;
    }

    private void OnEffectProc(StatusEffect statusEffect) {
        if (this.statusEffect == statusEffect) {
            // Play animation
            spriteAnimator.Play("Proc");
        }
    }

    private void OnEffectUpdate(StatusEffect statusEffect) {
        if (this.statusEffect == statusEffect) {
            // Play animation
            stackAnimator.Play("Update");

            // Update stacks
            if (statusEffect.stacks > 1) {
                stacksText.text = statusEffect.stacks.ToString();
            } 
            else {
                stacksText.text = "";
            }
        }
    }
}
