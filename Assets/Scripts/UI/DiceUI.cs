using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [Header("Visuals")]
    [SerializeField] private Image background;
    [SerializeField] private Image selectionOutline;

    [Header("Components")]
    [SerializeField] private Dice dice;
    [SerializeField] private Animator animator;
    [SerializeField] private Image[] pipHolders;
    [SerializeField] private Combatant combatant;

    [SerializeField] private RectTransform origin;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform currentParent;

    [SerializeField] private float travelRate = 0.1f;
    [SerializeField] private float spinRate = 3f;
    [SerializeField] private bool isInteractable;

    private Coroutine rollRoutine;
    private bool isBeingDragged;
    private int rotationDirection = 1;
    private Transform playerScreen;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(Combatant combatant, Color color, RectTransform origin, bool interactable = false)
    {
        this.dice = combatant.unit.dice;
        this.combatant = combatant;
        this.origin = origin;
        this.isInteractable = interactable;
        background.color = color;
        playerScreen = transform.root;

        // UI events
        CombatEvents.instance.onPlayerTurnStart += OnPlayerTurnStart;
        CombatEvents.instance.onDieStartDrag += OnStartDrag;
        CombatEvents.instance.onDieEndDrag += OnEndDrag;
        CombatEvents.instance.onPlayerTurnEnd += OnPlayerTurnEnd;

        // Subscribe to events
        CombatEvents.instance.onRoll += OnRoll;
        CombatEvents.instance.onReroll += OnRoll;
        CombatEvents.instance.onSet += OnSet;
        CombatEvents.instance.onGrow += OnGrow;
        CombatEvents.instance.onShrink += OnShrink;
        CombatEvents.instance.onReplenish += OnReplenish;
        CombatEvents.instance.onExhaust += OnExhaust;

        // Display the current state of the die
        UpdateVisuals();
    }

    public void Uninitialize() {
        // Unsub to all events
        CombatEvents.instance.onPlayerTurnStart -= OnPlayerTurnStart;
        CombatEvents.instance.onDieStartDrag -= OnStartDrag;
        CombatEvents.instance.onDieEndDrag -= OnEndDrag;
        CombatEvents.instance.onPlayerTurnEnd -= OnPlayerTurnEnd;

        CombatEvents.instance.onRoll -= OnRoll;
        CombatEvents.instance.onReroll -= OnRoll;
        CombatEvents.instance.onSet -= OnSet;
        CombatEvents.instance.onGrow -= OnGrow;
        CombatEvents.instance.onShrink -= OnShrink;
        CombatEvents.instance.onReplenish -= OnReplenish;
        CombatEvents.instance.onExhaust -= OnExhaust;
    }

    private void OnPlayerTurnStart(int value) {
        // Check if this is an ally die
        if (combatant.isAlly()) {
            // Make it interactive
            isInteractable = true;

            CheckHighlight();
        }
    }

    private void OnPlayerTurnEnd(int valid) {
        // Check if this is an ally die
        if (combatant.isAlly()) {
            // Make it NOT interactive
            isInteractable = false;

            // Remove any highlight
            animator.Play("Idle");
        }
    }

    private void CheckHighlight() {
        // Check if it's usable
        if (combatant.isAlly() && !dice.isExhausted) {
            // Show outline animation
            selectionOutline.color = Color.white;
            animator.Play("Highlight");
        }
        else {
            // Else play regular animation
            animator.Play("Idle");
        }
    }

    private void OnStartDrag(Dice dice) {
        if (combatant.isAlly()) {
            // Die should become idle
            animator.Play("Idle");
        }   
    }

    private void OnEndDrag(Dice dice) {
        // If this is an ally die
        if (!this.dice.isExhausted && combatant.isAlly()) {
            // If a die was selected
            if (CombatManager.instance.selectedDie != null) {
                // If this was the selected die
                if (CombatManager.instance.selectedDie == this.dice) {
                    selectionOutline.color = Color.white;
                    animator.Play("Select");
                }
                else {
                    animator.Play("Idle");
                }
            }
            else {
                selectionOutline.color = Color.white;
                animator.Play("Highlight");
            }
        }
        
    }

    private void OnRoll(Dice dice) {
        // If this die was rolled
        if (this.dice == dice) {
            Roll();
        }
    }

    private void OnSet(Dice dice) {
        // If this die was set
        if (this.dice == dice) {
            // Update value
            DrawValue(dice.GetValue());

            // Show visuals
            selectionOutline.color = Color.yellow;
            animator.Play("Select");
        }
    }

    private void OnGrow(Dice dice) {
        // If this die was rolled
        if (this.dice == dice) {
            // Update visuals
            DrawValue(dice.GetValue());

            // Do green animation
            selectionOutline.color = Color.green;
            animator.Play("Select"); // MAKE THIS GREEN
        }
    }

    private void OnShrink(Dice dice) {
        // If this die was rolled
        if (this.dice == dice) {
            // Update visuals
            DrawValue(dice.GetValue());

            // Do green animation
            selectionOutline.color = Color.red;
            animator.Play("Select"); // MAKE THIS GREEN
        }
    }

    public void OnReplenish(Dice dice) {
        // If this die was the target
        if (this.dice == dice) {
            canvasGroup.alpha = 1f;
        }
    }

    public void OnExhaust(Dice dice) {
        // If this die was the target
        if (this.dice == dice) {
            canvasGroup.alpha = 0.5f;
        }
    }

    public void SetInteractive(bool state) {
        isInteractable = state;
    }

    public void Roll() {
        if (rollRoutine != null) {
            StopCoroutine(rollRoutine);
        }
        rollRoutine = StartCoroutine(RollVisuals(CombatManager.instance.rollTime));
    }

    private IEnumerator RollVisuals(float duration)
    {
        float elapsedTime = 0;

        // Make animation fast if in mode
        if (GameManager.instance.fastMode) duration = 0.1f;

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
        selectionOutline.color = Color.white;
        animator.Play("Select");
    }

    private void UpdateVisuals() {
        // Draw the physical die
        DrawValue(dice.GetValue());

        // Update transparancy depending if die is active
        canvasGroup.alpha = dice.isExhausted ? 0.5f : 1f;
    }

    public Dice GetDie() {
        return dice;
    }

    private void FixedUpdate() {
        if (isBeingDragged) {
            FollowAndRotate();
        }
    }

    // Dragging helper functions
    public void OnBeginDrag(PointerEventData eventData) {
        // If interactable and die is active
        if (!dice.isExhausted && isInteractable) {

            // Update visuals
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            // Save parent
            currentParent = rectTransform.parent;

            // Remove parent
            rectTransform.SetParent(playerScreen);

            // Enable flag
            isBeingDragged = true;

            // Randomize rotation direction
            rotationDirection = Random.Range(0, 2) == 0 ? 1 : -1;

            // Trigger event
            CombatEvents.instance.TriggerOnDieStartDrag(dice);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        // Does nothing so far
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!dice.isExhausted && isInteractable) {
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

            // Update UI
            UpdateVisuals();
            
            // Trigger event
            CombatEvents.instance.TriggerOnDieEndDrag(dice);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        // Return to origin on right click
        if (eventData.button == PointerEventData.InputButton.Right) {
            // If it is currently in a dice slot, attempt to remove itself
            if (transform.parent.TryGetComponent(out SkillDisplaySlotUI actionHolderUI)) {
                // Trigger Event
                CombatEvents.instance.TriggerOnActionSelect(null, null);
                // Trigger Event
                CombatEvents.instance.TriggerOnDieEndDrag(dice);
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
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 2:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = false;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = false;
                pipHolders[4].enabled = false;
                pipHolders[5].enabled = false;
                pipHolders[6].enabled = false;
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 3:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = true;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = false;
                pipHolders[4].enabled = false;
                pipHolders[5].enabled = false;
                pipHolders[6].enabled = false;
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 4:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = false;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = true;
                pipHolders[4].enabled = true;
                pipHolders[5].enabled = false;
                pipHolders[6].enabled = false;
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 5:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = true;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = true;
                pipHolders[4].enabled = true;
                pipHolders[5].enabled = false;
                pipHolders[6].enabled = false;
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 6:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = false;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = true;
                pipHolders[4].enabled = true;
                pipHolders[5].enabled = true;
                pipHolders[6].enabled = true;
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 7:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = true;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = true;
                pipHolders[4].enabled = true;
                pipHolders[5].enabled = true;
                pipHolders[6].enabled = true;
                pipHolders[7].enabled = false;
                pipHolders[8].enabled = false;
                break;

            case 8:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = false;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = true;
                pipHolders[4].enabled = true;
                pipHolders[5].enabled = true;
                pipHolders[6].enabled = true;
                pipHolders[7].enabled = true;
                pipHolders[8].enabled = true;
                break;

            case 9:
                pipHolders[0].enabled = true;
                pipHolders[1].enabled = true;
                pipHolders[2].enabled = true;
                pipHolders[3].enabled = true;
                pipHolders[4].enabled = true;
                pipHolders[5].enabled = true;
                pipHolders[6].enabled = true;
                pipHolders[7].enabled = true;
                pipHolders[8].enabled = true;
                break;

            default:
                throw new System.Exception("Dice value is not displayable: " + dice.GetValue());
        }
    }
}
