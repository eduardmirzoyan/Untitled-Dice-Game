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
    [SerializeField] private RectTransform unitModelTransform;
    [SerializeField] private List<ItemSlotUI> weaponSlots;
    [SerializeField] private List<ItemSlotUI> armorSlots;
    
    [Header("Temporary")]
    [SerializeField] private GameObject itemPrefab;

    [Header("Settings")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color fullColor;
    
    // Unit name
    [SerializeField] private UnitUI unitUI;
    private bool interact;

    private void Start() {
        SelectionEvents.instance.onAddUnitToParty += AddUnit;
        SelectionEvents.instance.onItemInsertIntoSlot += HandleItemInsertedIntoItemSlot;
        
        SelectionEvents.instance.onFillParty += FillWithUnit;
    }

    public void FillWithUnit(Unit unit, int index) {
        if (this.index != index) return;

        var ui = Instantiate(GameManager.instance.unitUIprefab, unitModelTransform).GetComponent<UnitUI>();
        ui.Initialize(unit, index, unitModelTransform, false);
        EquipUnitUI(ui);
        interact = false;
    }

    public void EquipUnitUI(UnitUI newUnitUI) {
        unitUI = newUnitUI;
        unitUI.SetParent(unitModelTransform);
        unitUI.SetIndex(index);

        UpdateVisuals();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && unitUI == null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            interact = true;
            EquipUnitUI(newUnitUI);

            SelectionManager.instance.AddUnitToParty(unitUI.GetUnit(), index);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Return to origin on right click
        // if (interact && unitUI != null && eventData.button == PointerEventData.InputButton.Right) {
        //     unitUI.ResetLocation();
        //     unitUI = null;

        //     UpdateVisuals();

        //     SelectionManager.instance.AddUnitToParty(null, index);
        // }
    }


    private void HandleItemInsertedIntoItemSlot(ItemUI itemUI, ItemSlotUI itemSlotUI) {
        // Check if item is weapon
        int wpnIndex = weaponSlots.IndexOf(itemSlotUI);
        if (wpnIndex != -1) {
            // Item wants to be removed
            if (itemUI == null) {
                unitUI.GetUnit().EquipWeapon(null, wpnIndex);
                return;
            }

            var weapon = (Weapon) itemUI.GetItem();

            // Equip weapon logic
            unitUI.GetUnit().EquipWeapon(weapon, wpnIndex);
            return;
        }

        // Check if item is armor
        int amrIndex = armorSlots.IndexOf(itemSlotUI);
        if (amrIndex != -1) {

            // Item wants to be removed
            if (itemUI == null) {
                unitUI.GetUnit().EquipArmor(null, amrIndex);
                return;
            }

            var armor = (Armor) itemUI.GetItem();

            // Equip weapon logic
            unitUI.GetUnit().EquipArmor(armor, amrIndex);
            return;
        }

        // Else we don't care
    }


    private void AddUnit(Unit unit, int index) {

        if (this.index == index) {
            if (unit == null) {
                unitUI = null;
                UpdateVisuals();
            }
        }
        else if (unitUI != null && unitUI.GetUnit() == unit) {
            unitUI = null;
            UpdateVisuals();
        }
        
    }

    public void UpdateVisuals() {
        
        // Make sure a unit exists
        if (unitUI == null) {
            // Set to default values

            // Update display name
            unitName.text = "[EMPTY]";
            unitName.color = emptyColor;

            // Show "drop here" icon
            dropIcon.color = emptyColor;
            dropIcon.enabled = true;

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
        unitName.color = fullColor;

        // Hide "drop here" icon
        dropIcon.enabled = false;

        // Update health
        healthStat.text = unit.GetHealthStatus();

        // Update die
        diceUI.DrawValue(unit.dice.maxValue);

        // Update speed stat
        speedStat.text = unit.speed.ToString();

        // Check for weapons
        for (int i = 0; i < weaponSlots.Count; i++) {
            if (unit.weapons[i] != null) {
                var itemUI = Instantiate(itemPrefab, weaponSlots[i].transform).GetComponent<ItemUI>();
                itemUI.Initialize(unit.weapons[i], weaponSlots[i]);
            }
        }
        
        // Check for armor
        for (int i = 0; i < armorSlots.Count; i++) {
            if (unit.armors[i] != null) {
                var itemUI = Instantiate(itemPrefab, armorSlots[i].transform).GetComponent<ItemUI>();
                itemUI.Initialize(unit.armors[i], armorSlots[i]);
            }
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
