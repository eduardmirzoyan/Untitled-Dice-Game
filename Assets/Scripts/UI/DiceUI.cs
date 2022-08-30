using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    // MODULARIZE CLASS!

    public static float rollDuration = 0.1f;

    [Header("Visuals")]
    [SerializeField] private Image background;

    [Header("Components")]
    [SerializeField] private Dice dice;
    [SerializeField] private Animator animator;
    [SerializeField] private Image[] pipHolders;

    [SerializeField] private RectTransform origin;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform currentParent;

    [SerializeField] private float travelRate = 0.1f;
    [SerializeField] private float spinRate = 3f;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool isActive;

    private Coroutine rollRoutine;
    private bool isBeingDragged;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // REMOVE THIS CLASS FOR IMPROVEMENTS!!! ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    private void Update() {
        // For debugging
        if (Input.GetKeyDown(KeyCode.F)) {
            Roll();
        }
        // For debugging
        if (Input.GetKeyDown(KeyCode.L)) {
            Replenish();
        }
    }

    public void Initialize(Dice dice, Color color, RectTransform origin, bool interactable = false) {
        this.dice = dice;
        this.origin = origin;
        this.isInteractable = interactable;
        background.color = color;
        isActive = true;

        // Display the current state of the die
        UpdateVisuals();
    }

    public void SetInteractive(bool state) {
        isInteractable = state;
    }

    public void Roll() {
        if (rollRoutine != null) {
            StopCoroutine(rollRoutine);
        }
        rollRoutine = StartCoroutine(RollVisuals(rollDuration));
    }

    public void Replenish() {
        isActive = true;
        UpdateVisuals();
    }

    public void Grow() {
        dice.Grow();
        UpdateVisuals();
        
        // Visual feedback
        animator.Play("Selected");
    }

    public void Shrink() {
        dice.Shrink();
        UpdateVisuals();

        // Visual feedback
        animator.Play("Selected");
    }

    private void UpdateVisuals() {
        // Draw the physical die
        ValueToUI(dice.GetValue());  

        // Update transparancy depending if die is active
        if (isActive) {
            canvasGroup.alpha = 1f;
        }
        else {
            canvasGroup.alpha = 0.5f;
        }
    }

    public Dice GetDice() {
        return dice;
    }

    private IEnumerator RollVisuals(float duration) {

        float elapsedTime = 0;
        // Smoothly move to target
        while (elapsedTime < duration)
        {
            // Lerp target item to its spot
            dice.Roll();

            // Update visuals
            UpdateVisuals();

            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        animator.Play("Selected");
    }

    private void ValueToUI(int value) {
        if (pipHolders == null) {
            throw new System.Exception("Dice render not set");
        }

        // Display certain pips depending on die value
        switch (value) {
            case 1:
            pipHolders[0].enabled = false;
            pipHolders[1].enabled = true;
            pipHolders[2].enabled = false;
            pipHolders[3].enabled = false;
            pipHolders[4].enabled = false;
            pipHolders[5].enabled = false;
            pipHolders[6].enabled = false;
            break;

            case 2:
            pipHolders[0].enabled = true;
            pipHolders[1].enabled = false;
            pipHolders[2].enabled = true;
            pipHolders[3].enabled = false;
            pipHolders[4].enabled = false;
            pipHolders[5].enabled = false;
            pipHolders[6].enabled = false;
            break;

            case 3:
            pipHolders[0].enabled = true;
            pipHolders[1].enabled = true;
            pipHolders[2].enabled = true;
            pipHolders[3].enabled = false;
            pipHolders[4].enabled = false;
            pipHolders[5].enabled = false;
            pipHolders[6].enabled = false;
            break;

            case 4:
            pipHolders[0].enabled = true;
            pipHolders[1].enabled = false;
            pipHolders[2].enabled = true;
            pipHolders[3].enabled = true;
            pipHolders[4].enabled = true;
            pipHolders[5].enabled = false;
            pipHolders[6].enabled = false;
            break;

            case 5:
            pipHolders[0].enabled = true;
            pipHolders[1].enabled = true;
            pipHolders[2].enabled = true;
            pipHolders[3].enabled = true;
            pipHolders[4].enabled = true;
            pipHolders[5].enabled = false;
            pipHolders[6].enabled = false;
            break;

            case 6:
            pipHolders[0].enabled = true;
            pipHolders[1].enabled = false;
            pipHolders[2].enabled = true;
            pipHolders[3].enabled = true;
            pipHolders[4].enabled = true;
            pipHolders[5].enabled = true;
            pipHolders[6].enabled = true;
            break;

            default:
            throw new System.Exception("Dice value is not displayable: " + dice.GetValue());
        }
    }

    // Dragging helper functions
    public void OnBeginDrag(PointerEventData eventData) {
        if (isInteractable) {
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

    private void FixedUpdate() {
        if (isBeingDragged) {
            FollowAndRotate();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        // Does nothing so far
    }

    public void OnEndDrag(PointerEventData eventData) {
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

            UpdateVisuals();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        // Return to origin on right click
        if (eventData.button == PointerEventData.InputButton.Right) {
            // If it is currently in a dice slot, attempt to remove itself
            if (transform.parent.TryGetComponent(out DiceSlotUI diceSlotUI)) {
                // Deselect the action
                CombatManager.instance.SelectAction(null, null);
            }
        }
    }

    public void ResetLocation() {
        // Reset parent to origin
        rectTransform.SetParent(origin);
        // Then relocate
        transform.localPosition = Vector3.zero;
    }

    public void SetParent(Transform transform) {
        currentParent = transform;
    }

    public void SetActive(bool active) {
        isActive = active;
        // Update visuals
        UpdateVisuals();
    }

    public bool IsActive() {
        return isActive;
    }

    public void DisplayValue(int value) {
        ValueToUI(value);
    }

    private void FollowAndRotate() {
        // Make Die smoothly travel towards mouse
        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        transform.position = Vector3.Lerp(transform.position, point, travelRate);

        // Rotate Die
        transform.Rotate(0, 0, spinRate); //rotates 50 degrees per second around z axis
    }
}
