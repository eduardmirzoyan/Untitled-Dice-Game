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
    [SerializeField] private float transitionTime = 3f;
    [SerializeField] private float raisedHeight;
    [SerializeField] private float loweredHeight;
    private Coroutine transitionRoutine;
    private bool isRasied = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if left click and on toggle rect
        if (eventData.button == PointerEventData.InputButton.Left 
            && RectTransformUtility.RectangleContainsScreenPoint(toggleRaiseRect, Input.mousePosition, Camera.main)) {
            // Stop any coroutines first
            if (transitionRoutine != null) StopCoroutine(transitionRoutine);

            // Raise or lower based on current state
            if (isRasied) {
                transitionRoutine = StartCoroutine(LowerUI());
            }
            else {
                transitionRoutine = StartCoroutine(RaiseUI());
            }
        }

        // if you ever right click window, attempt to lower
        if (eventData.button == PointerEventData.InputButton.Right
            && RectTransformUtility.RectangleContainsScreenPoint(lowerRect, Input.mousePosition, Camera.main)) {
    
            // Lower if possible
            if (isRasied) {
                // Stop any coroutines first
                if (transitionRoutine != null) StopCoroutine(transitionRoutine);

                // Lower
                transitionRoutine = StartCoroutine(LowerUI());
            }
        }

    }

    private IEnumerator RaiseUI() {
        isRasied = true;

        float timer = transitionTime;
        while (timer > 0) {
            // Raise UI over time
            var newY = Mathf.Lerp(window.anchoredPosition.y, raisedHeight, 1 - timer / transitionTime);
            window.anchoredPosition = new Vector2(window.anchoredPosition.x, newY);

            // Decrement time
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LowerUI() {
        isRasied = false;

        float timer = transitionTime;
        while (timer > 0) {
            // Raise UI over time
            var newY = Mathf.Lerp(window.anchoredPosition.y, loweredHeight, 1 - timer / transitionTime);
            window.anchoredPosition = new Vector2(window.anchoredPosition.x, newY);

            // Decrement time
            timer -= Time.deltaTime;
            yield return null;
        }
    }

}
