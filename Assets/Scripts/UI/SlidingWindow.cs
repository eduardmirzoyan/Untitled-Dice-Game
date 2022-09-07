using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlidingWindow : MonoBehaviour, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private RectTransform window;
    [SerializeField] private RectTransform toggleRaiseRect;
    [SerializeField] private RectTransform lowerRect;

    [Header("Settings")]
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private float transitionTime = 3f;
    [SerializeField] private float raisedHeight;
    [SerializeField] private float loweredHeight;

    [Header("State")]
    [SerializeField] private bool isRasied = false;
    [SerializeField] private bool isReversed;
    private Coroutine transitionRoutine;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if left click and on toggle rect
        if (isInteractable && eventData.button == PointerEventData.InputButton.Left 
            && RectTransformUtility.RectangleContainsScreenPoint(toggleRaiseRect, Input.mousePosition, Camera.main)) {
            // Toggle state
            Toggle();
        }

        // if you ever right click window, attempt to lower
        if (isInteractable && eventData.button == PointerEventData.InputButton.Right
            && RectTransformUtility.RectangleContainsScreenPoint(lowerRect, Input.mousePosition, Camera.main)) {
            // Attempt to lower
            if (isReversed) Raise();
            else Lower();
        }
    }

    public void Toggle() {
        // Stop any coroutines first
        if (transitionRoutine != null) StopCoroutine(transitionRoutine);

        // Raise or lower based on current state
        if (isRasied)
        {
            transitionRoutine = StartCoroutine(LowerUI());
        }
        else
        {
            transitionRoutine = StartCoroutine(RaiseUI());
        }
    }

    public void Raise() {
        // Raise if possible
        if (!isRasied)
        {
            // Stop any coroutines first
            if (transitionRoutine != null) StopCoroutine(transitionRoutine);

            // Raise
            transitionRoutine = StartCoroutine(RaiseUI());
        }
    }

    public void Lower() {
        // Lower if possible
        if (isRasied)
        {
            // Stop any coroutines first
            if (transitionRoutine != null) StopCoroutine(transitionRoutine);

            // Lower
            transitionRoutine = StartCoroutine(LowerUI());
        }
    }

    private IEnumerator RaiseUI() {
        isRasied = true;
        float start = window.anchoredPosition.y;
        float end = raisedHeight;


        float timer = 0;
        while (timer < transitionTime) {
            // Raise UI over time
            var newY = Mathf.Lerp(start, end, timer / transitionTime);
            window.anchoredPosition = new Vector2(window.anchoredPosition.x, newY);

            // Increment time
            timer += Time.deltaTime;
            yield return null;
        }

        // Set final point
        window.anchoredPosition = new Vector2(window.anchoredPosition.x, end);
    }

    private IEnumerator LowerUI() {
        isRasied = false;
        float start = window.anchoredPosition.y;
        float end = loweredHeight;

        float timer = 0;
        while (timer < transitionTime) {
            // Lower UI over time
            var newY = Mathf.Lerp(start, end, timer / transitionTime);
            window.anchoredPosition = new Vector2(window.anchoredPosition.x, newY);

            // Increment time
            timer += Time.deltaTime;
            yield return null;
        }

        // Set final point
        window.anchoredPosition = new Vector2(window.anchoredPosition.x, end);
    }

}
