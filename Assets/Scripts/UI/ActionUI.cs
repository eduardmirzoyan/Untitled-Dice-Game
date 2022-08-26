using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text text;
    [SerializeField] private DiceSlotUI diceSlotUI;
    [SerializeField] private Image borderImage;

    [Header("Contained Action")]
    [SerializeField] private Action action;
    [SerializeField] private Weapon sourceWeapon;

    private void Awake() {
        diceSlotUI = GetComponentInChildren<DiceSlotUI>();   
    }

    public void Initialize(Action action, Weapon sourceWeapon) {
        this.action = action;
        this.sourceWeapon = sourceWeapon;
        UpdateText();
    }


    private void UpdateText() {
        if (diceSlotUI.ContainsDie()) {
            text.text = "Dice value: " + diceSlotUI.GetContainedDice().GetValue() + " With action: " + action.name;
        }
        else {
            text.text = action.name + " from: " + sourceWeapon.weaponName;
        }
    }

    private void UpdateVisuals() {
        if (diceSlotUI.ContainsDie()) {
            borderImage.color = Color.yellow;
        }
        else {
            borderImage.color = Color.white;
        }
    }

    public Action GetAction() {
        return action;
    }

    public Weapon GetWeapon() {
        return sourceWeapon;
    }

    public bool ContainsDie() {
        return diceSlotUI.ContainsDie();
    }

    public void DeactivateDie() {
        diceSlotUI.GetDiceUI().SetActive(false);
    }

    public void DeSelect() {
        // Remove inserted die if possible
        diceSlotUI.RemoveInsertedDie();

        // Update visuals
        UpdateText();
        UpdateVisuals();
    }

    public void Update() {
        UpdateText();
        UpdateVisuals();
    }

}
