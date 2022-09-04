using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthbarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    // Stores entity its tracking
    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(int maxHealth, int currentHealth) {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        text.text = slider.value + "/" + slider.maxValue;
    }

    public void UpdateHealth(int currentHealth) {
        slider.value = currentHealth;
        text.text = slider.value + "/" + slider.maxValue;
    }

}
