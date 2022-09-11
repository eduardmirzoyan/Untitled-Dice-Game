using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HolderHolder : MonoBehaviour
{
    [SerializeField] private GameObject unitHolderPrefab;
    [SerializeField] private LayoutGroup layoutGroup;

    private void Start() {
        SelectionEvents.instance.onDisplayUnitOptions += DisplayUnits;
    }

    public void DisplayUnits(List<Unit> units) {
        print("Displaying copies!");
        foreach (var unit in units) {
            var holder = Instantiate(unitHolderPrefab, layoutGroup.transform).GetComponent<UnitHolderUI>();
            // Make copies of units instead of themselves
            holder.Initialize(unit.Copy());
        }
    }
}
