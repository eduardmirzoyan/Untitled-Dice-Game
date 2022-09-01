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

    private void Awake()
    {
        // Singleton Logic
        if(CombatEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Game states
    public event Action<ActionInfo> onCombatStart;
    public event Action<ActionInfo> onRoundStart;
    public event Action<ActionInfo, Combatant> onTurnStart; // Update
    public event Action<ActionInfo> onRoundEnd;
    public event Action<ActionInfo> onCombatEnd;

    // Die events
    public event Action<Dice> onRoll;
    public event Action<ActionInfo> onReroll;
    public event Action<Dice> onGrow;
    public event Action<Dice> onShrink;
    public event Action<Dice, bool> onSetActive;

    
    // Action events
    public event Action<Action, Dice> onDieInsert;
    public event Action<Action> onActionConfirm;
    public event Action<Combatant> onTargetSelect;

    // General events
    public event Action<Combatant> onTakeDamage;
    public event Action<Combatant> onDie;

    // UI events
    public event Action<Queue<Combatant>> onFormQueue; // ?
    public event Action<Combatant> deQueue; // ?
    public event Action<int> onClearQueue; // ?

    public event Action<string> onFeedback;

    public void TriggerOnFeedback(string message) {
        if (onFeedback != null) {
            onFeedback(message);
        }
    }

    #region Action based events
    public void TriggerOnDieInsert(Action action, Dice dice) {
        if (onDieInsert != null) {
            onDieInsert(action, dice);
        }
    }

    public void TriggerOnActionConfirm(Action action) {
        if (onActionConfirm != null) {
            onActionConfirm(action);
        }
    }

    public void TriggerOnTargetSelect(Combatant combatant) {
        if (onTargetSelect != null) {
            onTargetSelect(combatant);
        }
    }

    #endregion


    #region Queue based events

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

    // FIGURE THIS SHIT OUT ?
    public IEnumerator TriggerReroll(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onReroll != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onReroll.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
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

    public void TriggerSetActive(Dice dice, bool state) {
        if (onSetActive != null) {
            onSetActive(dice, state);
        }
    }

    #endregion

    #region Combat state based events
    // Turn state events
    public IEnumerator TriggerCombatStart(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onCombatStart != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onCombatStart.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerRoundStart(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onRoundStart != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onRoundStart.GetInvocationList()) {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerTurnStart(ActionInfo info, Combatant combatant) 
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onTurnStart != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onTurnStart.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info, combatant);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerRoundEnd(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onRoundEnd != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onRoundEnd.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    public IEnumerator TriggerCombatEnd(ActionInfo info)
    {
        // Wait for some time first
        yield return new WaitForSeconds(info.waitTime);

        // If there are any subscribers
        if (onCombatEnd != null)
        {
            // Invoke all the subscribers
            foreach (var invocation in onCombatEnd.GetInvocationList())
            {
                // Call invokation and update info values
                invocation.DynamicInvoke(info);
                // Wait an amount of time
                yield return new WaitForSeconds(info.waitTime);
            }
        }
    }

    #endregion

    #region Unit-based events

    public void TriggerOnTakeDamage(Combatant combatant) {
        if (onTakeDamage != null) {
            onTakeDamage(combatant);
        }
    }

    public void TriggerOnDie(Combatant combatant) {
        if (onDie != null) {
            onDie(combatant);
        }
    }

    #endregion
}
