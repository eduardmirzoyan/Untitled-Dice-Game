using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    [TextArea(10, 20)]
    public string itemDescription;
    public Sprite sprite;
}
