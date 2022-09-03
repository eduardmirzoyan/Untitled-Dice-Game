using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionEvents : MonoBehaviour
{
    public static SelectionEvents instance;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public event Action<Unit, int> onAddUnitToParty;
    public event Action<List<Unit>> onDisplayUnitOptions;
    public event Action<bool> onPartyFull;

    public void TriggerOnAddUnitToParty(Unit unit, int index) {
        if (onAddUnitToParty != null) {
            onAddUnitToParty(unit, index);
        }
    }

    public void TriggerOnDisplayUnitOptions(List<Unit> units) {
        if (onDisplayUnitOptions != null) {
            onDisplayUnitOptions(units);
        }
    }

    public void TriggerOnPartyFull(bool state) {
        if (onPartyFull != null) {
            onPartyFull(state);
        }
    }
}
