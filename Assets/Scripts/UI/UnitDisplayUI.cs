using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitDisplayUI : MonoBehaviour
{
    public static UnitDisplayUI instance;

    [Header("Displaying Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image lockImage;
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private DiceUI unitDiceUI;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI speedText;

    [Header("Adjustments")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isLocked;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        lockImage.enabled = false;
    }

    private void Update() {
        if (isLocked && Input.GetMouseButtonDown(1)) {
            isLocked = false;
            lockImage.enabled = false;

            // Remove interact
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Hide();
        }
    }

    public void Lock() {
        // Make sure the display is active
        if (isVisible) {
            // Make sure it doesn't go away when you hover out
            isLocked = true;
            lockImage.enabled = true;

            // Add interact
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Show(Unit unit, Vector3 position) {
        // Don't reshow if locked
        if (isLocked) return;

        // Move transform
        transform.position = position + offset;

        // Update basic stats
        unitName.text = unit.name;
        healthText.text = unit.GetHealthStatus();
        speedText.text = unit.speed.ToString();

        // Update die
        unitDiceUI.DrawValue(unit.dice.maxValue);

        // Display window
        canvasGroup.alpha = 1f;

        isVisible = true;
    }

    public void Hide() {
        // Don't hide if it's locked
        if (isLocked) return;

        // Then disable window
        canvasGroup.alpha = 0f;

        isVisible = false;
    }
}
