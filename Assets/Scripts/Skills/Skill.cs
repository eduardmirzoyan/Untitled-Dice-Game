using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class Skill : ScriptableObject
{
    public new string name;

    [TextArea(10, 20)]
    [SerializeField] protected string description;
    public Sprite icon;
    public Equipment sourceEquipment;
    public Combatant sourceCombatant;

    protected Regex rx = new Regex("[A-Z][A-Z]+\\b");

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

    public virtual string GetRawDescription()
    {
        // Get base raw description
        // string rawText = description; //base.GetRawDescription();
        // Replace text with die icons where needed
        return rx.Replace(description, new MatchEvaluator(InsertIcons));
    }

    // public virtual string GetRawDescription() {
    //     //return description;
    //     // This gets the description of the skill replacing words

    //     // Use regex to insert hyperlinks inplace of fully captialized words
    //     Regex rx = new Regex("\\b[A-Z][A-Z]+");
        
    //     return rx.Replace(description, new MatchEvaluator(InsertHyperlink));
    // }

    public virtual string GetHighlightedDescription() {
        // This gets the description of the skill replacing words

        // Use regex to insert hyperlinks inplace of fully captialized words
        Regex rx = new Regex("\\b[A-Z][A-Z]+");

        return rx.Replace(description, new MatchEvaluator(InsertHyperlink));
    }

    protected virtual string InsertIcons(Match m)
    {
        // If word is a keyword then replace it with hyperlink
        if (GameManager.instance.dictionary.ContainsKey(m.Value)) {
            return InsertHyperlink(m);
        }
        // Set the icon
        return "<sprite=\"" + m.Value + "\" index=0>";
    }

    protected string InsertHyperlink(Match m) {
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
