using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public new string name;
    
    [TextArea(10, 20)]
    public string description;
    public Sprite sprite;
}
