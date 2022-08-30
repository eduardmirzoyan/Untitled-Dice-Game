using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

    public Action<int> onCombatStart;
    public Action<int> onRoundStart;
    public Action<Combatant> onTurnStart;
    public Action<int> onRoundEnd;
    public Action<int> onCombatEnd;

    public Action<int> onReroll;

    public void TriggerCombatStart(int value)
    {
        if (onCombatStart != null)
        {
            onCombatStart(value);
        }
    }

    public void TriggerRoundStart(int value)
    {
        if (onRoundStart != null)
        {
            onRoundStart(value);
        }
    }

    public void TriggerTurnStart(Combatant combatant) 
    {
        if (onTurnStart != null) 
        {
            onTurnStart(combatant);
        }
    }

    public void TriggerRoundEnd(int value)
    {
        if (onRoundEnd != null)
        {
            onRoundEnd(value);
        }
    }

    public void TriggerCombatEnd(int value)
    {
        if (onCombatEnd != null)
        {
            onCombatEnd(value);
        }
    }

    public void TriggerReroll(int value) 
    {
        if (onReroll != null)
        {
            onReroll(value);
        }
    }

}
