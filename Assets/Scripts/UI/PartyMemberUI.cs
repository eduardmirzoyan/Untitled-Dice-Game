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
    [SerializeField] private Image dropArrowIcon;
    [SerializeField] private DiceUI diceUI;
    [SerializeField] private TextMeshProUGUI healthStat;
    [SerializeField] private TextMeshProUGUI speedStat;
    [SerializeField] private RectTransform unitModelTransform;
    [SerializeField] private SlidingWindow slidingWindow;

    [Header("Data")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color fullColor;

    // Unit name
    [SerializeField] private UnitUI unitUI;
    private bool isInteractable;

    private void Start()
    {
        // Check if we are in selection
        if (SelectionEvents.instance != null)
        {
            SelectionEvents.instance.onDisplayUnitOptions += OnDisplay;
        }
        // Check if we are in combat
        else if (CombatEvents.instance != null)
        {
            CombatEvents.instance.onCombatStart += OnCombatStart;
        }

        // SUB!
        GameEvents.instance.onDeployUnit += DeployUnit;
    }

    private void OnCombatStart(int value)
    {
        // Bring into view
        slidingWindow.Raise();
    }

    private void OnDisplay(List<Unit> units)
    {
        // Bring into view
        slidingWindow.Raise();
    }

    private void OnDestroy()
    {
        // Unsub
        if (SelectionEvents.instance != null)
        {
            SelectionEvents.instance.onDisplayUnitOptions -= OnDisplay;
        }
        else if (CombatEvents.instance != null)
        {
            CombatEvents.instance.onCombatStart -= OnCombatStart;
        }
        
        GameEvents.instance.onDeployUnit -= DeployUnit;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && unitUI == null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            // Add unit
            AddUnit(newUnitUI);

            // Update party
            GameManager.instance.SetPlayerParty(newUnitUI.GetUnit(), index);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO?
    }

    private void AddUnit(UnitUI unitUI) {
        this.unitUI = unitUI;
        this.unitUI.SetParent(unitModelTransform);
        this.unitUI.SetIndex(index);
        isInteractable = true;

        UpdateVisuals();

        // Trigger event
        GameEvents.instance.TriggerOnAddUnitToParty(unitUI.GetUnit(), index);
    }

    private void DeployUnit(Unit unit, int index)
    {
        if (this.index == index) {
            var unitUI = Instantiate(GameManager.instance.unitUIprefab, unitModelTransform).GetComponent<UnitUI>();
            unitUI.Initialize(unit, index, unitModelTransform, false);

            AddUnit(unitUI);
        }
    }

    public void UpdateVisuals()
    {
        // Make sure a unit exists
        if (unitUI == null)
        {
            // Set to default values

            // Update display name
            unitName.text = "[EMPTY]";
            unitName.color = emptyColor;

            // Show "drop here" icon
            dropIcon.color = emptyColor;
            dropArrowIcon.color = emptyColor;
            dropIcon.enabled = true;
            dropArrowIcon.enabled = true;

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

        if (dropIcon == null)
        {
            print("null for " + gameObject.name);
        }

        // Hide "drop here" icon
        dropIcon.enabled = false;
        dropArrowIcon.enabled = false;

        // Update health
        healthStat.text = unit.GetHealthStatus();

        // Update die
        diceUI.DrawValue(unit.dice.maxValue);

        // Update speed stat
        speedStat.text = unit.speed.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && unitUI == null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            unitName.color = highlightColor;
            dropIcon.color = highlightColor;
            dropArrowIcon.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (unitUI != null)
        {
            unitName.color = fullColor;
        }
        else
        {
            unitName.color = emptyColor;
            dropIcon.color = emptyColor;
            dropArrowIcon.color = emptyColor;
        }
    }
}
