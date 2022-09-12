using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public abstract class AttackAction : Action
{
    public float baseDamageMultiplier = 1f;

    public virtual int ActionBaseDamage() {
        if (sourceEquipment is Weapon) {
            return (int)(baseDamageMultiplier * ((Weapon) sourceEquipment).baseDamage);
        }
        else {
            return 0;
        }
    }

    public virtual int FinalDamageValue() {
        return (int) (ActionBaseDamage() * sourceCombatant.baseDamageMultiplier);
    }

    public override string GetDynamicDescription()
    {
        string baseText = base.GetDynamicDescription();

        // If the source is a weapon, then replace damage numbers
        if (sourceEquipment is Weapon) {
            string hyperlinkDamageValue = "<link=\"" + baseDamageMultiplier + "x weapon base damage" + "\"><color=yellow>" + ActionBaseDamage() + "</color></link>";
            baseText = baseText.Replace("%%", hyperlinkDamageValue);
        }

        return baseText;
    }
}
