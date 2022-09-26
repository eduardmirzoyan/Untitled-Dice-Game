using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Burn/Attack 2")]
public class BurnAction2 : AttackAction
{
    public Burn burn;
    public int stacksToAdd = 3;

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        // Deal damage
        combatants[targetIndex].TakeDamage(dice.GetValue());

        // Make copy
        var burn = Instantiate(this.burn);
        burn.stacks = stacksToAdd;

        // Apply burn
        combatants[targetIndex].AddStatusEffect(burn);
    }
}
