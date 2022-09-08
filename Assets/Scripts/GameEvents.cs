using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake() {
        // Singleton Logic
        if(GameEvents.instance != null) {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(this);
    }

    public event Action<Party> onSetParty;

    public void TriggerOnSetParty(Party party) {
        if (onSetParty != null) {
            onSetParty(party);
        }
    }
}
