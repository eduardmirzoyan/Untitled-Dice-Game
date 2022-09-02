using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PartyMemberUI : MonoBehaviour, IDropHandler
{

    [Header("Displaying Components")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private Image unitIcon;
    [SerializeField] private DiceUI diceUI;
    [SerializeField] private TextMeshProUGUI healthStat;
    [SerializeField] private TextMeshProUGUI speedStat;
    [SerializeField] private RectTransform weapon1Transform;
    [SerializeField] private RectTransform weapon2Transform;
    [SerializeField] private RectTransform unitModelTransform;
    [SerializeField] private GameObject dropObject;
    
    [Header("Temporary")]
    [SerializeField] private GameObject itemPrefab;
    
    // Unit name
    [SerializeField] private Unit unit;

    public void Initialize(Unit unit) {
        this.unit = unit;
        UpdateVisuals();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && unit == null && eventData.pointerDrag.TryGetComponent(out UnitUI unitUI))
        {
            unitUI.SetParent(unitModelTransform);
            Initialize(unitUI.GetUnit());
        }
    }

    public void UpdateVisuals() {
        // Make sure a unit exists
        if (unit == null) {
            // throw new System.Exception("No unit in this slot. " + gameObject.name);
            // Set to default values
            // Update display name
            unitName.text = "[EMPTY]";

            dropObject.SetActive(true);

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

        // Update display name
        unitName.text = unit.name;

        dropObject.SetActive(false);

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
            print("armro!!");
            var itemUI = Instantiate(itemPrefab, weapon2Transform).GetComponent<ItemUI>();
            itemUI.Initialize(unit.armors[0]);
        }
    }


}
