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
    public event Action<Unit, int> onAddUnitToParty;
    public event Action<Unit, int> onDeployUnit;
    public event Action<bool> onPartyFull;

    public void TriggerOnSetParty(Party party) {
        if (onSetParty != null) {
            onSetParty(party);
        }
    }

    public void TriggerOnAddUnitToParty(Unit unit, int index) {
        if (onAddUnitToParty != null) {
            onAddUnitToParty(unit, index);
        }
    }

    public void TriggerOnDeployUnit(Unit unit, int index) {
        if (onDeployUnit != null) {
            onDeployUnit(unit, index);
        }
    }

    public void TriggerOnPartyFull(bool state) {
        if (onPartyFull != null) {
            onPartyFull(state);
        }
    }
}
