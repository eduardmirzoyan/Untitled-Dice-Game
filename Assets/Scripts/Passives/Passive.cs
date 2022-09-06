using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class Passive : ScriptableObject
{
    public new string name;

    [TextArea(10, 20)]
    public string description;
    public Sprite icon;
    public Combatant combatant;
    public Armor sourceArmor;

    public string GetDynamicDescription() {
        // Use regex to insert hyperlinks inplace of fully captial words
        Regex rx = new Regex("\\b[A-Z][A-Z]+");
        string formattedDescription = rx.Replace(description, new MatchEvaluator(InsertHyperlink));

        // string hyperlinkDamageValue = "<link=\"" + baseDamageMultiplier + "x weapon BASE DAMAGE" + "\"><color=yellow>" + FinalDamageValue() + "</color></link>";
        return formattedDescription;
    }

    private string InsertHyperlink(Match m)
    {
        return "<link=" + GameManager.instance.dictionary[m.Value] + "><color=yellow>" + m.Value + "</color></link>";
    }

    public virtual void Initialize(Combatant combatant) {
        this.combatant = combatant;
    }

    public abstract void Terminate();
}
