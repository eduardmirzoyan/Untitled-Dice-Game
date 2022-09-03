using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitHolderUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject unitUIPrefab;
    [SerializeField] private UnitUI unitUI;
    [SerializeField] private RectTransform unitModelTransform;

    public void Initialize(Unit unit) {
        // Spawn model
        unitUI = Instantiate(unitUIPrefab, unitModelTransform).GetComponent<UnitUI>();
        unitUI.Initialize(unit, unitModelTransform);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Make sure there already isn't an item and it is an itemUI
        if (eventData.pointerDrag != null && unitUI == null && eventData.pointerDrag.TryGetComponent(out UnitUI newUnitUI))
        {
            newUnitUI.SetParent(unitModelTransform);
            unitUI = newUnitUI;
        }
    }
}
