using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActionInfo 
{
    public float waitTime;

    public ActionInfo(float value) {
        this.waitTime = value;
    }

    public ActionInfo() {
        this.waitTime = 0f;
    }
}

public class CombatEvents : MonoBehaviour
{
    public static CombatEvents instance;

    private void Awake() {
        // Singleton Logic
        if(CombatEvents.instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Banner events
    public event Action<string> onShowBanner;
    public event Action<string> onHideBanner;

    // Game state events
    public event Action<int> onCombatStart;
    public event Action<int> onRoundStart;
    public event Action<int> onTurnStart;
    public event Action<int> onTurnEnd;
    public event Action<int> onRoundEnd;
    public event Action<int> onCombatEnd;

    // Skill events
    public event Action<ActionInfo> onSkillCombatStart;
    public event Action<ActionInfo> onSkillRoundStart;
    public event Action<ActionInfo, Combatant> onSkillTurnStart;
    public event Action<ActionInfo> onSkillRoundEnd;
    public event Action<ActionInfo> onSkillCombatEnd;

    // Die events
    public event Action<Dice> onRoll;
    public event Action<Dice> onReroll;
    public event Action<Dice> onGrow;
    public event Action<Dice> onShrink;
    public event Action<Dice> onReplenish;
    public event Action<Dice> onExhaust;

    // Action events
    public event Action<int> onPlayerTurnStart;
    public event Action<Dice> onDieStartDrag;
    public event Action<Dice> onDieEndDrag;
    public event Action<Action, Dice> onActionSelect;
    public event Action<Combatant> onTargetSelect;
    public event Action<Action> onPreActionConfirm;
    public event Action<Action> onActionConfirm;
    public event Action<int> onPlayerTurnEnd;

    // General events
    public event Action<Combatant, int> onHeal;
    public event Action<Combatant, int> onTakeDamage;
    public event Action<Combatant> onDie;

    // UI events
    public event Action<Combatant, float> onSpawnCombatant;
    public event Action<Queue<Combatant>> onFormQueue;
    public event Action<Combatant> deQueue;
    public event Action<int> onClearQueue;
    public event Action<string> onFeedback;


    #region Game State Events

    public void TriggerOnCombatStart(int value) {
        if (onCombatStart != null) {
            onCombatStart(value);
        }
    }

    public void TriggerOnRoundStart(int value) {
        if (onRoundStart != null) {
            onRoundStart(value);
        }
    }

    public void TriggerOnTurnStart(int value) {
        if (onTurnStart != null) {
            onTurnStart(value);
        }
    }

    public void TriggerOnTurnEnd(int value) {
        if (onTurnEnd != null) {
            onTurnEnd(value);
        }
    }

    public void TriggerOnRoundEnd(int value) {
        if (onRoundEnd != null) {
            onRoundEnd(value);
        }
    }

    public void TriggerOnCombatEnd(int value) {
        if (onCombatEnd != null) {
            onCombatEnd(value);
        }
    }

    #endregion

    #region UI Events
    public void TriggerOnShowBanner(string message) {
        if (onShowBanner != null) {
            onShowBanner(message);
        }
    }

    public void TriggerOnHideBanner(string message) {
        if (onHideBanner != null) {
            onHideBanner(message);
        }
    }

    public void TriggerOnFeedback(string message) {
        if (onFeedback != null) {
            onFeedback(message);
        }
    }

    #endregion


    public void TriggerOnPlayerTurnStart(int value) {
        if (onPlayerTurnStart != null) {
            onPlayerTurnStart(value);
        }
    }

    public void TriggerOnPlayerTurnEnd(int value) {
        if (onPlayerTurnEnd != null) {
            onPlayerTurnEnd(value);
        }
    }


    

    #region Action based events
    public void TriggerOnDieStartDrag(Dice dice) {
        if (onDieStartDrag != null) {
            onDieStartDrag(dice);
        }
    }

    public void TriggerOnActionSelect(Action action, Dice dice) {
        if (onActionSelect != null) {
            if (action != null) print("Debug: Action " + action.name + " was selected.");
            else print("Debug: Action was deselected.");

            onActionSelect(action, dice);
        }
    }

    public void TriggerOnDieEndDrag(Dice dice) {
        if (onDieEndDrag != null) {
            onDieEndDrag(dice);
        }
    }

    public void TriggerOnPreActionConfirm(Action action) {
        if (onPreActionConfirm != null) {
            onPreActionConfirm(action);
        }
    }

    public void TriggerOnActionConfirm(Action action) {
        if (onActionConfirm != null) {
            onActionConfirm(action);
        }
    }

    public void TriggerOnTargetSelect(Combatant combatant) {
        if (onTargetSelect != null) {
            if (combatant != null) print("Debug: Target " + combatant.unit.name + " was selected.");
            else print("Debug: Target was deselected.");
            onTargetSelect(combatant);
        }
    }

    #endregion


    #region Queue based events

    public void TriggerOnSpawnCombatant(Combatant combatant, float spawnTime) {
        if (onSpawnCombatant != null) {
            onSpawnCombatant(combatant, spawnTime);
        }
    }

    public void TriggerFormQueue(Queue<Combatant> queue) {
        if (onFormQueue != null) {
            onFormQueue(queue);
        }
    }

    public void TriggerDequeue(Combatant combatant) {
        if (deQueue != null) {
            deQueue(combatant);
        }
    }

    public void TriggerClearQueue(int value)
    {
        if (onClearQueue != null)
        {
            onClearQueue(value);
        }
    }

    #endregion


    #region Dice based events

    public void TriggerOnRoll(Dice dice) {
        if (onRoll != null) {
            onRoll(dice);
        }
    }

    public void TriggerReroll(Dice dice) {
        if (onReroll != null) {
            onReroll(dice);
        }
    }

    public void TriggerOnGrow(Dice dice) {
        if (onGrow != null) {
            onGrow(dice);
        }
    }

    public void TriggerOnShrink(Dice dice) {
        if (onShrink != null) {
            onShrink(dice);
        }
    }

    public void TriggerOnReplenish(Dice dice) {
        if (onReplenish != null) {
            onReplenish(dice);
        }
    }

    public void TriggerOnExhaust(Dice dice) {
        if (onExhaust != null) {
            onExhaust(dice);
        }
    }

    #endregion

    #region Skill Trigger Events
    // Turn state events
    public IEnumerator TriggerSkillCombatStart(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onSkillCombatStart != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onSkillCombatStart.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerSkillRoundStart(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onSkillRoundStart != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onSkillRoundStart.GetInvocationList()) {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerSkillTurnStart(ActionInfo info, Combatant combatant) 
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onSkillTurnStart != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onSkillTurnStart.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info, combatant);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerSkillRoundEnd(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onSkillRoundEnd != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onSkillRoundEnd.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerSkillCombatEnd(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onSkillCombatEnd != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onSkillCombatEnd.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    #endregion

    #region Unit-related events

    public void TriggerOnHeal(Combatant combatant, int amount) {
        if (onHeal != null) {
            onHeal(combatant, amount);
        }
    }

    public void TriggerOnTakeDamage(Combatant combatant, int amount) {
        if (onTakeDamage != null) {
            onTakeDamage(combatant, amount);
        }
    }

    public void TriggerOnDie(Combatant combatant) {
        if (onDie != null) {
            onDie(combatant);
        }
    }

    #endregion
}
