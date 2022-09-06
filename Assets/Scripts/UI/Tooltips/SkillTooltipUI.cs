using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTooltipUI : MonoBehaviour
{
    public static SkillTooltipUI instance;

    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image sourceIcon;

    [Header("Icons")]
    [SerializeField] private Image selfTargetIcon;
    [SerializeField] private Image allyTargetIcon;
    [SerializeField] private Image enemyTargetIcon;
    [SerializeField] private Image passiveIcon;
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

        // Update text
        skillName.text = action.name;
        skillDescription.text = action.GetDynamicDescription();

        // Update source
        if (action is AttackAction) {
            AttackAction attackAction = (AttackAction)action;
            if (attackAction.sourceWeapon != null) {
                sourceIcon.sprite = attackAction.sourceWeapon.sprite;
                sourceIcon.enabled = true;
            }
        }
        else {
            sourceIcon.enabled = false;
        }

        // Toggle icons based on what action can do
        selfTargetIcon.gameObject.SetActive(action.canTargetAllies);
        allyTargetIcon.gameObject.SetActive(action.canTargetAllies);
        enemyTargetIcon.gameObject.SetActive(action.canTargetEnemies);
        passiveIcon.gameObject.SetActive(false);

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

        // Update text
        skillName.text = passive.name;
        skillDescription.text = passive.GetDynamicDescription();

        // Update source
        if (passive.sourceArmor != null) {
            sourceIcon.sprite = passive.sourceArmor.sprite;
            sourceIcon.enabled = true;
        }
        else {
            sourceIcon.enabled = false;
        }

        // Toggle icons based on what action can do
        selfTargetIcon.gameObject.SetActive(false);
        allyTargetIcon.gameObject.SetActive(false);
        enemyTargetIcon.gameObject.SetActive(false);
        passiveIcon.gameObject.SetActive(true);

        // Update the icon of the action
        skillIcon.sprite = passive.icon;

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
