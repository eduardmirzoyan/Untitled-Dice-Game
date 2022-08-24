using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
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
    private Canvas playerScreen;

    [SerializeField] private float travelRate = 0.1f;
    private bool isBeingDragged;

    [SerializeField] private float spinRate = 3f;

    [SerializeField] private Transform child;
    [SerializeField] private float speed;

    [SerializeField] private bool isActive;

    private Coroutine rollRoutine;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        playerScreen = GameObject.Find("Player Screen").GetComponent<Canvas>();
    }
    
    private void Update() {
        DrawDice();

        if (Input.GetKeyDown(KeyCode.F)) {
            Roll();
        }
    }

    public void Initialize(Dice dice, Color color, RectTransform origin, bool isActive = false) {
        this.dice = dice;
        background.color = color;
        // Set origin to itself
        this.origin = origin;
        this.isActive = isActive;
    }

    public void SetActive(bool active) {
        isActive = active;
    }

    public void Roll() {
        if (rollRoutine != null) {
            StopCoroutine(rollRoutine);
        }
        rollRoutine = StartCoroutine(RollDice(rollDuration));
    }

    private void DrawDice() {
        ValueToUI();
    }

    public Dice GetDice() {
        return dice;
    }

    private IEnumerator RollDice(float duration) {

        float elapsedTime = 0;
        // Smoothly move to target
        while (elapsedTime < duration)
        {
            // Lerp target item to its spot
            dice.Roll();

            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        animator.Play("Selected");
    }

    private void ValueToUI() {
        if (pipHolders == null) {
            throw new System.Exception("Dice render not set");
        }

        // Display certain pips depending on die value
        switch (dice.GetValue()) {
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
        if (isActive) {
            // Update visuals
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            // Save parent
            currentParent = rectTransform.parent;

            // Remove parent
            rectTransform.SetParent(playerScreen.transform);

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
        if (isActive) {
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

    private void Oscillate() {
        // TODO

        child.localPosition -= new Vector3(0, -speed * Time.deltaTime, 0);
        transform.Rotate(0, 0, spinRate);

        if (child.localPosition.y >= 210) {
            speed = -speed;
            child.localPosition = Vector3.up * 200;
        }
        if (child.localPosition.y <= -210) {
            speed = -speed;
            child.localPosition = Vector3.up * -200;
        }
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
