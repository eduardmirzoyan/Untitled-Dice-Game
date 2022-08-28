using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartyMemberUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Transforms")]
    [SerializeField] private RectTransform windowTransform;
    [SerializeField] private RectTransform HUDTransform;

    [Header("Displaying Components")]
    [SerializeField] private Text displayName;
    [SerializeField] private Image icon;
    [SerializeField] private Slider healthBar;
    [SerializeField] private DiceUI diceUI;
    [SerializeField] private Text speedStat;
    [SerializeField] private RectTransform weapon1Transform;
    [SerializeField] private RectTransform weapon2Transform;
    
    [Header("Temporary")]
    [SerializeField] private GameObject itemPrefab;
    
    // Unit name
    [SerializeField] private Unit unit;
    [SerializeField] private float UIchangeTime = 1f;
    private Coroutine transitionRoutine;
    private bool isRasied = false;
    // Update display name

        // Update icon

        // Update healthbar

        // Update die

        // Update speed stat

        // Update equipped weapons

        // Update equipped armors

    public void Initialize(Unit unit) {
        this.unit = unit;
        UpdateVisuals();
    }

    private void Update() {
        // Debugging
        if (Input.GetKeyDown(KeyCode.J)) {
            Initialize(unit);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(HUDTransform, Input.mousePosition, Camera.main)) {
            // Stop any coroutines first
            if (transitionRoutine != null) StopCoroutine(transitionRoutine);

            // Raise or lower based on current state
            if (isRasied) {
                transitionRoutine = StartCoroutine(LowerUI());
            }
            else {
                transitionRoutine = StartCoroutine(RaiseUI());
            }
        }
    }

    private IEnumerator RaiseUI() {
        isRasied = true;

        float timer = UIchangeTime;
        while (timer > 0) {
            // Raise UI over time
            var newY = Mathf.Lerp(windowTransform.anchoredPosition.y, 350, 1 - timer / UIchangeTime);
            windowTransform.anchoredPosition = new Vector2(windowTransform.anchoredPosition.x, newY);

            // Decrement time
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LowerUI() {
        isRasied = false;

        float timer = UIchangeTime;
        while (timer > 0) {
            // Raise UI over time
            var newY = Mathf.Lerp(windowTransform.anchoredPosition.y, -350, 1 - timer / UIchangeTime);
            windowTransform.anchoredPosition = new Vector2(windowTransform.anchoredPosition.x, newY);

            // Decrement time
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public void UpdateVisuals() {
        // Make sure a unit exists
        if (unit == null) {
            throw new System.Exception("No unit in this slot. " + gameObject.name);
        }

        // Update display name
        displayName.text = unit.name;

        // Update icon
        icon.sprite = unit.icon;
        icon.color = Color.white;

        // Update healthbar
        healthBar.maxValue = unit.maxHealth;
        healthBar.value = unit.currentHealth;

        // Update die
        diceUI.DisplayValue(unit.dice.maxValue);

        // Update speed stat
        speedStat.text = unit.speed.ToString();

        // Update equipped weapons
        if (unit.weapons[0] != null) {
            var itemUI = Instantiate(itemPrefab, weapon1Transform).GetComponent<ItemUI>();
            itemUI.Initialize(unit.weapons[0]);
        }
        if (unit.weapons[1] != null) {
            var itemUI = Instantiate(itemPrefab, weapon2Transform).GetComponent<ItemUI>();
            itemUI.Initialize(unit.weapons[1]);
        }

        // Update equipped armors
    }


}
