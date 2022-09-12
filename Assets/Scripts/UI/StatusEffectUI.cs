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

    [Header("Status Effect")]
    [SerializeField] private StatusEffect statusEffect;

    private void Awake() {
        statusEffectIcon = GetComponentInChildren<Image>();
        stacksText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(StatusEffect statusEffect) {
        this.statusEffect = statusEffect;

        statusEffectIcon.sprite = statusEffect.icon;

        // Show stacks if more than 1
        if (statusEffect.stacks > 1) {
            stacksText.text = statusEffect.stacks.ToString();
        } 
        else {
            stacksText.text = "";
        }
    }
}
