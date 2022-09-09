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
    [SerializeField] private ItemSlotUI itemSlotUI;
    private bool isBeingDragged;
    private Transform currentParent;
    private Transform playerScreen;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
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

    public void Initialize(Item item, ItemSlotUI itemSlotUI) {
        this.item = item;
        this.itemSlotUI = itemSlotUI;
        itemSprite.sprite = item.sprite;
        isInteractable = true;
    }

    public Item GetItem() {
        return item;
    }

    public void SetItemSlot(ItemSlotUI itemSlotUI) {
        this.itemSlotUI = itemSlotUI;
    }

    public ItemSlotUI GetItemSlotUI() {
        return itemSlotUI;
    }

    public void SetParent(Transform transform) {
        currentParent = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Display item info
        Vector3[] corners = new Vector3[4];
        // Get all corners of item boundry
        GetComponent<RectTransform>().GetWorldCorners(corners);
        // Send bottom right corner as position
        ItemTooltipUI.instance.Show(item, corners[3]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // HIde item info
        ItemTooltipUI.instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Lock item info
        ItemTooltipUI.instance.Lock();
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
            rectTransform.SetParent(playerScreen);

            isBeingDragged = true;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Nothing
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
}
