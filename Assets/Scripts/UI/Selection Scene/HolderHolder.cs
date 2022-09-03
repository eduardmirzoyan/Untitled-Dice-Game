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
        foreach (var unit in units) {
            var holder = Instantiate(unitHolderPrefab, layoutGroup.transform).GetComponent<UnitHolderUI>();
            holder.Initialize(unit);
        }
    }
}
