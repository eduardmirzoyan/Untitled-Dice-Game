using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine coroutine;

    private void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        CombatEvents.instance.onFeedback += ShowMessage;
    }

    private void ShowMessage(string message) {
        if (coroutine != null) StopCoroutine(coroutine);

        // Show Message
        text.text = message;
        // Start to fade out
        coroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        // Make message visible
        canvasGroup.alpha = 1;
        
        // Wait a bit before fading
        yield return new WaitForSeconds(pauseDuration);

        // Set duration
        float elapsedTime = fadeDuration;
        // Smoothly count down
        while (elapsedTime > 0)
        {
            // Increase alpha from 1 -> 0
            canvasGroup.alpha = elapsedTime / fadeDuration;

            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        // Set alpha to 0
        canvasGroup.alpha = 0;

    }
}
