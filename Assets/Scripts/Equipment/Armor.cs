using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Armor : Equipment
{
    // Maybe have stats?
    // public List<Passive> passives;

    // public void Initialize(List<Passive> passives) {
    //     this.passives = new List<Passive>();

    //     foreach (var passive in passives) {
    //         passive.sourceEquipment = this;
    //         this.passives.Add(passive);
    //     }
    // }

    public override Equipment Copy() {
        // Make a copy
        Armor copy = Instantiate(this);

        // Make a copy of all it's actions
        for (int i = 0; i < skills.Count; i++) {
            // Make a copy of all the weapons
            copy.skills[i] = Instantiate(skills[i]);
            // Set source of passive to the copy
            copy.skills[i].sourceEquipment = copy;
        }

        return copy;
    }
}
