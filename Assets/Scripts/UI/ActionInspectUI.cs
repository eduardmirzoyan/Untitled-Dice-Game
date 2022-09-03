using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionInspectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Action action;
    [SerializeField] private Passive passive;
    [SerializeField] private GameObject actionUIPrefab;
    [SerializeField] private RectTransform displayLocation;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isLocked;
    private ActionUI actionUI;

    public void Initialize(Action action) {
        this.action = action;
    }

    public void Initialize(Passive passive)
    {
        this.passive = passive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If the display is visible
        if (isVisible) {
            // Toggle lock state
            isLocked = !isLocked;

            // Update visuals
            actionUI.SetLocked(isLocked);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Don't reshow if locked
        if (isLocked) return;

        // Spawn preview
        actionUI = Instantiate(actionUIPrefab, displayLocation.transform).GetComponent<ActionUI>();
        if (action != null) {
            actionUI.Initialize(action);
        }
        else if (passive != null) {
            actionUI.Initialize(passive);
        }
        

        isVisible = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Don't remove if it's locked
        if (isLocked) return;

        // Destroy preview if possible
        if (isVisible && actionUI != null)
        {
            Destroy(actionUI.gameObject);
            isVisible = false;
        }
    }
}