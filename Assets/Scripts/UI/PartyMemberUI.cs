using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartyMemberUI : MonoBehaviour
{

    [Header("Displaying Components")]
    [SerializeField] private Text displayName;
    [SerializeField] private Image icon;
    [SerializeField] private Slider healthBar;
    [SerializeField] private DiceUI diceUI;
    [SerializeField] private Text speedStat;
    [SerializeField] private RectTransform weapon1Transform;
    [SerializeField] private RectTransform weapon2Transform;
    
    [Header("Temporary")]
    [SerializeField] private GameObject itemPrefab;
    
    // Unit name
    [SerializeField] private Unit unit;

    public void Initialize(Unit unit) {
        this.unit = unit;
        UpdateVisuals();
    }

    public void UpdateVisuals() {
        // Make sure a unit exists
        if (unit == null) {
            throw new System.Exception("No unit in this slot. " + gameObject.name);
        }

        // Update display name
        displayName.text = unit.name;

        // Update icon
        icon.sprite = unit.icon;
        icon.color = Color.white;

        // Update healthbar
        healthBar.maxValue = unit.maxHealth;
        healthBar.value = unit.currentHealth;

        // Update die
        diceUI.DisplayValue(unit.dice.maxValue);

        // Update speed stat
        speedStat.text = unit.speed.ToString();

        // Update equipped weapons
        if (unit.weapons[0] != null) {
            var itemUI = Instantiate(itemPrefab, weapon1Transform).GetComponent<ItemUI>();
            itemUI.Initialize(unit.weapons[0]);
        }
        if (unit.weapons[1] != null) {
            var itemUI = Instantiate(itemPrefab, weapon2Transform).GetComponent<ItemUI>();
            itemUI.Initialize(unit.weapons[1]);
        }

        // Update equipped armors
        // TODO
    }


}
