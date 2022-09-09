using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkillUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SkillDisplaySlotUI skillDisplaySlotUI;
    [SerializeField] private SkillDisplayDescriptionUI skillDisplayDescriptionUI;

    [SerializeField] private Action action;
    [SerializeField] private Passive passive;


    public void Initialize(Action action) {
        this.action = action;
        skillDisplaySlotUI.Initialize(action, false);
        skillDisplayDescriptionUI.Initialize(action);
        
    }

    public void Initialize(Passive passive) {
        this.passive = passive;
        skillDisplaySlotUI.Initialize(passive);
        skillDisplayDescriptionUI.Initialize(passive);
    }
}
