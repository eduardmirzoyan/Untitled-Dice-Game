using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTooltipUI : MonoBehaviour
{
    public static TextTooltipUI instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;
    [SerializeField] private RectTransform windowTransform;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        canvasGroup = GetComponentInChildren<CanvasGroup>();
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
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        this.header.text = header;
        this.description.text = description;
    }

    public void Hide() {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
