using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FloatingNumberUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Rigidbody2D body;

    [SerializeField] private float holdDuration;
    [SerializeField] private float fadeDuration;

    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;

    public void Initialize(string damageText, Color color) {
        // Change text
        text.text = damageText;
        text.color = color;

        // Get random velocity
        float velocity = Random.Range(minVelocity, maxVelocity);

        // Give upward momentum
        body.velocity = new Vector2(velocity, velocity * 0.33f);
        
        // Destroy itself in 1 sec
        Destroy(gameObject, fadeDuration + holdDuration);

        // Start to fade out
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        var mainColor = text.color;

        // Wait
        yield return new WaitForSeconds(holdDuration);

        float timer = fadeDuration;
        while (timer > 0) {
            // Reduce alpha
            mainColor.a = timer / fadeDuration;

            // Update color
            text.color = mainColor;

            // Decrement time
            timer -= Time.deltaTime;
            yield return null;
        }

        text.color = Color.clear;
    }
}
