using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class SkillDisplayDescriptionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private Image sourceIcon;
    [SerializeField] private Image selfTargetIcon;
    [SerializeField] private Image allyTargetIcon;
    [SerializeField] private Image enemyTargetIcon;
    [SerializeField] private Image passiveIcon;

    [Header("Options")]
    [SerializeField] private bool enableAutoKeywordTooltip;
    [SerializeField] private CanvasGroup keywordCanvasGroup;
    [SerializeField] private RectTransform keywordTransform;
    [SerializeField] private TextMeshProUGUI keywordText;
    [SerializeField] private Vector2 keywordOffset;

    public void InitializeRaw(Skill skill) {
        // Update text
        skillName.text = skill.name;
        skillDescription.text = skill.GetRawDescription();
        
        // Set source as equipment if possible
        if (skill.sourceEquipment != null) {
            sourceIcon.enabled = true;
            sourceIcon.sprite = skill.sourceEquipment.sprite;
        }
        // Set source as combatant if possible
        else if (skill.sourceCombatant != null) {
            sourceIcon.enabled = true;
            sourceIcon.sprite = skill.sourceCombatant.unit.sprite;
        }
        // Else disable icon
        else {
            sourceIcon.enabled = false;
        }

        // Set icons based of if action or passive
        if (skill is Action) {
            selfTargetIcon.gameObject.SetActive(((Action)skill).canTargetAllies);
            allyTargetIcon.gameObject.SetActive(((Action)skill).canTargetAllies);
            enemyTargetIcon.gameObject.SetActive(((Action) skill).canTargetEnemies);
            passiveIcon.gameObject.SetActive(false);
        }
        else if (skill is Passive) {
            selfTargetIcon.gameObject.SetActive(false);
            allyTargetIcon.gameObject.SetActive(false);
            enemyTargetIcon.gameObject.SetActive(false);
            passiveIcon.gameObject.SetActive(true);
        }

        // Automatically show a window will all tooltips defined on the side
        if (enableAutoKeywordTooltip)
        {
            // Extract keywords via regex
            Regex rx = new Regex("\\b[A-Z][A-Z]+");
            var matches = rx.Matches(skill.GetRawDescription());
            string formattedDescription = "";

            // Fill text with keywords and their definitions
            foreach (Match match in matches)
            {
                formattedDescription += "<color=yellow>" + match.Value + "</color>" + ": " + GameManager.instance.dictionary[match.Value] + "\n";
            }

            // If no keywords were found, then don't show this window
            if (formattedDescription.Length == 0)
                return;

            // Set output
            keywordText.text = formattedDescription;

            // Update layout
            LayoutRebuilder.ForceRebuildLayoutImmediate(keywordTransform);

            // Make visible
            keywordCanvasGroup.alpha = 1f;
            // Pin top left of keyword tooltip to top right of this window
            Vector3[] corners = new Vector3[4];
            // Get corners from skill tooltip
            GetComponent<RectTransform>().GetWorldCorners(corners);
            // Top right = corners[2]
            keywordTransform.position = corners[2];
            // Now give offset
            keywordTransform.anchoredPosition += new Vector2(keywordTransform.rect.width / 2, -keywordTransform.rect.height / 2) + keywordOffset;
        }
    }

    public void InitializeDynamic(Action action, Dice dice) {
        // Update text
        skillName.text = action.name;
        skillDescription.text = action.GetDynamicDescription(dice);

        // Set source as equipment if possible
        if (action.sourceEquipment != null)
        {
            sourceIcon.enabled = true;
            sourceIcon.sprite = action.sourceEquipment.sprite;
        }
        // Set source as combatant if possible
        else if (action.sourceCombatant != null)
        {
            sourceIcon.enabled = true;
            sourceIcon.sprite = action.sourceCombatant.unit.sprite;
        }
        // Else disable icon
        else
        {
            sourceIcon.enabled = false;
        }

        selfTargetIcon.gameObject.SetActive(action.canTargetAllies);
        allyTargetIcon.gameObject.SetActive(action.canTargetAllies);
        enemyTargetIcon.gameObject.SetActive(action.canTargetEnemies);
        passiveIcon.gameObject.SetActive(false);
    }

    public void InitializeFinal(Action action) {
        // Update text
        skillName.text = action.name;
        skillDescription.text = action.GetFinalDescription();

        // Set source as equipment if possible
        if (action.sourceEquipment != null)
        {
            sourceIcon.enabled = true;
            sourceIcon.sprite = action.sourceEquipment.sprite;
        }
        // Set source as combatant if possible
        else if (action.sourceCombatant != null)
        {
            sourceIcon.enabled = true;
            sourceIcon.sprite = action.sourceCombatant.unit.sprite;
        }
        // Else disable icon
        else
        {
            sourceIcon.enabled = false;
        }

        selfTargetIcon.gameObject.SetActive(action.canTargetAllies);
        allyTargetIcon.gameObject.SetActive(action.canTargetAllies);
        enemyTargetIcon.gameObject.SetActive(action.canTargetEnemies);
        passiveIcon.gameObject.SetActive(false);
    }
}
