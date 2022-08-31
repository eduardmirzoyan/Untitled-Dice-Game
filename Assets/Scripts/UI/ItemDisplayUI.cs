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
    [SerializeField] private GameObject actionHolderPrefab;
    [SerializeField] private LayoutGroup actionsLayoutGroup;

    [Header("Adjustments")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isVisible;
    [SerializeField] private bool isLocked;

    private List<ActionHolderUI> actionHolderUIs;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        lockImage.enabled = false;
        actionHolderUIs = new List<ActionHolderUI>();
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
        if (isVisible) {
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
        flavorText.text = item.itemDescription;
        itemDescription.text = "";

        // If item is a weapon then display it's actions
        if (item is Weapon) {
            var weapon = (Weapon) item;
            itemDescription.text = "Base Damage: " + weapon.baseDamage;

            // Display all the actions
            foreach (var action in weapon.actions) {
                // Spawn visuals of actions
                var actionHolderUI = Instantiate(actionHolderPrefab, actionsLayoutGroup.transform).GetComponent<ActionHolderUI>();
                actionHolderUI.Initialize(action, false);
                actionHolderUIs.Add(actionHolderUI);
            }
        }

        isVisible = true;
    }

    public void Hide() {
        // Don't hide if it's locked
        if (isLocked) return;

        // Destroy all the ui
        foreach (var ui in actionHolderUIs) {
            Destroy(ui.gameObject);
        }
        actionHolderUIs.Clear();

        // Then disable window
        window.SetActive(false);

        isVisible = false;
    }
    
}
