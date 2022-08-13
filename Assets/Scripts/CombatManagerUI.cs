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
    [SerializeField] private HorizontalLayoutGroup actionLayoutGroup;
    [SerializeField] private GameObject actionUIPrefab;
    [SerializeField] private List<ActionUI> actionUIs;

    [Header("Dice UI")]
    [SerializeField] private HorizontalLayoutGroup allyDiceGroup;
    [SerializeField] private HorizontalLayoutGroup enemyDiceGroup;
    [SerializeField] private GameObject diceOutlinePrefab;
    [SerializeField] private GameObject dicePrefab;

    [Header("State Indicator UI")]
    [SerializeField] private StateIndicatorUI stateIndicatorUI;

    [SerializeField] private List<DiceUI> allyDiceUI;
    [SerializeField] private List<DiceUI> enemyDiceUI;

    [SerializeField] private List<GameObject> allyOutlineUI;
    [SerializeField] private List<GameObject> enemyOutlineUI;

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
    }

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

    public void GenerateOutlineUI(int numAllyDice, int numEnemyDice) {
        allyOutlineUI = new List<GameObject>();
        for (int i = 0; i < numAllyDice; i++) {
            var dieOutline = Instantiate(diceOutlinePrefab, allyDiceGroup.transform);
            allyOutlineUI.Add(dieOutline);
        }

        enemyOutlineUI = new List<GameObject>();
        for (int i = 0; i < numEnemyDice; i++) {
            var dieOutline = Instantiate(diceOutlinePrefab, enemyDiceGroup.transform);
            enemyOutlineUI.Add(dieOutline);
        }
    }

    public void SpawnNewDice(Dice dice, bool isAlly, int index) {
        if (isAlly) {
            // Get corresponding outline
            var dieOutline = allyOutlineUI[index];

            // Create the UI
            var dieUI = Instantiate(dicePrefab, dieOutline.transform).GetComponent<DiceUI>();
            dieUI.transform.position = dieOutline.transform.position;
            dieUI.Initialize(dice, Color.red, dieOutline.GetComponent<RectTransform>());

            // Store
            allyDiceUI.Add(dieUI);
        }
        else {
            // Get corresponding outline
            var dieOutline = enemyOutlineUI[index];

            // Create the UI
            var dieUI = Instantiate(dicePrefab, dieOutline.transform).GetComponent<DiceUI>();
            dieUI.transform.position = dieOutline.transform.position;
            dieUI.Initialize(dice, Color.blue, dieOutline.GetComponent<RectTransform>());

            // Store
            enemyDiceUI.Add(dieUI);
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
            var canvas = Instantiate(combatant.unit.prefab, centeredPosition, Quaternion.identity, parent).GetComponentInChildren<Canvas>();
        
            // Spawn healthbar
            var healthbar = Instantiate(healthbarPrefab, canvas.transform).GetComponent<HealthbarUI>();

            // Assign healthbar
            combatant.AssignHealthbar(healthbar);

            // TODO Spawn animation?
        }
    }

    public void RollDice(bool isAlly, int index) {
        // Check if dice is ally
        if (isAlly) {
            // Check if index is in bounds
            if (index >= 0 && index < allyDiceUI.Count) {
                // Roll dice
                allyDiceUI[index].Roll();
            }
        }
        else {
            // Check if index is in bounds
            if (index >= 0 && index < enemyDiceUI.Count) {
                // Roll dice
                enemyDiceUI[index].Roll();
            }
        }
    }

    public void EnableAllyDice() {
        foreach (var diceUI in allyDiceUI) {
            // Enable moving the die
            diceUI.SetActive(true);
        }
    }


    /// Clears all dice and outline UI
    public void ClearAllDiceUI() {
        // Clear ally UI
        for (int i = 0; i < allyDiceUI.Count; i++)
        {
            if (allyDiceUI[i] != null) {
                // Destroy Dice UI
                Destroy(allyDiceUI[i].gameObject);     
            }
            // Destroy Ouline
            Destroy(allyOutlineUI[i].gameObject);
        }
        allyDiceUI.Clear();
        allyOutlineUI.Clear();

        // Clear ally UI
        for (int i = 0; i < enemyDiceUI.Count; i++)
        {
            // Destroy Dice UI
            Destroy(enemyDiceUI[i].gameObject);
            // Destroy Ouline
            Destroy(enemyOutlineUI[i].gameObject);
        }
        enemyDiceUI.Clear();
        enemyOutlineUI.Clear();
    }

    public void DeleteDiceUI(bool isAlly, int index) {
        // Check if dice is ally
        if (isAlly) {
            // Check if index is in bounds
            if (index >= 0 && index < allyDiceUI.Count) {
                // Roll dice
                Destroy(allyDiceUI[index]);
                allyDiceUI.Remove(allyDiceUI[index]);
            }
        }
        else {
            // Check if index is in bounds
            if (index >= 0 && index < enemyDiceUI.Count) {
                Destroy(enemyDiceUI[index]);
                allyDiceUI.Remove(enemyDiceUI[index]);
            }
        }
    }

    public void DisplayUnitActions(Unit unit) {
        // Clear any existing actions if exists
        ClearUnitActions();

        // Get all possible actions from unit and display em
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
