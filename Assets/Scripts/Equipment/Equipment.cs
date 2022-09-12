using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item
{
    public List<Skill> skills;

    public void Initialize(List<Skill> skills) {
        // Make new list of actions
        this.skills = new List<Skill>();

        foreach (var skill in skills) {
            // Set source to this
            skill.sourceEquipment = this;
            // Then add skill
            this.skills.Add(skill);
        }
    }

    public abstract Equipment Copy();

}
