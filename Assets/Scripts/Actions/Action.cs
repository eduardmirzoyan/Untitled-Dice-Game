using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public new string name;
    [TextArea(10, 20)]
    public string description;
    public Sprite icon;
    public bool canTargetSelf = false;
    public bool canTargetAllies = false;
    public bool canTargetEnemies = false;
    public int cooldown = 0;
    public int cooldownCounter = 0;
    public int uses = 0;
    public int usesCounter = 0;
    
    public virtual bool checkDieConstraints(Dice dice) {
        return true;
    }

    public virtual bool checkTargetConstraints(Combatant combatant) {
        return true;
    }

    public abstract void Perform(int targetIndex, List<Combatant> combatants, Dice dice);

    public virtual string GetDynamicDescription() {
        return description;
    }
}
