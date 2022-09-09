using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class SkillTooltipUI : MonoBehaviour
{
    public static SkillTooltipUI instance;

    [Header("Components")]
    [SerializeField] private SkillDisplayDescriptionUI skillDisplayDescriptionUI;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Icons")]
    [SerializeField] private Image lockIcon;

    [Header("Adjustments")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isLocked;


    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        canvasGroup = GetComponentInChildren<CanvasGroup>();
        lockIcon.enabled = false;
    }

    private void Update() {
        // Right click unlocks if possible
        if (isLocked && Input.GetMouseButtonDown(1)) {
            // Unlock
            isLocked = false;
            lockIcon.enabled = false;

            // Remove interact
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Hide();
        }
    }

    public void Show(Action action, Vector3 position) {
        // Don't reshow if locked
        if (isLocked) return;

        // Move transform
        transform.position = position + offset;

        // Initialize
        skillDisplayDescriptionUI.Initialize(action);

        // Display window
        canvasGroup.alpha = 1f;

        // Set visible
        isVisible = true;
    }

    public void Show(Passive passive, Vector3 position) {
        // Don't reshow if locked
        if (isLocked) return;

        // Move transform
        transform.position = position + offset;

        // Initialize
        skillDisplayDescriptionUI.Initialize(passive);

        // Display window
        canvasGroup.alpha = 1f;

        // Set visible
        isVisible = true;
    }

    public void Lock() {
        // Make sure the display is active
        if (isVisible) {
            // Make sure it doesn't go away when you hover out
            isLocked = true;
            lockIcon.enabled = true;

            // Add interact
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Hide() {
        // Don't hide if it's locked
        if (isLocked) return;

        // Then disable window
        canvasGroup.alpha = 0f;

        isVisible = false;
    }
}
