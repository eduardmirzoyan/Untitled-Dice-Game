using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : Skill
{
    public bool canTargetSelf = false;
    public bool canTargetAllies = false;
    public bool canTargetEnemies = false;
    public int hasCooldown = 0;
    public int cooldown = 0;
    public int hasUses = 0;
    public int uses = 0;

    public virtual bool CheckActionConstraints() {
        // Check coooldown first
        if (hasCooldown > 0 && cooldown > 0) {
            // Action is on cooldown
            return false;
        }

        // Then check if item has uses
        if (hasUses > 0 && uses <= 0) {
            // Action is out of uses
            return false;
        }

        // Else everything is good
        return true;
    }

    public virtual void ShowActionConstraintFeedback() {
        if (hasCooldown > 0 && cooldown > 0) {
            // Action is on cooldown
            CombatEvents.instance.TriggerOnFeedback("This action is on cooldown.");
        }

        // Then check if item has uses
        if (hasUses > 0 && uses <= 0) {
            // Action is out of uses
            CombatEvents.instance.TriggerOnFeedback("This action is out of uses.");
        }
    }
    
    public virtual bool CheckDieConstraints(Dice dice) {
        return true;
    }

    public virtual void ShowDieConstraintFeedback(Dice dice) {
        // Does nothing
    }

    public virtual bool CheckTargetConstraints(Combatant combatant) {
        return true;
    }

    public virtual void ShowTargetConstraintFeedback() {
        // Does nothing
    }

    public void Initialize() {
        // Set uses to max uses
        uses = hasUses;

        // Remove cooldowns
        cooldown = 0;
    }

    public void SetCooldown() {
        // When action is performed
        // Must add an extra 1 to account for same turn reduce cooldown
        cooldown = hasCooldown + 1;
    }

    public void ReduceCooldown() {
        // Reduce cooldown by 1 until 0
        cooldown = Mathf.Max(cooldown - 1, 0);
    }

    public void UpdateUses() {
        // Reduce uses by 1 until 0
        uses = Mathf.Max(uses - 1, 0);
    }

    public abstract void Perform(int targetIndex, List<Combatant> combatants, Dice dice);
}
