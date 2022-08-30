using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : ScriptableObject
{
    // Source?
    public new string name;
    public string description;
    public Unit possessor;
    public Combatant combatant;

    public virtual void Initialize(Combatant combatant) {
        this.combatant = combatant;
    }

    public abstract void Terminate();
}
