using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DicePool : ScriptableObject
{
    public List<Dice> dice;

    public bool IsOrdered() {
        return false;
    }

    public int OddCount() {
        return dice.Sum(die => die.IsOdd() ? 1 : 0);
    }

    public int EvenCount() {
        return dice.Sum(die => die.IsEven() ? 1 : 0);
    }
}
