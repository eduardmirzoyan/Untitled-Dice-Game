using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<string, string> dictionary;
    public Party allyParty;
    public Party enemyParty;

    [SerializeField] private List<Unit> possibleEnemies;

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

        // Initialize party
        enemyParty = ScriptableObject.CreateInstance<Party>();
        for (int i = 0; i < 4; i++)
        {
            enemyParty.Add(possibleEnemies[i], i);
        }

        DontDestroyOnLoad(this);
    }

    public void SetParty(Party party) {
        this.allyParty = party;
    }
}
