using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public new string name;
    
    [TextArea(10, 20)]
    public string description;
    public Sprite icon;
    public Combatant combatant; // The unit that the effect is on
    public int maxStacks = 1; // 0 means inf stacks
    public int stacks = 0;

    public virtual void Initialize(Combatant combatant) {
        this.combatant = combatant;
    }

    public virtual void Stack(StatusEffect statusEffect) {
        // Does nothing naturally
    }

    public virtual void Uninitialize() {
        this.combatant = null;
    }
}
