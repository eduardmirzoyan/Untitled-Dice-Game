using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PartyMemberUI : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private int index;
    [SerializeField] private DiceUI diceUI;
    [SerializeField] private TextMeshProUGUI healthStat;
    [SerializeField] private TextMeshProUGUI speedStat;
    [SerializeField] private RectTransform unitModelTransform;
    [SerializeField] private SlidingWindow slidingWindow;

    [Header("Data")]
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

        GameEvents.instance.onAddUnitToParty += DisplayStats;
        //GameEvents.instance.onDeployUnit += DeployUnit;
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

        GameEvents.instance.onAddUnitToParty -= DisplayStats;
        //GameEvents.instance.onDeployUnit -= DeployUnit;
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

    private void DeployUnit(Unit unit, int index) 
    {
        // TODO
        if (this.index == index) {
            // Create Unit UI
            var unitUI = Instantiate(GameManager.instance.unitUIprefab, unitModelTransform).GetComponent<UnitUI>();
            unitUI.Initialize(unit, null, unitModelTransform, false);

            // Cache it
            this.unitUI = unitUI;
            this.unitUI.SetParent(unitModelTransform);
            isInteractable = false;

            // Display stats
            // DisplayStats(unitUI.GetUnit(), index);

            // Trigger event
            GameEvents.instance.TriggerOnAddUnitToParty(unitUI.GetUnit(), index);
        }
    }

    private void DisplayStats(Unit unit, int index) {
        if (this.index == index) {
            if (unit != null) {
                // Update name
                unitName.text = unit.name;
                // unitName.color = fullColor;

                // Update health
                healthStat.text = unit.GetHealthStatus();

                // Update die
                diceUI.DrawValue(unit.dice.maxValue);

                // Update speed stat
                speedStat.text = unit.speed.ToString();
            }
            else {
                // Update name
                unitName.text = "[Empty]";

                // Update health
                healthStat.text = "0/0";

                // Update die
                diceUI.DrawValue(1);

                // Update speed stat
                speedStat.text = "" + 0; 
            }
        }
    }
}
