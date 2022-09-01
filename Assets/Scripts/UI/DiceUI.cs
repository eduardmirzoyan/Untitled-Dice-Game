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
    [SerializeField] private Image selectionOutline;

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
    private int rotationDirection = 1;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // REMOVE THIS CLASS FOR IMPROVEMENTS!!! ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void Initialize(Dice dice, Color color, RectTransform origin, bool interactable = false)
    {
        this.dice = dice;
        this.origin = origin;
        this.isInteractable = interactable;
        background.color = color;
        isActive = true;

        // Subscribe to events
        CombatEvents.instance.onRoll += OnRoll;
        CombatEvents.instance.onGrow += OnGrow;
        CombatEvents.instance.onShrink += OnShrink;
        CombatEvents.instance.onSetActive += OnSetActive;

        // Display the current state of the die
        UpdateVisuals();
    }

    private void OnRoll(Dice dice) {
        // If this die was rolled
        if (this.dice == dice) {
            Roll();
        }
    }

    private void OnGrow(Dice dice) {
        // If this die was rolled
        if (this.dice == dice) {
            // Update visuals
            DrawValue(dice.GetValue());

            // Do green animation
            selectionOutline.color = Color.green;
            animator.Play("Selected"); // MAKE THIS GREEN
        }
    }

    private void OnShrink(Dice dice) {
        // If this die was rolled
        if (this.dice == dice) {
            // Update visuals
            DrawValue(dice.GetValue());

            // Do green animation
            selectionOutline.color = Color.red;
            animator.Play("Selected"); // MAKE THIS GREEN
        }
    }

    private void OnSetActive(Dice dice, bool state)
    {
        // If this die was rolled
        if (this.dice == dice)
        {
            // Change state of the die
            isActive = state;
            canvasGroup.alpha = isActive ? 1f : 0.5f;
        }
    }

    private void Update() {
        // For debugging
        if (Input.GetKeyDown(KeyCode.F)) {
            Roll();
        }
    }

    public void SetInteractive(bool state) {
        isInteractable = state;
    }

    public void SetActive(bool state) {
        isActive = state;
        UpdateVisuals();
    }

    public void Roll() {
        if (rollRoutine != null) {
            StopCoroutine(rollRoutine);
        }
        rollRoutine = StartCoroutine(RollVisuals(rollDuration));
    }

    private IEnumerator RollVisuals(float duration)
    {
        float elapsedTime = 0;
        // Randomly roll through numbers and then finally land on the final die value

        // Smoothly move to target
        while (elapsedTime < duration)
        {
            // Lerp target item to its spot
            int random = Random.Range(1, dice.maxValue + 1);

            // Draw the die value
            DrawValue(random);

            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        // At the end draw the true number that was rolled
        DrawValue(dice.GetValue());

        // Visual feedback
        animator.Play("Selected");
    }

    private void UpdateVisuals() {
        // Draw the physical die
        DrawValue(dice.GetValue());

        // Update transparancy depending if die is active
        canvasGroup.alpha = isActive ? 1f : 0.5f;
    }

    public Dice GetDie() {
        return dice;
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
            rotationDirection = Random.Range(0, 2) == 0 ? 1 : -1;
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
            if (transform.parent.TryGetComponent(out ActionHolderUI actionHolderUI)) {
                // Deselect the action
                // CombatManager.instance.SelectAction(null, null);
                CombatEvents.instance.TriggerOnDieInsert(null, null);
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

    private void FollowAndRotate() {
        // Make Die smoothly travel towards mouse
        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        transform.position = Vector3.Lerp(transform.position, point, travelRate);

        // Rotate Die
        transform.Rotate(0, 0, rotationDirection * spinRate); //rotates 50 degrees per second around z axis
    }

    /// This should always be at the end
    public void DrawValue(int value)
    {
        if (pipHolders == null)
        {
            throw new System.Exception("Dice render not set");
        }

        // Display certain pips depending on die value
        switch (value)
        {
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
}
