using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake() {
        button = GetComponent<Button>();
        button.interactable = false;
    }

    private void Start() {
        GameEvents.instance.onPartyFull += OnPartyFull;
    }

    private void OnDestroy() {
        GameEvents.instance.onPartyFull -= OnPartyFull;
    }

    private void OnPartyFull(bool state) {
        button.interactable = state;
    }
}
