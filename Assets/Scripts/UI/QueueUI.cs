using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> queueSlotGameobjects;
    [SerializeField] private GameObject queueSlotPrefab;
    [SerializeField] private HorizontalLayoutGroup queueLayoutGroup;

    [SerializeField] private Text roundCounterText;

    public void Enqueue(Unit unit, Color color) {
        // Create slot
        var go = Instantiate(queueSlotPrefab, queueLayoutGroup.transform);
        // Cache
        queueSlotGameobjects.Add(go);
        // Intialize based on unit
        if (go.TryGetComponent(out QueueSlot queueSlot)) {
            queueSlot.Initialize(unit.icon, color);
        }
        else {
            print("Prefab does not have QueueSlot componenet");
        }

    }

    public void Dequeue() {
        if (queueSlotGameobjects.Count > 0) {
            Destroy(queueSlotGameobjects[0]);
            queueSlotGameobjects.RemoveAt(0);
        }
    }

    public void Clear() {
        foreach (var gameObject in queueSlotGameobjects) {
            Destroy(gameObject);
        }
        queueSlotGameobjects.Clear();
    }

    public void SetRoundCounter(string message) {
        roundCounterText.text = message;
    }
}
