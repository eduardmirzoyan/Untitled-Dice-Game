using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class Passive : Skill
{
    // public new string name;

    // [TextArea(10, 20)]
    // public string description;
    // public Sprite icon;
    // public Combatant sourceCombatant;
    // public Armor sourceEquipment;

    // public string GetDynamicDescription() {
    //     // Use regex to insert hyperlinks inplace of fully captial words
    //     Regex rx = new Regex("\\b[A-Z][A-Z]+");

    //     return rx.Replace(description, new MatchEvaluator(InsertHyperlink));
    // }

    // private string InsertHyperlink(Match m) {
    //     return "<link=" + GameManager.instance.dictionary[m.Value] + "><color=yellow>" + m.Value + "</color></link>";
    // }

    // public virtual void Initialize(Combatant combatant) {
    //     this.sourceCombatant = combatant;
    // }

    // public abstract void Uninitialize();
}
