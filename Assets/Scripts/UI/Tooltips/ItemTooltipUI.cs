using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI instance;

    [Header("Displaying Components")]
    [SerializeField] private RectTransform windowTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image lockImage;
    [SerializeField] private GameObject itemSkillPrefab;
    [SerializeField] private LayoutGroup actionsLayoutGroup;

    [Header("Adjustments")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isLocked;

    private List<ItemSkillUI> itemSkillUIs;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        canvasGroup = GetComponentInChildren<CanvasGroup>();

        lockImage.enabled = false;
        itemSkillUIs = new List<ItemSkillUI>();
    }

    private void Update() {
        if (isLocked && Input.GetMouseButtonDown(1)) {
            // Make un-interactable
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            isLocked = false;
            lockImage.enabled = false;
            Hide();
        }
    }

    public void Lock() {
        // Make sure the display is active
        if (isVisible) {
            // Make it interactable
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            // Make sure it doesn't go away when you hover out
            isLocked = true;
            lockImage.enabled = true;
        }
    }

    public void Show(Item item, Vector3 position) {
        // Don't reshow if locked
        if (isLocked) return;

        // Display window
        canvasGroup.alpha = 1f;

        // Update information
        itemName.text = item.name;
        flavorText.text = item.description;
        itemDescription.text = "";

        // If the item is an equipment item
        if (item is Equipment) {
            // Cast
            var equipment = (Equipment) item;

            // Show text
            if (equipment is Weapon) itemDescription.text = "Base Damage: " + ((Weapon) equipment).baseDamage;
            else itemDescription.text = "";

            // Display all its skills
            foreach (var skill in equipment.skills) {
                // Spawn visuals of actions
                var itemSkillUI = Instantiate(itemSkillPrefab, actionsLayoutGroup.transform).GetComponent<ItemSkillUI>();
                itemSkillUI.Initialize(skill);
                itemSkillUIs.Add(itemSkillUI);
            }
        }

        // Update Canvas
        LayoutRebuilder.ForceRebuildLayoutImmediate(actionsLayoutGroup.GetComponent<RectTransform>());

        // Update Canvas
        LayoutRebuilder.ForceRebuildLayoutImmediate(windowTransform);

        // Check if offscreen

        // Get mouse position
        var pos = Input.mousePosition;
        var width = windowTransform.rect.width;
        var height = windowTransform.rect.height;
        var curOffset = offset;

        // Check if window goes off-screen on x-axis
        // If so, flip horizontally
        if (pos.x + width > Screen.width) {
            width = -width;
            curOffset.x = -curOffset.x;
        }
            

        // Check if window goes off-screen on y-axis
        // If so, flip vertically
        if (pos.y + height > Screen.height) {
            height = -height;
            curOffset.y = -curOffset.y;
        }
            

        // Move transform
        transform.position = position + curOffset;

        // Translate tooltip so that window's bottom left corner is at bottom right
        windowTransform.anchoredPosition = new Vector2(width / 2, height / 2);
        
        // Debug
        // print("Item tooltip:" + transform.position);
        // print("Window size: " + windowTransform.rect);
        // print("Mouse: " + Input.mousePosition);

        isVisible = true;
    }

    public void Hide() {
        // Don't hide if it's locked
        if (isLocked) return;

        // Hide any skill tooltips if possible
        SkillTooltipUI.instance.Hide();

        // Destroy all the ui
        foreach (var ui in itemSkillUIs) {
            Destroy(ui.gameObject);
        }
        itemSkillUIs.Clear();

        // Then disable window
        canvasGroup.alpha = 0f;

        isVisible = false;
    }
    
}
