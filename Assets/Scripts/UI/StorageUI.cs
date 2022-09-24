using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    [SerializeField] private List<ItemSlotUI> itemSlotUIs;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private SlidingWindow slidingWindow;

    private void Start() {
        // Check if we are in selection
        if (SelectionEvents.instance != null) {
            SelectionEvents.instance.onDisplayUnitOptions += OnDisplay;
            SelectionEvents.instance.onAddItemToStorage += SpawnItem;
        }
        // Check if we are in combat
        else if (CombatEvents.instance != null) {
            CombatEvents.instance.onCombatStart += OnCombatStart;
        }
    }

    private void OnCombatStart(int value) {
        // Bring into view
        slidingWindow.Raise();
    }

    private void OnDisplay(List<Unit> units) {
        // Bring into view
        slidingWindow.Raise();
    }

    private void SpawnItem(Item item, int index) {
        // Spawn the item
        var itemUI = Instantiate(itemPrefab, itemSlotUIs[index].transform).GetComponent<ItemUI>();
        itemUI.Initialize(item, itemSlotUIs[index]);
        itemSlotUIs[index].StoreItem(itemUI);
    }
}
