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
        dictionary["REROLL"] = "Roll the dice to a new value (The new value can be same as it was previously)";
        dictionary["HIGHROLL"] = "The value of the die is at its maximum.";
        dictionary["LOWROLL"] = "The value of the die is at its minimum.";
        dictionary["EVEN"] = "The value of the die is an even number.";
        dictionary["ODD"] = "The value of the die is an odd number.";
        dictionary["GROW"] = "Increase the value of the die by 1, with overflow";
        dictionary["SHRINK"] = "Decrease the value of the die by 1, with overflow";
        dictionary["PAIR"] = "Your dice pool contains 2+ dice with the same value.";
        dictionary["UNIQUE"] = "Every die in your dice pool has a different value.";
        dictionary["HIGHROLLPOOL"] = "The value of all your dice are at their maximum";
        dictionary["LOWROLLPOOL"] = "The value of all your dice are at their minimum";

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
