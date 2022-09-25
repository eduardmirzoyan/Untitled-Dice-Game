using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemberHolderUI : UnitHolderUI
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private CanvasGroup coverCanvasGroup;

    [Header("Data")]
    [SerializeField] private int index;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;

    private void Start() {
        GameEvents.instance.onDeployUnit += Initialize;
    }

    private void OnDestroy() {
        GameEvents.instance.onDeployUnit -= Initialize;
    }

    public override void Initialize(Unit unit)
    {
        // Spawn model
        unitUI = Instantiate(unitUIPrefab, unitModelTransform).GetComponent<UnitUI>();
        // Initialize
        unitUI.Initialize(unit, this, unitModelTransform);

        // Trigger event
        GameEvents.instance.TriggerOnAddUnitToParty(unit, index);
    }

    public void Initialize(Unit unit, int index)
    {
        if (this.index == index) {
            // Spawn model
            unitUI = Instantiate(unitUIPrefab, unitModelTransform).GetComponent<UnitUI>();
            // Initialize
            unitUI.Initialize(unit, this, unitModelTransform);

            // Enable Transform
            unitModelTransform.gameObject.SetActive(true);

            // Disable cover
            coverCanvasGroup.alpha = 0f;
            coverCanvasGroup.interactable = false;
            coverCanvasGroup.blocksRaycasts = false;

            // Trigger event
            GameEvents.instance.TriggerOnAddUnitToParty(unit, index); 
        }
    }

    protected override void Highlight(bool enable)
    {
        if (enable) {
            unitName.color = highlightColor;
        }
        else {
            print(unitUI);
            unitName.color = defaultColor;
        }
    }

    protected override void StoreUnit(UnitUI unitUI)
    {
        if (unitUI != null)
        {
            // Debugging
            print("Party member " + unitUI.name + " has inserted into slot: " + name);

            // Enable Transform
            unitModelTransform.gameObject.SetActive(true);

            // Disable cover
            coverCanvasGroup.alpha = 0f;
            coverCanvasGroup.interactable = false;
            coverCanvasGroup.blocksRaycasts = false;

            // Change parents of UI
            unitUI.SetParent(unitModelTransform);
            unitUI.SetHolder(this);

            // Update party
            GameManager.instance.SetPlayerParty(unitUI.GetUnit(), index);

            // Trigger event
            GameEvents.instance.TriggerOnAddUnitToParty(unitUI.GetUnit(), index);
        }
        else
        {
            // Debugging
            print("Party member was removed from slot: " + name);

            // Disable Transform
            unitModelTransform.gameObject.SetActive(false);

            // Enable cover
            coverCanvasGroup.alpha = 1f;
            coverCanvasGroup.interactable = true;
            coverCanvasGroup.blocksRaycasts = true;

            // Update party
            GameManager.instance.SetPlayerParty(null, index);

            // Trigger event
            GameEvents.instance.TriggerOnAddUnitToParty(null, index);
        }

        this.unitUI = unitUI;
    }
}
