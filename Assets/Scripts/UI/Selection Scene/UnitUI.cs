using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    [Header("Settings")]
    [SerializeField] private Unit unit;

    // Debugging
    private bool isBeingDragged;
    private Transform currentParent;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void FixedUpdate() {
        if (isBeingDragged) {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;
            transform.position = point;
        }
    }

    public Unit GetUnit() {
        return unit;
    }

    public void SetParent(Transform transform) {
        currentParent = transform;
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
        rectTransform.SetParent(GameManager.instance.playerScreen.transform);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        // Show character info?
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
