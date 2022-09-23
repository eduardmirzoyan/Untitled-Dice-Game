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
    [SerializeField] private Skill skill;

    public void Initialize(Skill skill)
    {
        this.skill = skill;

        if (skill is Action) {
            skillDisplaySlotUI.Initialize((Action) skill, false);
        }
        else if (skill is Passive) {
            skillDisplaySlotUI.Initialize((Passive) skill);
        }

        skillDisplayDescriptionUI.InitializeRaw(skill);
    }

    // public void Initialize(Action action) {
    //     this.action = action;
    //     skillDisplaySlotUI.Initialize(action, false);
    //     skillDisplayDescriptionUI.Initialize(action);
        
    // }

    // public void Initialize(Passive passive) {
    //     this.passive = passive;
    //     skillDisplaySlotUI.Initialize(passive);
    //     skillDisplayDescriptionUI.Initialize(passive);
    // }
}
