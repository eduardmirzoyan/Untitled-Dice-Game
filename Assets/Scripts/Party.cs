using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Party")]
public class Party : ScriptableObject
{
    public List<Unit> partyMembers;
    public DicePool dicePool;
    public int maxSize = 4;
    
    private void Awake() {
        // Initialize with null
        partyMembers = new List<Unit>();
        for (int i = 0; i < maxSize; i++)
        {
            partyMembers.Add(null);
        }
        
        // Create die pool
        dicePool = ScriptableObject.CreateInstance<DicePool>();
    }

    public void Add(Unit unit, int index) {
        // Error checking
        if (index >= maxSize || index < 0) {
            Debug.Log("Tried to add unit to an invalid index: " + index);
            return;
        }

        // Check if a unit exists in that spot
        if (partyMembers[index] != null) {
            Debug.Log("Unit exists in this spot and was replaced.");
        }

        // Check if the same unit already exists in the party
        for (int i = 0; i < maxSize; i++) {
            if (partyMembers[i] == unit) {
                partyMembers[i] = null;
            }
        }

        // Add unit to party
        partyMembers[index] = unit;

        // Add unit's die to diepool
        if (unit != null)
            dicePool.Add(unit.dice, index);
    }

    public Unit this[int index] {
        get { return partyMembers[index]; }
        set { partyMembers[index] = value; }
    }

    public List<Unit> GetMembers() {
        return partyMembers;
    }

    public bool IsFull() {
        return partyMembers.Count(unit => unit != null) == maxSize;
    }

    public bool IsDead() {
        // Check if all party members are dead
        return partyMembers.All(member => member.IsDead());
    }
}
