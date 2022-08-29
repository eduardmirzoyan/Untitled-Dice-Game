using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;

    [TextArea(10, 20)]
    [SerializeField] private string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.instance.Show(header, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.instance.Hide();
    }
}
