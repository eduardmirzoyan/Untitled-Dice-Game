using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;

    [SerializeField] private Text header;
    [SerializeField] private Text description;

    [SerializeField] private GameObject window;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Update() {
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = position;
    }

    public void UpdateTooltip(string header, string description) {
        this.header.text = header;
        this.description.text = description;
    }

    public void Show(string header, string description) {
        window.SetActive(true);
        UpdateTooltip(header, description);
    }

    public void Hide() {
        window.SetActive(false);
    }
}
