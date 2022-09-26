using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Apply D BURN
[CreateAssetMenu(menuName = "Actions/Burn/Attack 1")]
public class BurnAction1 : AttackAction
{
    public Burn burn;

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Make copy
        var burn = Instantiate(this.burn);
        burn.stacks = dice.GetValue();
        combatants[targetIndex].AddStatusEffect(burn);
    }
}
