using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private bool isPaused;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }

        isPaused = false;
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(pauseKey)) {
            if (!isPaused) Pause();
            else Resume();
        }
    }

    public void Pause() {
        isPaused = true;
        
        // Stop time
        Time.timeScale = 0f;

        // Make menu visible
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Resume() {
        isPaused = false;

        // Start time
        Time.timeScale = 1f;

        // Make menu invisible
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void MainMenu() {
        // Resume first
        Resume();

        // Clear player party
        GameManager.instance.ClearPlayerParty();
        
        // Load main menu
        TransitionManager.instance.LoadMainMenu();
    }

    public void Quit() {
        Debug.Log("Player quit game.");
        Application.Quit();
    }

}
