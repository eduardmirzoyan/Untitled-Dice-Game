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

    [SerializeField] public GameObject unitUIprefab;

    public bool fastMode;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        // Initialize library
        dictionary = new Dictionary<string, string>();
        dictionary["REROLL"] = "ROLL the dice to a new value (The new value can be same as it was previously)";
        dictionary["HIGHROLL"] = "The value of the die is at its MAXIMUM.";
        dictionary["LOWROLL"] = "The value of the die is at its MINIMUM.";
        dictionary["EVEN"] = "The value of the die is an EVEN number.";
        dictionary["ODD"] = "The value of the die is an ODD number.";
        dictionary["GROW"] = "INCREASE the value of the die by 1, with overflow";
        dictionary["SHRINK"] = "DECREASE the value of the die by 1, with overflow";
        dictionary["PAIR"] = "Your DICE POOL contains 2+ dice with the SAME value.";
        dictionary["UNIQUE"] = "Every die in your DICE POOL has a DIFFERENT value.";
        dictionary["BLEED"] = "To be implemented.";

        // Create a player party (Which should be overwritten)
        allyParty = ScriptableObject.CreateInstance<Party>();

        // Create a enemy party (Which should be overwritten)
        enemyParty = ScriptableObject.CreateInstance<Party>();

        // Create storage (Which should be overwritten)
        storage = ScriptableObject.CreateInstance<Storage>();

        DontDestroyOnLoad(this);
    }

    public void SetPlayer(Party party) {
        this.allyParty = party;
    }

    public void SetOpponent(Party party) {
        this.enemyParty = party;
    }
}
