using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform window;
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private TextMeshProUGUI actionDescription;
    [SerializeField] private Image actionIcon;
    [SerializeField] private Image sourceImage;
    [SerializeField] private Image selfTargetIcon;
    [SerializeField] private Image allyTargetIcon;
    [SerializeField] private Image enemyTargetIcon;
    [SerializeField] private Image lockIcon;

    public void Initialize(Action action, bool showIcon = true) {
        // Move to center
        window.anchoredPosition = new Vector3(0, window.rect.size.y / 2); // window.rect.size.x / 2

        // Update text
        actionName.text = action.name;
        actionDescription.text = action.GetDynamicDescription();

        // Update source
        if (action is AttackAction) {
            AttackAction attackAction = (AttackAction)action;
            if (attackAction.sourceWeapon != null) {
                sourceImage.sprite = attackAction.sourceWeapon.sprite;
                sourceImage.enabled = true;
            }
        }
        else {
            sourceImage.enabled = false;
        }

        // Toggle icons based on what action can do
        selfTargetIcon.gameObject.SetActive(action.canTargetAllies);
        allyTargetIcon.gameObject.SetActive(action.canTargetAllies);
        enemyTargetIcon.gameObject.SetActive(action.canTargetEnemies);

        // Update the icon of the action
        actionIcon.sprite = action.icon;
        if (!showIcon)
            actionIcon.transform.parent.gameObject.SetActive(false);
        
        // Default state is unlocked
        SetLocked(false);
    }

    public void SetLocked(bool state) {
        lockIcon.enabled = state;
    }
}
