using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


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
        // Use regex to insert hyperlinks inplace of fully captial words
        Regex rx = new Regex("\\b[A-Z][A-Z]+");
        string formattedDescription = rx.Replace(description, new MatchEvaluator(InsertHyperlink));

        string hyperlinkDamageValue = "<link=\"" + baseDamageMultiplier + "x weapon BASE DAMAGE" + "\"><color=yellow>" + FinalDamageValue() + "</color></link>";
        return formattedDescription.Replace("%%", hyperlinkDamageValue);
    }

    private string InsertHyperlink(Match m)
    {
        return "<link=" + GameManager.instance.dictionary[m.Value] + "><color=yellow>" + m.Value + "</color></link>";
    }
}
