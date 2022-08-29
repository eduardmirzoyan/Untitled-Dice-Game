using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayUI : MonoBehaviour
{
    public static ItemDisplayUI instance;

    [Header("Displaying Components")]
    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image lockImage;

    [Header("Adjustments")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isActive;
    [SerializeField] private bool isLocked;

    private List<ActionUI> actionUIs;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        lockImage.enabled = false;
        actionUIs = new List<ActionUI>();
    }

    private void Update() {
        if (isLocked && Input.GetMouseButtonDown(0)) {
            isLocked = false;
            lockImage.enabled = false;
            Hide();
        }
    }

    public void Lock() {
        // Make sure the display is active
        if (isActive) {
            // Make sure it doesn't go away when you hover out
            isLocked = true;
            lockImage.enabled = true;
        }
    }

    public void Show(Item item, Vector3 position) {
        // Don't reshow if locked
        if (isLocked) return;

        // Move transform
        transform.position = position + offset;

        // Display window
        window.SetActive(true);

        // Update information
        itemName.text = item.itemName;
        flavorText.text = "Generic flavor text here";
        itemDescription.text = item.itemDescription;

        // If item is a weapon then display it's actions
        if (item is Weapon) {
            var weapon = (Weapon) item;
            itemDescription.text = "Base damage: " + weapon.baseDamage;

            // Display all the actions
            foreach (var action in weapon.actions) {
                var ui = Instantiate(CombatManagerUI.instance.actionUIPrefab, window.transform).GetComponent<ActionUI>();
                ui.JustDisplay(action);
                actionUIs.Add(ui);
            }
        }

        isActive = true;
    }

    public void Hide() {
        // Don't hide if it's locked
        if (isLocked) return;

        // Destroy all the ui
        foreach (var ui in actionUIs) {
            Destroy(ui.gameObject);
        }
        actionUIs.Clear();

        // Then disable window
        window.SetActive(false);

        isActive = false;
    }
    
}
