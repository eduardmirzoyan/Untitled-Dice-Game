using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Burn/Attack 3")]
public class BurnAction3 : AttackAction
{
    public Burn burn;

    public override void Perform(int targetIndex, List<Combatant> combatants, Dice dice)
    {
        int damage = dice.GetValue();
        
        // If target has burn, then add stacks
        var effect = combatants[targetIndex].GetStatusEffect(burn);
        if (effect != null) {
            damage += effect.stacks;
        }

        combatants[targetIndex].TakeDamage(damage);
    }
}
