using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Item item;
    [SerializeField] private Image itemSprite;

    public void Initialize(Item item) {
        this.item = item;
        itemSprite.sprite = item.sprite;
    }

    private void UpdateVisuals() {
        // TODO
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
}
