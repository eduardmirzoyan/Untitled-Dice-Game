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
}
