using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        var weapon = (Weapon) item;
        string actionText = "";
        foreach (var action in weapon.actions) {
            actionText += action.name + ": " + action.description + "\n";
        }
        TooltipUI.instance.Show(item.name, "Base damage: " + weapon.baseDamage + "\n"
        + "~~~~~~Actions~~~~~\n"
        + actionText
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
}
