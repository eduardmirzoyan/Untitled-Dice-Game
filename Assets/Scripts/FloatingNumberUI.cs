using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FloatingNumberUI : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private TextMesh textShadowMesh;
    [SerializeField] private Rigidbody2D body;

    [SerializeField] private float holdDuration;
    [SerializeField] private float fadeDuration;

    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;

    public void Initialize(string damageText) {
        // Change text values
        textMesh.text = damageText;
        textShadowMesh.text = damageText;

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
        var mainColor = textMesh.color;
        var backgroundColor = textShadowMesh.color;

        // Wait
        yield return new WaitForSeconds(holdDuration);

        float timer = fadeDuration;
        while (timer > 0) {
            // Reduce alpha
            mainColor.a = timer / fadeDuration;
            backgroundColor.a = timer / fadeDuration;

            // Update color
            textMesh.color = mainColor;
            textShadowMesh.color = backgroundColor;

            // Decrement time
            timer -= Time.deltaTime;
            yield return null;
        }

        textMesh.color = Color.clear;
        textShadowMesh.color = Color.clear;
    }
}
