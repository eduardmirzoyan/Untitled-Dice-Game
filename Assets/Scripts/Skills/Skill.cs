using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class Skill : ScriptableObject
{
    public new string name;

    [TextArea(10, 20)]
    [SerializeField] private string description;
    public Sprite icon;

    public Equipment sourceEquipment;
    public Combatant sourceCombatant;

    public void Initialize(Combatant combatant) {
        sourceCombatant = combatant;
        Init();
    }

    protected virtual void Init() {
        // This is what should be overwritten by super classes
    }

    protected virtual void Uninit() {
        // This is what should be overwritten by super classes
    }

    public void Uninitialize() {
        Uninit();
        sourceCombatant = null;
    }

    public virtual string GetDynamicDescription() {
        // Use regex to insert hyperlinks inplace of fully captialized words
        Regex rx = new Regex("\\b[A-Z][A-Z]+");
        
        // INCORPERATE THIS LATER?
        //     string hyperlinkDamageValue = "<link=\"" + baseDamageMultiplier + "x weapon BASE DAMAGE" + "\"><color=yellow>" + ActionBaseDamage() + "</color></link>";
        //     return formattedDescription.Replace("%%", hyperlinkDamageValue);

        return rx.Replace(description, new MatchEvaluator(InsertHyperlink));
    }

    private string InsertHyperlink(Match m) {
        return "<link=" + GameManager.instance.dictionary[m.Value] + "><color=yellow>" + m.Value + "</color></link>";
    }

    public Skill Copy() {
        // Make a copy
        Skill copy = Instantiate(this);

        // Set sources
        copy.sourceEquipment = sourceEquipment;
        copy.sourceCombatant = sourceCombatant;
        
        return copy;
    }
}
