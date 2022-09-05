using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : ScriptableObject
{
    public List<Item> items;
    public int maxSize = 20;

    public void Initialize(int size) {
        // Create an empty list of size items
        items = new List<Item>();
        maxSize = size;
        for (int i = 0; i < size; i++) {
            items.Add(null);
        }
    }

    public void AddToIndex(Item item, int index) {
        if (items[index] != null) {
            Debug.Log("Tried to add item at an index which already contained an item: " + index);
            return;
        }
        items[index] = item;
    }

    public void RemoveAtIndex(int index) {
        if (items[index] == null) {
            Debug.Log("Tried to remove an item at an index which doesn't contain an item: " + index);
            return;
        }
        items[index] = null;
    }
}
