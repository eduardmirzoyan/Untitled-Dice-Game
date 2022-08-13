using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// [System.Serializable]
// public struct Combatant {
//     public Unit unit;
//     public int partyIndex;
//     public Vector3Int worldPosition;
//     public bool isAllyAllegiance;
    
//     // Temp
//     private HealthbarUI healthbar;

//     public Combatant(Unit unit, int partyIndex, Vector3Int worldPosition, bool isAllyAllegiance) {
//         this.unit = unit;
//         this.partyIndex = partyIndex;
//         this.worldPosition = worldPosition;
//         this.isAllyAllegiance = isAllyAllegiance;
//         // this.healthbar = healthbar;

//         // Initialize health
//         // healthbar.Initialize(unit.maxHealth, unit.currentHealth);
//     }

//     public void TakeDamage(int amount) {
//         // Calls unit
//         unit.TakeDamage(amount);

//         // Spawn damage prefab

//         // Updates health bar UI
//         healthbar.UpdateHealth(unit.currentHealth);
//     }
// }

public class CombatManager : MonoBehaviour
{
    private enum CombatState { CombatStart, RoundStart, TurnStart, RoundEnd, CombatEnd };

    private enum TurnState { SelectingAction, SelectingTarget, Confirming, Finished };

    public static CombatManager instance;

    [SerializeField] private CombatState state;
    [SerializeField] private Party allyParty;
    [SerializeField] private Party enemyParty;
    [SerializeField] private List<Dice> allyDice; // Remove this as soon as all units have their own dice
    [SerializeField] private List<Dice> enemyDice; // Remove this as soon as all units have their own dice
    [SerializeField] private Combatant currentCombatant;
    [SerializeField] private Queue<Combatant> turnQueue;
    [SerializeField] private int roundNumber;

    [Header("Settings")]
    [SerializeField] private Vector3Int[] allyPositions;
    [SerializeField] private Vector3Int[] enemyPositions;

    [SerializeField] private List<Combatant> allyCombatants;
    [SerializeField] private List<Combatant> enemyCombatants;

    [SerializeField] private Action selectedAction;
    [SerializeField] private List<Combatant> selectedTargets;

    private Coroutine coroutine;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        
        // Initialize to null
        selectedTargets = null;

        // Set starting state
        state = CombatState.CombatStart;

        // Start combat
        coroutine = StartCoroutine(StartCombat());
    }

    private void Update() {


        // Temp confirm action
        if (Input.GetKeyDown(KeyCode.Return) && state == CombatState.TurnStart) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }

            if (selectedTargets != null) {
                coroutine = StartCoroutine(ConfirmAction());
            }
            else {
                coroutine = StartCoroutine(EndTurn());
            }
        }

        // Debugging
        if(Input.GetKeyDown(KeyCode.G)) {
            CombatManagerUI.instance.SpawnFloatingNumber("3", Vector3.zero);
        }
    }

    /// Main Turn-based logic ~~~~~~~~~~~~~~~~~~~~~~
    # region Turn-based Logic 
    
    private IEnumerator StartCombat() {
        // Debug Feedback
        print("Combat Start");

        // Visuals
        CombatManagerUI.instance.EnterState("Combat Start");
        yield return new WaitForSeconds(0.5f);

        // Change state
        state = CombatState.RoundStart;

        // Set round number
        roundNumber = 0;

        // Set up board
        yield return SetupCombat();

        // Start the first round
        yield return StartRound();
    }

    private IEnumerator StartRound()
    {
        // Debug Feedback
        print("Round Start");

        // Visuals
        CombatManagerUI.instance.EnterState("Round Start");
        yield return new WaitForSeconds(0.5f);

        // Change state
        state = CombatState.RoundStart;

        // Increment round counter
        roundNumber++;

        // Update visuals
        CombatManagerUI.instance.SetUpRound(roundNumber);

        // Roll Dice in sequence
        yield return GenerateDiceRolls();

        // Activate any "on round-start" passives
        // TODO

        // Generate turn order as a queue
        GenerateTurnOrder();

        // Check if queue is empty
        if (turnQueue.Count <= 0) {
            // No units alive, end combat
            yield return EndCombat();
        }
        else {
            // There is at least one unit that can make a turn
            yield return StartTurn();
        }

    }

    private IEnumerator StartTurn()
    {
        // Change state to turn start
        state = CombatState.TurnStart;

        // Use the first entity in the queue
        currentCombatant = turnQueue.Peek();

        // Debug Feedback
        print("Turn Start: " + currentCombatant.unit.name);

        // Visuals
        CombatManagerUI.instance.EnterState(currentCombatant.unit.name + "'s Turn");
        yield return new WaitForSeconds(0.5f);

        // Select (Outline) the tile that the unit is one, visual feedback
        CombatManagerUI.instance.SetTurn(currentCombatant.worldPosition);

        // If Unit is NPC then let it decide its own action
        if (currentCombatant.unit.isNPC) {
            // TODO AI shit
        }
        else {
            // Show visuals for what the current unit can do
            // TODO
            CombatManagerUI.instance.DisplayUnitActions(currentCombatant.unit);

            // Allow the player to interact with dice now
            CombatManagerUI.instance.EnableAllyDice();

            // Save reference
        }

        // Figure out from here cuz after this another coroutine should be started after turn is chosen
        yield return null;

        // If enemy turn, then do AI turn
        // If player turn, enable UI interaction for player
        // After turn,
            // queue is empty, then change state to round end
            // else set turn to next entity in queue
            
    }

    private IEnumerator EndRound() {
        // Debug Feedback
        print("Round End");

        // Change state to roundstart
        state = CombatState.RoundEnd;
        
        // Clear visuals
        CombatManagerUI.instance.ClearAllDiceUI();

        // Trigger round end effects
        // TODO

        // Start new round
        yield return StartRound();
    }

    private IEnumerator EndCombat() {
        // Debug Feedback
        print("Combat End");

        // Change state
        state = CombatState.CombatEnd;

        // TODO

        // End, for now
        yield return null;
    }

    #endregion

    /// Helper functions ~~~~~~~~~~~~~~~~~~~~~~~~

    private IEnumerator SetupCombat()
    {
        // TODO Finish

        // Generate combatants for this particular combat
        GenerateCombatants();

        // Spawn ally models onto map
        CombatManagerUI.instance.SpawnCombatants(allyCombatants);

        // Spawn enemy models onto map
        CombatManagerUI.instance.SpawnCombatants(enemyCombatants);

        yield return null;
    }

    /// Give the party members important info regarding current combat
    private void GenerateCombatants() {

        // Location of the unit in the battle field
        Vector3Int worldPosition;
        
        // Generate ally combatants
        allyCombatants = new List<Combatant>();
        for (int i = 0; i < allyPositions.Length; i++) {
            if (allyParty[i] != null) {
                worldPosition = allyPositions[i];
                var combatant = new Combatant(allyParty[i], i, worldPosition, true);
                // var combatant = ScriptableObject.CreateInstance<Combatant>();
                allyCombatants.Add(combatant);
            }
        }

        // Generate enemy combatants
        enemyCombatants = new List<Combatant>();
        for (int i = 0; i < enemyPositions.Length; i++) {
            if (enemyParty[i] != null) {
                worldPosition = enemyPositions[i];
                var combatant = new Combatant(enemyParty[i], i, worldPosition, false);
                enemyCombatants.Add(combatant);
            }
        }
    }

    // private void SpawnCombatants() {
    //     // Spawn allies
    //     foreach (var ally in allyCombatants) {
    //         var position = CombatManagerUI.instance.GetCellCenter(ally.worldPosition);
    //         // Spawn ally model
    //         CombatManagerUI.instance.SpawnCombatant(ally.unit, position);

    //         // Spawn animation?
    //     }

    //     // Spawn enemies
    //     foreach (var enemy in enemyCombatants) {
    //         var position = CombatManagerUI.instance.GetCellCenter(enemy.worldPosition);
    //         // Spawn ally model
    //         CombatManagerUI.instance.SpawnCombatant(enemy.unit, position);
    //         // Spawn animation?
    //     }
    // }

    private IEnumerator GenerateDiceRolls() {
        // Empty current dice sets
        allyDice = new List<Dice>();
        enemyDice = new List<Dice>();

        // Create outline visuals
        CombatManagerUI.instance.GenerateOutlineUI(allyCombatants.Count, enemyCombatants.Count);

        // Generate ally dice
        foreach (var combatant in allyCombatants) {
            // If unit exists and has a die then add it
            if (combatant.unit.dice != null) {
                // Create temp die
                // TODO: Change this
                Dice dice = ScriptableObject.CreateInstance<Dice>();
                allyDice.Add(dice);

                // Create UI
                CombatManagerUI.instance.SpawnNewDice(dice, true, combatant.partyIndex);
                // Roll animation
                CombatManagerUI.instance.RollDice(true, combatant.partyIndex);
                // Wait until animation is over
                yield return new WaitForSeconds(DiceUI.rollDuration + 0.5f);
            }
        }

        // Generate ally dice
        foreach (var combatant in enemyCombatants) {
            // If unit exists and has a die then add it
            if (combatant.unit.dice != null) {
                // Create temp die
                // TODO: Change this
                Dice dice = ScriptableObject.CreateInstance<Dice>();
                enemyDice.Add(dice);

                // Create UI
                CombatManagerUI.instance.SpawnNewDice(dice, false, combatant.partyIndex);
                // Roll animation
                CombatManagerUI.instance.RollDice(false, combatant.partyIndex);
                // Wait until animation is over
                yield return new WaitForSeconds(DiceUI.rollDuration + 0.5f);
            }
        }

    }

    /// Generate turn order queue based on speed values of all combatants
    private void GenerateTurnOrder() {
        // Get list of all combatants
        List<Combatant> allCombatants = allyCombatants.Concat<Combatant>(enemyCombatants).ToList<Combatant>();
        // Randomize list
        allCombatants.Sort((combatant1, combatant2) => UnityEngine.Random.value.CompareTo(UnityEngine.Random.value));
        // Sort by speed value
        allCombatants.Sort((combatant1, combatant2) => combatant2.unit.speed.CompareTo(combatant1.unit.speed));
        // Turn into queue
        turnQueue = new Queue<Combatant>(allCombatants);

        // Visuals
        CombatManagerUI.instance.SetUpTurnQueue(turnQueue);
    }

    private IEnumerator EndTurn() {
        // If it is currently a unit's turn, then end it
        if (state == CombatState.TurnStart) {

            // First check if all allies or all enemies are dead
            if (allyParty.GetMembers().All(unit => unit.IsDead()) || enemyParty.GetMembers().All(unit => unit.IsDead())) {
                // End combat
                yield return EndCombat();
                // Finish
                yield return null;
            }

            // Remove the current unit from queue
            turnQueue.Dequeue();

            // Handle visuals
            CombatManagerUI.instance.PopFromTurnQueue();
            CombatManagerUI.instance.ClearUnitActions();

            // If all units took their turn, start new round if possible or end combat
            if (turnQueue.Count == 0) {
                // End the current round
                yield return EndRound();
            }
            else {
                // Start turn on the next person in queue
                yield return StartTurn();
            }
        }
    }

    public void SelectAction(Action action) {
        // Update selected action
        selectedAction = action;

        // If given action is null, then Player wants to de-select action
        if (selectedAction != null) {
            // Select a default target if possible
            SelectDefaultTarget();
        }
        else {
            // Clear any targets
            selectedTargets = null;

            // Update visuals
            CombatManagerUI.instance.ClearTargets();
        }

        // Unselect other actions if possible
        CombatManagerUI.instance.DeSelectOtherActions(action);
    }

    private void SelectDefaultTarget() {
        // Loop through all possible slots
        if (selectedAction.canTargetSelf) {
            // Select self
            SelectTarget(currentCombatant.worldPosition);
            return;
        }
        else if (selectedAction.canTargetEnemies) {
            foreach (var enemy in enemyCombatants) {
                bool result = SelectTarget(enemy.worldPosition);
                // If a valid target has been chosen, then dip
                if (result)
                    return;
            }
        }
        else if (selectedAction.canTargetAllies) {
            foreach (var ally in allyCombatants) {
                bool result = SelectTarget(ally.worldPosition);
                // If a valid target has been chosen, then dip
                if (result)
                    return;
            }
        }
    }

    public bool isValidTarget(Combatant sourceCombatant, Combatant targetCombatant, Action action) {
        
        // If action cannot target allies, and the units are from the same side, then invalid
        if (!action.canTargetAllies && sourceCombatant.isAllyAllegiance == targetCombatant.isAllyAllegiance) {
            return false;
        }

        // If action cannot target enemies, and the units are from different sides, then invalid
        if (!action.canTargetEnemies && sourceCombatant.isAllyAllegiance != targetCombatant.isAllyAllegiance) {
            return false;
        }

        // Should not be able to target self if not allowed
        if (!action.canTargetSelf && sourceCombatant.isAllyAllegiance == targetCombatant.isAllyAllegiance && sourceCombatant.partyIndex == targetCombatant.partyIndex) {
            return true;
        }

        return true;
    }

    public bool SelectTarget(Vector3Int position) {
        // Check if target is valid
        // TODO

        // Clear any current targets if possible
        if (selectedTargets != null) {
            // Clear visuals
            CombatManagerUI.instance.ClearTargets();
            // Reset list of targets
            selectedTargets = null;
        }

        // Initialize
        selectedTargets = new List<Combatant>();
        
        // Loop through all allies
        foreach (var combatant in allyCombatants) {
            if (combatant.worldPosition == position) {
                // If the target is not valid
                if (!isValidTarget(currentCombatant, combatant, selectedAction)) {
                    return false;
                }

                selectedTargets.Add(combatant);
                
                // If no valid targets were chosen
                if (selectedTargets.Count == 0) {
                    return false;
                }

                // Update visuals
                foreach (var target in selectedTargets) {
                    CombatManagerUI.instance.HighlightTarget(target.worldPosition);
                }

                return true;
            }
        }

        // Loop through all enemies
        foreach (var combatant in enemyCombatants) {
            if (combatant.worldPosition == position) {
                // If the target is not valid
                if (!isValidTarget(currentCombatant, combatant, selectedAction)) {
                    return false;
                }

                selectedTargets.Add(combatant);

                // Get any secondary targets based on action
                selectedTargets.AddRange(selectedAction.getSecondaryTargets());
                
                // If no valid targets were chosen
                if (selectedTargets.Count == 0) {
                    return false;
                }

                // Update visuals
                foreach (var target in selectedTargets) {
                    CombatManagerUI.instance.HighlightTarget(target.worldPosition);
                }

                return true;
            }
        }

        return false;
    }

    private IEnumerator ConfirmAction() {
        // Error handling ~~~~~~~~~~~~~~~~~~
        if (selectedAction == null) {
            throw new Exception("Action has been confirmed without selecting an action");
        }

        if (selectedTargets == null) {
            throw new Exception("Action has been confirmed without selecting any targets");
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Perform action on targets
        selectedAction.Perform(selectedTargets, currentCombatant.unit.dice);

        // TODO Delete used Dice
        // CombatManagerUI.instance.DeleteDiceUI();

        // Clear visuals
        CombatManagerUI.instance.ClearTargets();

        // Clear action
        selectedAction = null;

        // Clear targets
        selectedTargets = null;

        // Change state?

        // End Turn
        yield return EndTurn();
    }

    public bool hasActionBeenChoosen() {
        return selectedAction != null;
    }
    
}
