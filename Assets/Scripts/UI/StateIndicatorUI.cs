using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateIndicatorUI : MonoBehaviour
{
    [SerializeField] private static float fadeDuration = 0.5f;
    [SerializeField] private static float pauseDuration = 0.5f;
    [SerializeField] private RectTransform main;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI textBox;

    private Coroutine routine;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        CombatEvents.instance.onShowBanner += ShowBanner;
        CombatEvents.instance.onHideBanner += HideBanner;

        // CombatEvents.instance.onCombatStart += StartCombat;
        // CombatEvents.instance.onRoundStart += StartRound;
        // CombatEvents.instance.onTurnStart += StartTurn;
        // CombatEvents.instance.onRoundEnd += EndRound;
        // CombatEvents.instance.onCombatEnd += EndCombat;
    }

    private void StartCombat(ActionInfo info) {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(EnterState("Combat Start"));
    }

    private void StartRound(ActionInfo info)
    {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(EnterState("Round Start"));
    }

    private void StartTurn(ActionInfo info, Combatant combatant)
    {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(EnterState("Turn Start: " + combatant.unit.name));
    }

    private void EndRound(ActionInfo info)
    {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(EnterState("Round End"));
    }

    private void EndCombat(ActionInfo info)
    {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(EnterState("Combat End"));
    }

    public void ShowBanner(string message) {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(EnterState(message));
    }

    public void HideBanner(string message) {
        if (routine != null) StopCoroutine(routine);

        routine = StartCoroutine(ExitState());
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

        // // Wait for a bit before fading out
        // yield return new WaitForSeconds(pauseDuration);

        // // Now exit
        // yield return ExitState();
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
