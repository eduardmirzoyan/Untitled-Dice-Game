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

    public event Action<List<Unit>> onDisplayUnitOptions;
    public event Action<Item, int> onAddItemToStorage;

    public void TriggerOnAddItemToStorage(Item item, int index) {
        if (onAddItemToStorage != null) {
            onAddItemToStorage(item, index);
        }
    }

    public void TriggerOnDisplayUnitOptions(List<Unit> units) {
        if (onDisplayUnitOptions != null) {
            onDisplayUnitOptions(units);
        }
    }
}
