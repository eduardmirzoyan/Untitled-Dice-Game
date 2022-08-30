using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Canvas playerScreen; // Stores the player's screen for other classes to get
    public Dictionary<string, string> dictionary;
    public PartyMemberUI[] partyMemberUIs;
    public Party party;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        // Initialize library
        dictionary = new Dictionary<string, string>();
        dictionary["KEYWORD"] = "This is a test.";
        dictionary["HIGHROLL"] = "The value of the die is at its maximum.";
        dictionary["LOWROLL"] = "The value of the die is at its minimum.";
        dictionary["EVEN"] = "The value of the die is an even number.";
        dictionary["ODD"] = "The value of the die is an odd number.";
        dictionary["BLEED"] = "To be implemented.";

        if (partyMemberUIs == null) {
            throw new System.Exception("Party Member UIs not assigned.");
        }

        for (int i = 0; i < party.Size(); i++)
        {
            partyMemberUIs[i].Initialize(party[i]);
        }
    }


}
