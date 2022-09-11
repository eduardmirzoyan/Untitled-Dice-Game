using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingNumber : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Vector2 randomOffset;
    [SerializeField] private float aliveTime = 1.25f;

    public void Initialize(string damageText, Color color) {
        // Change text
        text.text = damageText;
        text.color = color;

        transform.position = (Vector2) transform.position + Vector2.one * Random.Range(randomOffset.x, randomOffset.y);

        // Destroy itself in time
        Destroy(gameObject, aliveTime);
    }
}
