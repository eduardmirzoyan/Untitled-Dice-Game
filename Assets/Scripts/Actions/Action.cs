using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public bool canTargetSelf = false;
    public bool canTargetAllies = false;
    public bool canTargetEnemies = false;

    public abstract void Perform(List<Combatant> targets, Dice dice);

    public virtual List<Combatant> getSecondaryTargets() {
        // Returns empty list by default
        return new List<Combatant>();
    }
}
