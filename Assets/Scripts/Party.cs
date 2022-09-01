using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Party")]
public class Party : ScriptableObject
{
    public Unit[] partyMembers;
    public DicePool dicePool;
    
    private void Awake() {
        Debug.Log("test: " + name);
    }

    public void Create(Unit[] units) {
        // TODO

        // Set up party members
        partyMembers = units;

        // set up dice pool based on party members
        foreach (var unit in partyMembers) {
            // Fill die pool
        }
    }

    public void FormPool() {
        dicePool.Fill(partyMembers);
    }

    public Unit this[int index] {
        get {
            return partyMembers[index];
        }
        set {
            partyMembers[index] = value;
        }
    }

    public Unit[] GetMembers() {
        return partyMembers;
    }

    public int Size() {
        return partyMembers.Length;
    }

    public bool IsDead() {
        // Check if all party members are dead
        return partyMembers.All(member => member.IsDead());
    }
}
