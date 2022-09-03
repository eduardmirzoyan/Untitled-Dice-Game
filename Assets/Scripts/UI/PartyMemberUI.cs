using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PartyMemberUI : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Displaying Components")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private int index;
    [SerializeField] private Image dropIcon;
    [SerializeField] private DiceUI diceUI;
    [SerializeField] private TextMeshProUGUI healthStat;
    [SerializeField] private TextMeshProUGUI speedStat;
    [SerializeField] private RectTransform weapon1Transform;
    [SerializeField] private RectTransform weapon2Transform;
    [SerializeField] private RectTransform unitModelTransform;
    [SerializeField] private GameObject dropObject;
    
    [Header("Temporary")]
    [SerializeField] private GameObject itemPrefab;

    [Header("Settings")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color fullColor;
    
    // Unit name
    [SerializeField] private UnitUI unitUI;

    private void Start() {
        SelectionEvents.instance.onAddUnitToParty += AddedUnit;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && unitUI == null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            unitUI = newUnitUI;
            unitUI.SetParent(unitModelTransform);

            UpdateVisuals();

            SelectionManager.instance.AddUnitToParty(unitUI.GetUnit(), index);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Return to origin on right click
        if (unitUI != null && eventData.button == PointerEventData.InputButton.Right)
        {
            unitUI.ResetLocation();
            unitUI = null;

            UpdateVisuals();

            SelectionManager.instance.AddUnitToParty(null, index);
        }
    }

    public void RemoveUnit() {
        unitUI.ResetLocation();
        SelectionManager.instance.AddUnitToParty(unitUI.GetUnit(), index);
        unitUI = null;
    }

    private void AddedUnit(Unit unit, int index) {
        if (unitUI != null) {
            // Add unit
            // unitUI = newUnitUI;
            if (unitUI.GetUnit() == unit && this.index != index) {
                unitUI = null;
                UpdateVisuals();
            }
        }
        
    }

    public void UpdateVisuals() {
        
        // Make sure a unit exists
        if (unitUI == null) {
            // throw new System.Exception("No unit in this slot. " + gameObject.name);
            // Set to default values
            // Update display name
            unitName.text = "[EMPTY]";

            dropIcon.color = emptyColor;
            dropIcon.enabled = true;

            // Update icon
            //unitIcon.sprite = unit.icon;
            //unitIcon.color = Color.white;

            // Update health
            healthStat.text = "0/0";

            // Update die
            diceUI.DrawValue(1);

            // Update speed stat
            speedStat.text = "" + 0;

            return;
        }

        Unit unit = unitUI.GetUnit();

        // Update display name
        unitName.text = unit.name;

        dropIcon.enabled = false;

        // Update icon
        //unitIcon.sprite = unit.icon;
        //unitIcon.color = Color.white;

        // Update health
        healthStat.text = unit.GetHealthStatus();

        // Update die
        diceUI.DrawValue(unit.dice.maxValue);

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
        if (unit.armors != null && unit.armors.Length > 0 && unit.armors[0] != null) {
            var itemUI = Instantiate(itemPrefab, weapon2Transform).GetComponent<ItemUI>();
            itemUI.Initialize(unit.armors[0]);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && unitUI == null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI)) {
            unitName.color = highlightColor;
            dropIcon.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (unitUI != null) {
            unitName.color = fullColor;
        }
        else {
            unitName.color = emptyColor;
            dropIcon.color = emptyColor;
        }
    }
}
