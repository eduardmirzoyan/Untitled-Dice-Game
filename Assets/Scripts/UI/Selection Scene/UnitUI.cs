using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform origin;
    [SerializeField] private Image image;
    [SerializeField] private Image outlineImage;
    [SerializeField] private Outline outline;
    

    [Header("Data")]
    [SerializeField] private Unit unit;
    [SerializeField] private bool isInteractable;
    [SerializeField] private int index;

    // Debugging
    private bool isBeingDragged;
    private Transform currentParent;
    private Transform playerScreen;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        playerScreen = transform.root;
    }

    private void FixedUpdate() {
        if (isBeingDragged) {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;
            transform.position = point;
        }
    }

    public void Initialize(Unit unit, int index, RectTransform parent, bool isInteractable = true) {
        this.unit = unit;
        this.index = index;
        this.isInteractable = isInteractable;
        origin = parent;
        currentParent = parent;
        image.sprite = unit.sprite;
        outlineImage.sprite = unit.sprite;

        image.SetNativeSize();
        outlineImage.SetNativeSize();
    }

    public Unit GetUnit() {
        return unit;
    }

    public void SetParent(Transform transform) {
        currentParent = transform;
    }

    public void SetIndex(int index) {
        this.index = index;
    }

    public void ResetLocation() {
        // Reset parent to origin
        rectTransform.SetParent(origin);
        // Then relocate
        transform.localPosition = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

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
        if (!isInteractable) return;

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
        if (!isInteractable) return;

        // Show outline
        outline.effectColor = Color.white;

        // Show tooltip
        UnitTooltipUI.instance.Show(unit, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable) return;

        // Hide outline
        outline.effectColor = Color.clear;

        // Hide tooltip
        UnitTooltipUI.instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable) return;

        // If left click
        if (eventData.button == PointerEventData.InputButton.Left) {
            // Lock tooltip if possible
            UnitTooltipUI.instance.Lock();
        }

        // If right click
        if (eventData.button == PointerEventData.InputButton.Right && index != -1) {
            // Reset location
            ResetLocation();

            // Trigger event
            SelectionManager.instance.AddUnitToParty(null, index);

            // Remove index
            index = -1;
        }
    }
}
