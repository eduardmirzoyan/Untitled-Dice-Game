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
    [SerializeField] private Tilemap selectionTilemap;
    [SerializeField] private Tilemap turnIndicatorTilemap;
    [SerializeField] private Tilemap targetIndicatorTilemap;
    [SerializeField] private Tile selectionTile;
    [SerializeField] private Tile turnTile;
    [SerializeField] private Tile targetTile;
    [SerializeField] private Vector3Int selectionPosition;
    [SerializeField] private Vector3Int turnPosition;

    [Header("Queue UI")]
    [SerializeField] private QueueUI queueUI;

    [Header("Actions UI")]
    [SerializeField] private LayoutGroup actionLayoutGroup;
    [SerializeField] public GameObject actionUIPrefab;
    [SerializeField] private List<ActionUI> actionUIs;

    [Header("Dice UI")]
    [SerializeField] private HorizontalLayoutGroup allyDiceGroup;
    [SerializeField] private HorizontalLayoutGroup enemyDiceGroup;
    [SerializeField] private GameObject diceOutlinePrefab;
    [SerializeField] private GameObject dicePrefab;

    [Header("State Indicator UI")]
    [SerializeField] private StateIndicatorUI stateIndicatorUI;
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


    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        // Default value for turn
        turnPosition = Vector3Int.back;
        actionUIs = new List<ActionUI>();

        // Initialize
        dieUIs = new List<DiceUI>();

        // Reset world selection
        ResetSelection();
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

            // Check if selected map has tile
            if (groundTilemap.HasTile(worldPos)) {
                // Reset previous selection if exists
                if (SelectionExists()) {
                    ResetSelection();
                }

                // Cache selected position
                selectionPosition = worldPos;
                selectionTilemap.SetTile(worldPos, selectionTile);
            }
        }

        // Right click to clear any selection
        if (Input.GetMouseButtonDown(1) && SelectionExists()) {
            ResetSelection();
        }
    }

    private bool SelectionExists() {
        return selectionPosition.z != -1;
    }

    private void ResetSelection() {
        if (SelectionExists()) {
            // Delete tile at selected location
            selectionTilemap.SetTile(selectionPosition, null);
        }
        // Reset position
        selectionPosition = new Vector3Int(0, 0, -1);
    }

    #endregion

    public void SetTurn(Vector3Int vector3Int) {
        // If something else was chosen as the turn, then deselect it
        if (turnPosition != Vector3Int.back) {
            turnIndicatorTilemap.SetTile(turnPosition, null);
        }
        turnPosition = vector3Int;
        // Set tile
        turnIndicatorTilemap.SetTile(vector3Int, turnTile);
    }

    public void SetUpRound(int roundNumber) {
        // Display visual feedback
        queueUI.SetRoundCounter("Round " + roundNumber);
    }

    public void SetUpTurnQueue(Queue<Combatant> queue) {
        // Debug print queue order
        string result = "";
        foreach (var combatant in queue.ToList()) {
            // Update UI
            queueUI.Enqueue(combatant.unit, Color.clear);

            result += combatant.unit.unitName + " -> ";
        }
        print(result);
    }

    public void SpawnDiceOutlines(List<Combatant> combatants)
    {
        //allyOutlineUI = new List<GameObject>();
        //enemyOutlineUI = new List<GameObject>();
        dieOutlineUIs = new List<GameObject>();

        GameObject dieOutline;
        Transform groupTransform;
        foreach (var combatant in combatants) {
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
        var dieOutlineUI = dieOutlineUIs[combatant.partyIndex];
        // Set color based on alligence
        Color dieColor = combatant.isAlly() ? Color.red : Color.blue;

        // Create the UI
        var dieUI = Instantiate(dicePrefab, dieOutlineUI.transform).GetComponent<DiceUI>();
        dieUI.transform.position = dieOutlineUI.transform.position;
        dieUI.Initialize(combatant.unit.dice, dieColor, dieOutlineUI.GetComponent<RectTransform>());

        // Store
        dieUIs.Add(dieUI);
    }

    public void SpawnModels(List<Combatant> combatants)
    {
        // Stores the center of the hex to spawn model
        Vector3 centeredPosition = Vector3.zero;
        Transform parent = null;

        // Loop through all combatants
        foreach (var combatant in combatants)
        {
            // Get center of world position
            centeredPosition = GetCellCenter(combatant.worldPosition);

            // Set parent based on allegiance
            parent = combatant.isAlly() ? allyModels : enemyModels;

            // Spawn unit's model prefab and store it's canvas
            var canvas = Instantiate(combatant.unit.modelPrefab, centeredPosition, Quaternion.identity, parent).GetComponentInChildren<Canvas>();

            // Spawn healthbar
            var healthbar = Instantiate(healthbarPrefab, canvas.transform).GetComponent<HealthbarUI>();

            // Assign healthbar
            combatant.AssignHealthbar(healthbar);

            // TODO Spawn animation?
        }
    }

    public void SpawnCombatants(List<Combatant> combatants) {
        // Stores the center of the hex to spawn model
        Vector3 centeredPosition = Vector3.zero;
        Transform parent = null;

        // Loop through all combatants
        foreach (var combatant in combatants) {
            // Get center of world position
            centeredPosition = GetCellCenter(combatant.worldPosition);

            // Set parent based on allegiance
            parent = combatant.isAllyAllegiance ? allyModels : enemyModels;

            // Spawn unit's model prefab and store it's canvas
            var canvas = Instantiate(combatant.unit.modelPrefab, centeredPosition, Quaternion.identity, parent).GetComponentInChildren<Canvas>();
        
            // Spawn healthbar
            var healthbar = Instantiate(healthbarPrefab, canvas.transform).GetComponent<HealthbarUI>();

            // Assign healthbar
            combatant.AssignHealthbar(healthbar);

            // TODO Spawn animation?
        }
    }

    public void RollDie(int index) {
        if (dieUIs[index] != null) {
            dieUIs[index].Roll();
        }
    }

    public void EnableAllyDice() {
        // Set the first 4 die active
        for (int i = 0; i < 4; i++) {
            // Enable moving the die
            dieUIs[i].SetInteractive(true);
        }
    }


    /// Clears all dice and outline UI
    public void DespawnDice() {
        // Destroy all the dice and their outlines
        for (int i = 0; i < dieUIs.Count; i++) {
            if (dieUIs[i] != null) {
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

    public void DisplayUnitActions(Unit unit) {
        // Clear any existing actions if exists
        ClearUnitActions();

        // Get all weapons equipped to the unit
        foreach (var action in unit.GetActions()) {
            // Create UI gameobject
                var actionUI = Instantiate(actionUIPrefab, actionLayoutGroup.transform).GetComponent<ActionUI>();
                // Initalize it
                actionUI.Initialize(action);
                // Add to list
                actionUIs.Add(actionUI);
        }
    }

    public void ClearUnitActions() {
        // If there is no UI displaying, do nothing
        if (actionUIs.Count == 0) 
            return;
        
        foreach (var actionUI in actionUIs) {
            // Destroy gameobject
            Destroy(actionUI.gameObject);
        }
        actionUIs.Clear();
    }

    public void PopFromTurnQueue() {
        queueUI.Dequeue();
    }

    public Vector3 GetCellCenter(Vector3Int position) {
        return worldGrid.GetCellCenterWorld(position);
    }

    public void HighlightTarget(Vector3Int position) {
        // Turn hex color to red
        if (targetIndicatorTilemap.HasTile(position)) {
            print("This location is already targeted.");
        }

        targetIndicatorTilemap.SetTile(position, targetTile);
    }

    public void ClearTargets() {
        // Turn hex color to red
        foreach (var position in targetIndicatorTilemap.cellBounds.allPositionsWithin) {
            targetIndicatorTilemap.SetTile(position, null);
        }
    }

    public void DeSelectOtherActions(Action action) {
        
        // Unselect all other actions
        for (int i = 0; i < actionUIs.Count; i++) {
            var actionUI = actionUIs[i];
            
            // Attempt to un-select every action
            if (actionUI.GetAction() != action && actionUI.ContainsDie()) {
                actionUI.DeSelect();
            }
        }
    }

    public void PerformAction(Action action) {
        foreach (var actionUI in actionUIs) {
            if (actionUI.ContainsDie()) {
                if (actionUI.GetAction() == action) {
                    actionUI.DeactivateDie();
                }
                actionUI.DeSelect();
            }
        }
    }

    public void SpawnFloatingNumber(string damageText, Vector3 position) {
        var floatingNum = Instantiate(floatingNumberPrefab, position, Quaternion.identity).GetComponent<FloatingNumberUI>();
        floatingNum.Initialize(damageText);
    }

    public void EnterState(string text) {
        StartCoroutine(stateIndicatorUI.EnterState(text));
    }

    public void ExitState() {
        StartCoroutine(stateIndicatorUI.ExitState());
    }
}
