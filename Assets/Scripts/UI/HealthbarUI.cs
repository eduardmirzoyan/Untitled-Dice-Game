using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] public Slider slider;

    // Stores entity its tracking

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Initialize(int maxHealth, int currentHealth) {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void UpdateHealth(int currentHealth) {
        slider.value = currentHealth;
    }

}
