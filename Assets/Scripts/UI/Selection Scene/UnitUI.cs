using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform origin;
    [SerializeField] private Image image;


    [Header("Settings")]
    [SerializeField] private Unit unit;

    // Debugging
    private bool isBeingDragged;
    private Transform currentParent;
    private Transform playerScreen;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        playerScreen = transform.root;
    }

    private void FixedUpdate() {
        if (isBeingDragged) {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;
            transform.position = point;
        }
    }

    public void Initialize(Unit unit, RectTransform parent) {
        this.unit = unit;
        origin = parent;
        currentParent = parent;
        image.sprite = unit.icon;
        image.SetNativeSize();
    }

    public Unit GetUnit() {
        return unit;
    }

    public void SetParent(Transform transform) {
        currentParent = transform;
    }

    public void ResetLocation()
    {
        // Reset parent to origin
        rectTransform.SetParent(origin);
        // Then relocate
        transform.localPosition = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Start dragging
        isBeingDragged = true;

        // Update visuals
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Save parent
        currentParent = rectTransform.parent;

        // Remove parent
        rectTransform.SetParent(playerScreen);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Do nothing
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Stop dragging
        isBeingDragged = false;

        // Update visuals
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Return parent
        rectTransform.SetParent(currentParent);

        // Reset old parent
        currentParent = null;

        // Reset rotation
        transform.rotation = Quaternion.identity;

        // Reset position
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitDisplayUI.instance.Show(unit, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitDisplayUI.instance.Hide();
    }
}
