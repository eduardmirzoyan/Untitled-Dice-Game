using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Dice Pool")]
public class DicePool : ScriptableObject
{
    public List<Dice> dice;
    public int maxSize = 4;
    
    private void Awake() {
        dice = new List<Dice>();
        for (int i = 0; i < maxSize; i++)
        {
            dice.Add(null);
        }
    }

    public void Initialize() {
        dice = new List<Dice>();
        for (int i = 0; i < maxSize; i++)
        {
            dice.Add(null);
        }
    }

    public Dice this[int index] {
        get { return dice[index]; }
        set { dice[index] = value; }
    }

    public void Add(Dice die, int index) {
        // Error checking
        if (index >= maxSize || index < 0)
        {
            Debug.Log("Tried to add unit to an invalid index: " + index);
            return;
        }

        // Add die to pool
        dice[index] = die;
    }

    public List<Dice> GetDice()
    {
        return dice;
    }

    public Dice GetSmallest() {
        return dice.Min();
    }

    public Dice GetLargest() {
        return dice.Max();
    }

    public bool IsOrdered() {
        return false;
    }

    public bool IsUnique() {
        return dice.All(die => dice.Count(die2 => die2.GetValue() == die.GetValue()) == 1);
    }

    public bool HasPair() {
        return dice.Any(die => dice.Count(die2 => die2.GetValue() == die.GetValue()) > 1);
    }

    public Dice GetFirstOdd() {
        return dice.First(die => die.IsOdd());
    }

    public Dice GetFirstEven() {
        return dice.First(die => die.IsEven());
    }

    public int OddCount() {
        return dice.Sum(die => die.IsOdd() ? 1 : 0);
    }

    public int EvenCount() {
        return dice.Sum(die => die.IsEven() ? 1 : 0);
    }
}
