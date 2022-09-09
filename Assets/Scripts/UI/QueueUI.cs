using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QueueUI : MonoBehaviour
{
    [SerializeField] private SlidingWindow slidingWindow;
    [SerializeField] private List<GameObject> queueSlotGameobjects;
    [SerializeField] private GameObject queueSlotPrefab;
    [SerializeField] private HorizontalLayoutGroup queueLayoutGroup;
    [SerializeField] private TextMeshProUGUI roundCounterT;

    private int roundNumber = 0;

    private void Awake() {
        slidingWindow = GetComponent<SlidingWindow>();
    }

    private void Start() {
        // Subscribe to events
        CombatEvents.instance.onTurnStart += StartTurn;
        CombatEvents.instance.onCombatStart += OnCombatStart;
        CombatEvents.instance.onFormQueue += FormQueue;
        CombatEvents.instance.deQueue += Dequeue;
        CombatEvents.instance.onClearQueue += ClearQueue;
        CombatEvents.instance.onCombatEnd += OnCombatEnd;
    }

    private void OnCombatStart(ActionInfo info) {
        // Lower window
        slidingWindow.Lower();
    }

    private void FormQueue(Queue<Combatant> queue) {
        // Update round number
        roundNumber++;
        roundCounterT.text = "Round " + roundNumber;

        // Spawn queue
        SetUpTurnQueue(queue);
    }

    public void Dequeue(Combatant combatant) {
        if (queueSlotGameobjects.Count > 0) {
            Destroy(queueSlotGameobjects[0]);
            queueSlotGameobjects.RemoveAt(0);
        }
    }

    private void StartTurn(ActionInfo actionInfo, Combatant combatant) {
        if (queueSlotGameobjects.Count > 0) {
            queueSlotGameobjects[0].GetComponent<QueueSlot>().Highlight();
        }
        actionInfo.waitTime = 0f;
    }

    private void ClearQueue(int value) {
        // ?
    }

    private void OnCombatEnd(ActionInfo info) {
        // Raise window
        slidingWindow.Raise();
    }


    // Helper functions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void SetUpTurnQueue(Queue<Combatant> queue) {
        string result = "";
        foreach (var combatant in queue.ToList()) {
            // Enqueue combatant
            Enqueue(combatant.unit, Color.clear);

            result += combatant.unit.name + " -> ";
        }
        // Debug print queue order
        print(result + "END");
    }

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
     
    /// Maybe doesn't need to be used?
    public void Clear() {
        foreach (var gameObject in queueSlotGameobjects) {
            Destroy(gameObject);
        }
        queueSlotGameobjects.Clear();
    }
}
