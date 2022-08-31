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

    public event Action<ActionInfo> onCombatStart;
    public event Action<ActionInfo> onRoundStart;
    public event Action<ActionInfo, Combatant> onTurnStart; // Update
    public event Action<ActionInfo> onRoundEnd;
    public event Action<ActionInfo> onCombatEnd;
    public event Action<ActionInfo> onReroll;
    public event Action<Action, Dice> onDieInsert;
    public event Action<Action> onActionConfirm;

    public void TriggerDieInsert(Action action, Dice dice) {
        if (onDieInsert != null) {
            onDieInsert(action, dice);
        }
    }

    public void TriggerActionConfirm(Action action) {
        if (onActionConfirm != null) {
            onActionConfirm(action);
        }
    }

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

}
