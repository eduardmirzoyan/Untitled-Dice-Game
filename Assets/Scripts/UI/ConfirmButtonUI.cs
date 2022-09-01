using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ConfirmButtonUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool isVisible = false;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        
    }

    private void Show() {
        // Show UI
        canvasGroup.alpha = 1;
        isVisible = true;
    }

    private void Hide() {
        // Hide UI
        canvasGroup.alpha = 0;
        isVisible = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If the button is visible and pressed by left click
        if (isVisible && eventData.button == PointerEventData.InputButton.Left) {
            // Confirm action
            CombatManager.instance.Confirm();
        }
    }
}
