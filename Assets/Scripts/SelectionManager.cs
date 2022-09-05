using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    [Header("Settings")]
    [SerializeField] private List<Unit> unitsToChooseFrom;
    [SerializeField] private List<Item> itemsToChooseFrom;

    [SerializeField] private LootGenerator lootGenerator;

    [Header("Weapon Generation Settings")]
    [SerializeField] private int numberOfWeaponsToGenerate = 3;

    [Header("Armor Generation Settings")]
    [SerializeField] private int numberOfArmorsToGenerate = 2;

    [Header("Debugging")]
    [SerializeField] private Party playerParty;
    [SerializeField] private Storage playerStorage;

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
        playerStorage = ScriptableObject.CreateInstance<Storage>();
    }

    private void Start() {
        // Trigger event to spawn all the units to choose from
        SelectionEvents.instance.TriggerOnDisplayUnitOptions(unitsToChooseFrom);

        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            // Start generating weapons
            playerStorage.Initialize(16);
            for (int i = 0; i < numberOfWeaponsToGenerate; i++)
            {
                // Generate weapon from generator
                var weapon = lootGenerator.GenerateWeapon();
                // Add to storage
                playerStorage.AddToIndex(weapon, i);
                // Trigger event
                SelectionEvents.instance.TriggerOnAddItemToStorage(weapon, i);
            }
        }
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
