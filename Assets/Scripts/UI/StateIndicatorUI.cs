using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateIndicatorUI : MonoBehaviour
{
    public static float fadeDuration = 0.5f;
    [SerializeField] private RectTransform main;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text textBox;

    private void Awake() {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        textBox = GetComponentInChildren<Text>();
    }

    public IEnumerator EnterState(string text) {
        textBox.text = text;

        // Set duration
        float elapsedTime = fadeDuration;
        // Smoothly count down
        while (elapsedTime > 0)
        {
            // Increase alpha from 0 -> 1
            canvasGroup.alpha = 1 - elapsedTime / fadeDuration;
            // Slide from left to center
            main.localPosition = Vector3.right * -50 * elapsedTime / fadeDuration;

            elapsedTime -= Time.deltaTime;
            yield return null;
        }
        main.localPosition = Vector3.zero;
        canvasGroup.alpha = 1;
    }

    public IEnumerator ExitState() {
        // Set duration
        float elapsedTime = fadeDuration;
        // Smoothly count down
        while (elapsedTime > 0)
        {
            // Increase alpha from 0 -> 1
            canvasGroup.alpha = elapsedTime / fadeDuration;
            // Slide from center to right
            main.localPosition = Vector3.right * 50 * (1 - elapsedTime / fadeDuration);

            elapsedTime -= Time.deltaTime;
            yield return null;
        }
        main.localPosition = Vector3.right * 50;
        canvasGroup.alpha = 0;
    }
}
