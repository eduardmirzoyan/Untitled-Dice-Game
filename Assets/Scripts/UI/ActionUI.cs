using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private TextMeshProUGUI actionDescription;
    [SerializeField] private DiceSlotUI diceSlotUI;
    [SerializeField] private Image borderImage;
    [SerializeField] private Image sourceImage;
    [SerializeField] private Image selfTargetIcon;
    [SerializeField] private Image allyTargetIcon;
    [SerializeField] private Image enemyTargetIcon;

    [Header("Contained Action")]
    [SerializeField] private Action action;
    [SerializeField] private Weapon sourceWeapon;

    private void Awake() {
        diceSlotUI = GetComponentInChildren<DiceSlotUI>();   
    }

    public void JustDisplay(Action action) {
        // Change name and text
        actionName.text = action.name;
        actionDescription.text = action.description;

        // Disable source image
        sourceImage.enabled = false;

        // Toggle icons based on what action can do
        selfTargetIcon.gameObject.SetActive(action.canTargetAllies);
        allyTargetIcon.gameObject.SetActive(action.canTargetAllies);
        enemyTargetIcon.gameObject.SetActive(action.canTargetEnemies);
    }

    public void Initialize(Action action, Weapon sourceWeapon) {
        this.action = action;
        this.sourceWeapon = sourceWeapon;
        UpdateText();
    }


    private void UpdateText() {
        // Update text
        actionName.text = action.name;
        actionDescription.text = action.description;

        // Update source
        if (sourceWeapon != null) {
            sourceImage.enabled = true;
            sourceImage.sprite = sourceWeapon.sprite;
        }
        else {
            sourceImage.enabled = false;
        }

        // Toggle icons based on what action can do
        selfTargetIcon.gameObject.SetActive(action.canTargetAllies);
        allyTargetIcon.gameObject.SetActive(action.canTargetAllies);
        enemyTargetIcon.gameObject.SetActive(action.canTargetEnemies);
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
        //UpdateText();
        //UpdateVisuals();
    }

}
