using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class Dice : ScriptableObject, IComparable<Dice>
{
    public int maxValue = 6;
    [SerializeField] private int value;

    private void Awake() {
        if (maxValue < 1) {
            throw new System.Exception("Error: Dice is negative or 0");
        }

        // Set default value to 1
        value = 1;
    }

    public int Roll() {
        // Set value
        value = UnityEngine.Random.Range(1, maxValue + 1);

        // Return value of roll
        return value;
    }

    public void Grow() {
        value = value % maxValue + 1;
    }

    public void Shrink() {
        value -= 1;
        if (value == 0) value = maxValue;
    }

    public int GetExpectedValue() {
        // CS70 coming in clutch
        int result = 0;
        for (int i = 1; i <= maxValue; i++) {
            result += i;
        }
        return result / maxValue;
    }

    public void SetValue(int value) {
        if (value > maxValue || value < 1) {
            throw new System.Exception("Dice " + name + " was set to an improper value: " + value);
        }
        this.value = value;
    }

    public int GetValue() {
        return value;
    }

    public int GetInvertedValue() {
        return maxValue - value + 1;
    }

    public bool IsHighroll() {
        return value == maxValue;
    }

    public bool IsLowroll() {
        return value == 1;
    }

    public bool IsEven() {
        return value % 2 == 0;
    }

    public bool IsOdd() {
        return !IsEven();
    }

    public int CompareTo(Dice other)
    {
        return GetValue().CompareTo(other.GetValue());
    }
}
