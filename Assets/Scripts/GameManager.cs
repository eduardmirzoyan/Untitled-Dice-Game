using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<string, string> dictionary;
    public Party allyParty;
    public Party enemyParty;
    public Storage storage;

    [SerializeField] private List<Unit> possibleEnemies;

    [SerializeField] public GameObject unitUIprefab;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        // Initialize library
        dictionary = new Dictionary<string, string>();
        dictionary["REROLL"] = "Rolls the dice to a new value (The new value can be same as it was previously)";
        dictionary["HIGHROLL"] = "The value of the die is at its maximum.";
        dictionary["LOWROLL"] = "The value of the die is at its minimum.";
        dictionary["EVEN"] = "The value of the die is an even number.";
        dictionary["ODD"] = "The value of the die is an odd number.";
        dictionary["BLEED"] = "To be implemented.";

        // Create a player party (Which should be overwritten)
        allyParty = ScriptableObject.CreateInstance<Party>();

        // Create a enemy party (Which should be overwritten)
        enemyParty = ScriptableObject.CreateInstance<Party>();
        for (int i = 0; i < 4; i++) {
            enemyParty.Add(possibleEnemies[i], i);
        }

        // Create storage (Which should be overwritten)
        storage = ScriptableObject.CreateInstance<Storage>();

        DontDestroyOnLoad(this);
    }

    public void SetParty(Party party) {
        this.allyParty = party;
    }
}
