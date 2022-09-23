using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class Action : Skill
{
    public bool canTargetSelf = false;
    public bool canTargetAllies = false;
    public bool canTargetEnemies = false;
    public int hasCooldown = 0;
    public int cooldown = 0;
    public int hasUses = 0;
    public int uses = 0;

    protected Regex rx = new Regex("(\\d+(?>\\.\\d+|))xD");
    protected Dice dice;

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

    public virtual void ShowTargetConstraintFeedback(Combatant combatant) {
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

    public override string GetRawDescription() {
        // Get base raw description
        string rawText = base.GetRawDescription();
        // Replace text with die icons where needed
        return rx.Replace(rawText, new MatchEvaluator(InsertDieIcons));
    }

    public string GetDynamicDescription(Dice insertedDie) {
        // Get base raw description
        string rawText = base.GetRawDescription();
        dice = insertedDie;
        // Replace text with die values where needed
        return rx.Replace(rawText, new MatchEvaluator(InsertDieValues));
    }

    public string GetFinalDescription() {
        // This is the final damage value after taking into account your bonuses and enemy debuffs
        string rawText = base.GetRawDescription();
        // Replace text with die values where needed
        return rx.Replace(rawText, new MatchEvaluator(InsertFinalValues));
    }

    

    private string InsertDieIcons(Match m) {
        // If multiplier is 1 return a pre-format
        if (m.Groups[1].ToString() == "1") return "<color=yellow><sprite=0></color>";

        return "<color=white>" + m.Groups[1] + "x<sprite=0></color>";
    }

    private string InsertDieValues(Match m) {
        // Calculate final value based on multiplied
        int value = (int) (float.Parse(m.Groups[1].ToString()) * dice.GetValue());

        return "<color=yellow>" + value + "</color>";
    }

    private string InsertFinalValues(Match m) {
        // Calculate final value based on user multiplier and die value
        float finalMultiplier = sourceCombatant.baseDamageMultiplier;
        int value = (int) (finalMultiplier * float.Parse(m.Groups[1].ToString()) * dice.GetValue());

        // If multiplier is positive, show green text
        if (finalMultiplier > 1.1f) {
            return "<color=green>" + value + "</color>";
        }
        // If multiplier is negative, show red text
        else if (finalMultiplier < 0.9f) {
            return "<color=red>" + value + "</color>";
        }

        return "<color=yellow>" + value + "</color>";
    }

    public abstract void Perform(int targetIndex, List<Combatant> combatants, Dice dice);
}
