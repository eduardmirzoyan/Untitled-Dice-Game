using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;

public class CombatManagerUI : MonoBehaviour
{
    public static CombatManagerUI instance;

    [SerializeField] private Grid worldGrid;
    [SerializeField] private Tilemap groundTilemap;

    [Header("Actions UI")]
    [SerializeField] private LayoutGroup actionLayoutGroup;
    [SerializeField] private GameObject actionHolderPrefab;
    [SerializeField] private List<SkillDisplaySlotUI> actionHolders;

    [Header("Dice UI")]
    [SerializeField] private HorizontalLayoutGroup allyDiceGroup;
    [SerializeField] private HorizontalLayoutGroup enemyDiceGroup;
    [SerializeField] private GameObject diceOutlinePrefab;
    [SerializeField] private GameObject dicePrefab;

    [Header("State Indicator UI")]
    [SerializeField] public List<DiceUI> dieUIs;
    [SerializeField] private List<GameObject> dieOutlineUIs;

    [Header("Healthbar UI")]
    [SerializeField] private GameObject healthbarPrefab;

    [Header("Ally Models Transform")]
    [SerializeField] private Transform allyModels;

    [Header("Enemy Models Transform")]
    [SerializeField] private Transform enemyModels;

    [Header("Damage number UI")]
    [SerializeField] private GameObject floatingNumberPrefab;

    [SerializeField] private GameObject combatStationPrefab;

    [SerializeField] private Color allyColor;
    [SerializeField] private Color enemyColor;

    private Canvas playerScreen;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        // Initialize
        dieUIs = new List<DiceUI>();

        playerScreen = GameObject.Find("Player Screen").GetComponent<Canvas>();
    }

    private void Start() {
        CombatEvents.instance.onRoundStart += SpawnDiceOutlines;
        
        CombatEvents.instance.onPlayerTurnStart += ShowActions;
        CombatEvents.instance.onActionConfirm += ClearActions;

        CombatEvents.instance.onRoundEnd += DespawnDice;


        CombatEvents.instance.onTakeDamage += SpawnDamageNumber;
        CombatEvents.instance.onHeal += SpawnHealNumber;

        CombatEvents.instance.onSpawnCombatant += SpawnCombatant;
    }

    # region Selection Logic
    private void Update() {
        // Left click to make selection
        if (Input.GetMouseButtonDown(0)) {
            // Get world position from camera
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0; // Because camera is -10 from the world
            Vector3Int worldPos = groundTilemap.WorldToCell(pos);

            // For Debugging
            // print(worldPos);

            // Check to see if this is a selection or target
            if (CombatManager.instance.hasActionBeenChoosen()) {
                // Check if there was something to target at that location
                var targetCombatant = CombatManager.instance.GetCombatantAtPosition(worldPos);
                if (targetCombatant != null) {
                    // Attempt to select target
                    CombatManager.instance.SelectTarget(targetCombatant);
                }
                
            }
        }

        // Deselect on right click
        else if (Input.GetMouseButtonDown(1)) {
            // if (CombatManager.instance.hasActionBeenChoosen()) {
            //     Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //     pos.z = 0; // Because camera is -10 from the world
            //     Vector3Int worldPos = groundTilemap.WorldToCell(pos);

            //     // Check if there was something to target at that location
            //     var targetCombatant = CombatManager.instance.GetCombatantAtPosition(worldPos);
            //     if (targetCombatant != null && CombatManager.instance.selectedTarget == targetCombatant) {
            //         // Attempt to de-select target
            //         CombatManager.instance.SelectTarget(null);
            //     }
                
            // }
        }
    }


    #endregion

    private void SpawnDiceOutlines(int value) {
        // Make new list
        dieOutlineUIs = new List<GameObject>();

        // Cache
        GameObject dieOutline;
        Transform groupTransform;

        // Loop through all combatants
        foreach (var combatant in CombatManager.instance.combatants) {
            if (combatant != null) {
                // Spawn outline in ally zone or enemy zone depending on combatant
                groupTransform = combatant.isAlly() ? allyDiceGroup.transform : enemyDiceGroup.transform;
                dieOutline = Instantiate(diceOutlinePrefab, groupTransform);
                dieOutlineUIs.Add(dieOutline);
            }
        }
    }

    public void SpawnDie(Combatant combatant) {
        // Get corresponding outline based on index
        var dieOutlineUI = dieOutlineUIs[combatant.index];
        // Set color based on alligence
        Color dieColor = combatant.isAlly() ? allyColor : enemyColor;

        // Create the UI
        var dieUI = Instantiate(dicePrefab, dieOutlineUI.transform).GetComponent<DiceUI>();
        dieUI.transform.position = dieOutlineUI.transform.position;
        dieUI.Initialize(combatant, dieColor, dieOutlineUI.GetComponent<RectTransform>());

        // Store
        dieUIs.Add(dieUI);
    }

    private void SpawnCombatant(Combatant combatant, float animationTime) {
        Vector3 centeredPosition = Vector3.zero;
        Transform parent = null;

        // Get center of world position
        centeredPosition = GetCellCenter(combatant.hexPosition);

        // Set parent based on allegiance
        parent = combatant.isAlly() ? allyModels : enemyModels;

        // Spawn unit's model prefab and store it's canvas
        var modelObject = Instantiate(combatant.unit.modelPrefab, centeredPosition, Quaternion.identity, parent);
        var canvas = modelObject.GetComponentInChildren<Canvas>();

        // Spawn healthbar and initialize
        var healthbar = Instantiate(healthbarPrefab, canvas.transform).GetComponent<HealthbarUI>();
        healthbar.Initialize(combatant);

        // Initialize model
        modelObject.GetComponent<ModelUI>().Initialize(combatant);

        // Assign model
        combatant.AssignModel(modelObject.transform);

        // Spawn station and initialize
        var combatStation = Instantiate(combatStationPrefab, centeredPosition, Quaternion.identity).GetComponent<CombatStationUI>();
        combatStation.Initialize(combatant);

        // Spawn animation
        StartCoroutine(Move(combatant, animationTime));
    }

    private IEnumerator Move(Combatant combatant, float animationTime) {
        float offset = combatant.isAlly() ? -5 : 5;
        float start = combatant.worldPosition.x + offset;
        float end = combatant.worldPosition.x;

        // Move to offset
        combatant.modelTransform.position = new Vector2(start, combatant.modelTransform.position.y);

        // Make animation fast if in mode
        if (GameManager.instance.fastMode) animationTime = 0.1f;

        float timer = 0;
        while (timer < animationTime) {
            // Move unit
            var newX = Mathf.Lerp(start, end, timer / animationTime);
            combatant.modelTransform.position = new Vector2(newX, combatant.modelTransform.position.y);

            // Increment time
            timer += Time.deltaTime;
            yield return null;
        }

        // Set final point
        combatant.modelTransform.position = new Vector2(end, combatant.modelTransform.position.y);

        yield return null;
    }

    private void DespawnDice(int value) {
        // Destroy all the dice and their outlines
        for (int i = 0; i < dieUIs.Count; i++) {
            if (dieUIs[i] != null) {
                // Unsub
                dieUIs[i].Uninitialize();
                // Destroy Dice UI
                Destroy(dieUIs[i].gameObject);
            }
            // Destroy Ouline
            Destroy(dieOutlineUIs[i].gameObject);
        }

        // Clear lists
        dieOutlineUIs.Clear();
        dieUIs.Clear();
    }

    private void ShowActions(int value) {
        // Make new list
        actionHolders = new List<SkillDisplaySlotUI>();
        // Get all weapons equipped to the unit
        foreach (var action in CombatManager.instance.currentCombatant.unit.GetActions()) {
            // Update cooldown
            action.ReduceCooldown();
            // Create UI gameobject
            var actionHolder = Instantiate(actionHolderPrefab, actionLayoutGroup.transform).GetComponent<SkillDisplaySlotUI>();
            // Initalize it
            actionHolder.Initialize(action);
            // Add to list
            actionHolders.Add(actionHolder);
        }
    }

    private void ClearActions(Action action) {
        // If there is no UI displaying, do nothing
        if (actionHolders.Count == 0) 
            return;
        
        // Delete each holder
        foreach (var actionHolder in actionHolders) {
            // Unsub
            actionHolder.Deinitialize();
            // Destroy gameobject
            Destroy(actionHolder.gameObject);
        }

        // Clear list
        actionHolders.Clear();
    }

    public Vector3 GetCellCenter(Vector3Int position) {
        return worldGrid.GetCellCenterWorld(position);
    }

    private void SpawnDamageNumber(Combatant combatant, int amount) {
        var floatingNum = Instantiate(floatingNumberPrefab, combatant.worldPosition, Quaternion.identity).GetComponent<FloatingNumber>();
        floatingNum.Initialize(amount.ToString(), Color.white);
    }

    private void SpawnHealNumber(Combatant combatant, int amount) {
        var floatingNum = Instantiate(floatingNumberPrefab, combatant.worldPosition, Quaternion.identity).GetComponent<FloatingNumber>();
        floatingNum.Initialize(amount.ToString(), Color.green);
    }
}
