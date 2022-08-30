using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackAction : Action
{
    public float baseDamageMultiplier = 1f;
    public Weapon sourceWeapon = null;

    public virtual int FinalDamageValue()
    {
        return (int)(baseDamageMultiplier * sourceWeapon.baseDamage);
    }

    public override string GetDynamicDescription()
    {
        string hyperlinkDamageValue = "<link=\"" + baseDamageMultiplier + "x weapon BASE DAMAGE" + "\"><color=yellow>" + FinalDamageValue() + "</color></link>";
        return description.Replace("%%", hyperlinkDamageValue);
    }
}
