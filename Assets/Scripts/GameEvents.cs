using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        // Singleton Logic
        if(GameEvents.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public event Action<DiceSlotUI> onDiceInserted;
    public event Action<DiceSlotUI> onDiceRemoved;

    

}
