using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    [TextArea(10, 20)]
    public string description;
    public Sprite icon;
    public bool canTargetSelf = false;
    public bool canTargetAllies = false;
    public bool canTargetEnemies = false;
    public Weapon sourceWeapon = null;
    public int cooldown = 0;
    public int cooldownCounter = 0;
    public int uses = 0;
    public int usesCounter = 0;

    public virtual bool passesContraints(Dice dice) {
        return true;
    }

    public abstract void Perform(List<Combatant> targets, Dice dice);

    public virtual List<Combatant> getSecondaryTargets() {
        // Returns empty list by default
        return new List<Combatant>();
    }

    public virtual string GetDynamicDescription() {
        return description;
    }
}
