using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Canvas playerScreen; // Stores the player's screen for other classes to get

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        
    }


}
