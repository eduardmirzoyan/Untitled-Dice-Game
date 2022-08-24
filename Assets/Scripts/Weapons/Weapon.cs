using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    // TODO:
    public string weaponName;
    public string description;
    public int baseDamage;
    public List<Action> actions;
}
