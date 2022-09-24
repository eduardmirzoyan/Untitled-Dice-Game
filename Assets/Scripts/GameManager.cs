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
    public int minPartySize = 4;
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
        // Die keywords
        dictionary["REROLL"] = "Roll the dice to a new value (The new value can be same as it was previously)";
        dictionary["HIGHROLL"] = "The value of the die is at its maximum.";
        dictionary["LOWROLL"] = "The value of the die is at its minimum.";
        dictionary["EVEN"] = "The value of the die is an even number.";
        dictionary["ODD"] = "The value of the die is an odd number.";
        dictionary["GROW"] = "Increase the value of the die by 1, with overflow";
        dictionary["SHRINK"] = "Decrease the value of the die by 1, with overflow";
        dictionary["PAIR"] = "Your dice pool contains 2+ dice with the same value.";
        dictionary["UNIQUE"] = "Every die in your dice pool has a different value.";
        dictionary["BOUNTY"] = "Triggers if the target has the mark status effect, then removes the status.";

        // Status effect keywords
        dictionary["MARK"] = "Triggers additional Skill effects when unit is targeted.";
        dictionary["BLEED"] = "On Turn Start: Unit takes damage equal to stacks. Then half stacks.";
        dictionary["STRENGTH"] = "Increase the damage of your next Attack Action by 20% per stack. Then remove this effect.";
        // ?
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

    public void SetPlayerParty(Unit unit, int index) {
        // Add the unit to the party
        allyParty.Set(unit, index);

        // Trigger event
        GameEvents.instance.TriggerOnPartyFull(allyParty.IsFull());
    }

    public void DeployPlayerParty() {
        // Create UnitUIs for each member in the player party
        int index = 0;
        foreach (var member in allyParty.GetMembers()) {
            // Trigger event
            GameEvents.instance.TriggerOnDeployUnit(member, index);

            index++;
        }
    }

    public void ClearPlayerParty() {
        // Create a new party
        allyParty = ScriptableObject.CreateInstance<Party>();
    }

    public void SetOpponent(Party party) {
        this.enemyParty = party;
    }
}
