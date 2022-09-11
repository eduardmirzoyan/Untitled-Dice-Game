using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Armor : Item
{
    // Maybe have stats?
    public List<Passive> passives;

    public void Initialize(List<Passive> passives) {
        this.passives = new List<Passive>();

        foreach (var passive in passives) {
            passive.sourceArmor = this;
            this.passives.Add(passive);
        }
    }

    public Armor Copy() {
        // Make a copy
        Armor copy = Instantiate(this);

        // Make a copy of all it's actions
        for (int i = 0; i < passives.Count; i++) {
            // Make a copy of all the weapons
            copy.passives[i] = Instantiate(passives[i]);
            // Set source of passive to the copy
            copy.passives[i].sourceArmor = copy;
        }

        return copy;
    }
}
