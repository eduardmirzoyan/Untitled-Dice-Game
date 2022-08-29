using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI instance;

    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject window;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;
    private RectTransform windowTransform;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        windowTransform = window.GetComponent<RectTransform>();
    }

    private void Update() {
        int headerLength = header.text.Length;
        int descriptionLength = description.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || descriptionLength > characterWrapLimit);

        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = position;
        
        windowTransform.anchoredPosition = new Vector2(windowTransform.rect.width / 2, windowTransform.rect.height / 2);
    }

    public void Show(string header, string description) {
        window.SetActive(true);
        this.header.text = header;
        this.description.text = description;
    }

    public void Hide() {
        window.SetActive(false);
    }
}
