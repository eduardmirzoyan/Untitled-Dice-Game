using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    [Header("Settings")]
    [SerializeField] private List<Unit> unitsToChooseFrom;

    [SerializeField] private LootGenerator lootGenerator;
    [SerializeField] private CombatEnounterGenerator combatEnounterGenerator;

    [Header("Weapon Generation Settings")]
    [SerializeField] private int numberOfWeaponsToGenerate = 4;

    [Header("Armor Generation Settings")]
    [SerializeField] private int numberOfArmorsToGenerate = 4;

    [Header("Debugging")]
    [SerializeField] private Party playerParty;
    [SerializeField] private Storage playerStorage;
    [SerializeField] private float waitTilSpawn = 0.5f;
    [SerializeField] private SlidingWindow slidingWindow;

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
        // Start routine
        StartCoroutine(WaitThenDisplay());
    }

    private IEnumerator WaitThenDisplay() {
        // Wait for dramatic effect
        yield return new WaitForSeconds(waitTilSpawn);

        // Spawn items
        SpawnItems();

        // Lower window
        slidingWindow.Lower();

        // Trigger event to spawn all the units to choose from
        SelectionEvents.instance.TriggerOnDisplayUnitOptions(unitsToChooseFrom);
    }

    private void SpawnItems() {
        // Initialize storage
        playerStorage.Initialize(16);

        // Generate weapons and spawn them
        for (int i = 0; i < numberOfWeaponsToGenerate; i++) {
            // Generate weapon from generator
            var weapon = lootGenerator.GenerateWeapon();
            // Add to storage
            playerStorage.AddToIndex(weapon, i);
            // Trigger event
            SelectionEvents.instance.TriggerOnAddItemToStorage(weapon, i);
        }

        // Generate armors and spawn them
        for (int i = numberOfWeaponsToGenerate; i < numberOfWeaponsToGenerate + numberOfArmorsToGenerate; i++) {
            // Generate weapon from generator
            var armor = lootGenerator.GenerateArmor();
            // Add to storage
            playerStorage.AddToIndex(armor, i);
            // Trigger event
            SelectionEvents.instance.TriggerOnAddItemToStorage(armor, i);
        }
    }

    public void StartGame() {
        // Replace party with copy
        // for (int i = 0; i < playerParty.maxSize; i++)
        // {
        //     // Make copy of unit
        //     var copy = Instantiate(playerParty[i]);
        //     // Make copy of die
        //     copy.dice = Instantiate(playerParty[i].dice);

        //     // Clear the current unit
        //     playerParty[i].ClearEquipment();

        //     // Add the selected unit to party
        //     playerParty.Set(copy, i);
        // }

        // Transfer data to combat manager
        // GameManager.instance.SetPlayer(playerParty);

        // Set random encounter
        GameManager.instance.SetOpponent(combatEnounterGenerator.GenerateEnemyEncounter());

        // Lower window
        slidingWindow.Raise();

        // Transition to combat scene
        TransitionManager.instance.LoadNextScene();
    }

}
