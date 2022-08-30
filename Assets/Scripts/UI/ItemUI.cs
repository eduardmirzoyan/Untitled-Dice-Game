using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Item item;
    [SerializeField] private Image itemSprite;
    [SerializeField] private bool isInteractable;
    private bool isBeingDragged;
    private Transform currentParent;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void FixedUpdate() {
        if (isBeingDragged) {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;
            transform.position = point;
        }
    }

    public void Initialize(Item item) {
        this.item = item;
        itemSprite.sprite = item.sprite;
        isInteractable = true;
    }

    public void SetParent(Transform transform) {
        currentParent = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);
        ItemDisplayUI.instance.Show(item, corners[3]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDisplayUI.instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemDisplayUI.instance.Lock();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isInteractable && eventData.button == PointerEventData.InputButton.Left) {
            // Update visuals
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            // Save parent
            currentParent = rectTransform.parent;

            // Remove parent
            rectTransform.SetParent(GameManager.instance.playerScreen.transform);

            isBeingDragged = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isInteractable) {
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Nothing
    }
}
