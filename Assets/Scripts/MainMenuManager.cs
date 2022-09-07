using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [SerializeField] private HelpUI helpUI;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void StartGame() {
        TransitionManager.instance.LoadNextScene();
    }

    public void QuitGame() {
        Debug.Log("Player quit game.");
        Application.Quit();
    }
}
