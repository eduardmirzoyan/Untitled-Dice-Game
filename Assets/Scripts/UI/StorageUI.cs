using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    [SerializeField] private List<ItemSlotUI> itemSlotUIs;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private SlidingWindow slidingWindow;

    private void Start() {
        SelectionEvents.instance.onAddItemToStorage += SpawnItem;

        // Raise on spawn
        slidingWindow.Raise();
    }

    private void SpawnItem(Item item, int index) {
        // Spawn the item
        var itemUI = Instantiate(itemPrefab, itemSlotUIs[index].transform).GetComponent<ItemUI>();
        itemUI.Initialize(item, itemSlotUIs[index]);
    }
}
