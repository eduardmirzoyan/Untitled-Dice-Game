using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    [SerializeField] private List<Unit> unitsToChooseFrom;
    [SerializeField] private Party playerParty;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        // Create a new party for the player to fill
        playerParty = ScriptableObject.CreateInstance<Party>();

        // Trigger event to spawn all the units to choose from
    }

    private void Start() {
        SelectionEvents.instance.TriggerOnDisplayUnitOptions(unitsToChooseFrom);
    }

    public void AddUnitToParty(Unit unit, int index) {
        // Add the selected unit to party
        playerParty.Add(unit, index);

        // Trigger event
        SelectionEvents.instance.TriggerOnAddUnitToParty(unit, index);

        // Trigger second event
        SelectionEvents.instance.TriggerOnPartyFull(playerParty.IsFull());
    }

    public void StartGame() {
        // Make sure the player's party is full before transitioning
        if (playerParty.IsFull()) {
            // Transfer data to combat manager
            GameManager.instance.SetParty(playerParty);

            // Transition to combat scene
            TransitionManager.instance.LoadNextScene();
        }
    }

}
