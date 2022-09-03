using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    private enum CombatState { CombatStart, RoundStart, TurnStart, RoundEnd, CombatEnd };

    private enum TurnState { SelectingAction, SelectingTarget, Confirming, Finished };

    public static CombatManager instance;

    [SerializeField] private CombatState state;
    [SerializeField] private Party allyParty;
    [SerializeField] private Party enemyParty;
    [SerializeField] private Combatant currentCombatant;
    [SerializeField] private Queue<Combatant> turnQueue;
    [SerializeField] private int roundNumber;

    [Header("Settings")]
    [SerializeField] private Vector3Int[] allyPositions;
    [SerializeField] private Vector3Int[] enemyPositions;

    [Header("Combatants")]
    [SerializeField] private List<Combatant> combatants;

    [Header("Selection")]
    [SerializeField] private Dice selectedDie;
    [SerializeField] private Action selectedAction;
    [SerializeField] private Combatant selectedTarget;

    private Coroutine coroutine;

    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        
        // Initialize to null
        selectedTarget = null;

        // Set starting state
        state = CombatState.CombatStart;

        // TEMP ~~~~~~~~~~~~~~~~~~~~~
        // allyParty.FormPool();
        // enemyParty.FormPoll();
        // TEMP ~~~~~~~~~~~~~~~~~~~~~
        
        // Get party from Game manager
        allyParty = GameManager.instance.allyParty;
        enemyParty = GameManager.instance.enemyParty;

        // Start combat
        coroutine = StartCoroutine(StartCombat());
    }

    private void Update() {
        // Temp confirm action
        if (Input.GetKeyDown(KeyCode.Space) && state == CombatState.TurnStart) {
            Confirm();
        }
    }

    /// Main Turn-based logic ~~~~~~~~~~~~~~~~~~~~~~
    # region Turn-based Logic 
    
    private IEnumerator StartCombat() {
        // Debug Feedback
        print("Combat Start"); 

        // Change state
        state = CombatState.RoundStart;

        // Set round number
        roundNumber = 0;

        // Set up board
        yield return SetupCombat();

        // Visuals
        CombatManagerUI.instance.EnterState("Combat Start");
        yield return new WaitForSeconds(0.5f);

        // Trigger event
        yield return CombatEvents.instance.TriggerCombatStart(new ActionInfo(0.5f));

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
        // CombatManagerUI.instance.SetUpRound(roundNumber);

        // Roll Dice in sequence
        yield return GenerateDiceRolls();

        // Activate any "on round-start" passives
        // Trigger event
        yield return CombatEvents.instance.TriggerRoundStart(new ActionInfo(0.5f));

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
        if (currentCombatant.unit.ai != null) {
        
            // Find best triple
            (Action, Dice, Combatant) bestChoice = 
                    currentCombatant.unit.ai.SelectBestActionAndTarget(combatants, currentCombatant.unit.GetActions(), currentCombatant.dicePool);
            print(currentCombatant.unit.unitName + " is deciding it's action...");

            yield return new WaitForSeconds(1f);

            // Select action
            SelectAction(bestChoice.Item1, bestChoice.Item2);
            print(currentCombatant.unit.unitName + " choose action: " + bestChoice.Item1.name 
                                                    + " with die value: " + bestChoice.Item2.GetValue());
            
            yield return new WaitForSeconds(1f);

            // Select target
            SelectTarget(bestChoice.Item3);
            print(currentCombatant.unit.unitName + " choose it's target: " + bestChoice.Item3.unit.unitName);

            yield return new WaitForSeconds(1f);

            // Confirm
            yield return ConfirmAction();
        }
        else {
            // Show visuals for what the current unit can do
            // TODO
            CombatManagerUI.instance.DisplayActions(currentCombatant.unit);

            // Allow the player to interact with dice now
            CombatManagerUI.instance.EnableAllyDice();

            // Start listening for events
            CombatEvents.instance.onDieInsert += SelectAction;

            // Save reference
        }

        // Figure out from here cuz after this another coroutine should be started after turn is chosen
        yield return null; 
    }

    private IEnumerator EndRound() {
        // Debug Feedback
        print("Round End");

        // Change state to roundstart
        state = CombatState.RoundEnd;
        
        // Clear visuals
        CombatManagerUI.instance.DespawnDice();

        // Trigger round end events
        yield return CombatEvents.instance.TriggerRoundEnd(new ActionInfo(0.5f));

        // Start new round
        yield return StartRound();
    }

    private IEnumerator EndCombat() {
        // Debug Feedback
        print("Combat End");

        // Change state
        state = CombatState.CombatEnd;

        // Trigger end of combat events
        yield return CombatEvents.instance.TriggerCombatEnd(new ActionInfo(0.5f));

        // Terminate passives
        foreach (var combatant in combatants) {
            foreach (var passive in combatant.unit.GetPassives()) {
                passive.Terminate();
            }
        }

        //

        // End, for now
        yield return null;
    }

    #endregion

    /// Helper functions ~~~~~~~~~~~~~~~~~~~~~~~~

    /// Give the party members important info regarding current combat
    private void GenerateCombatants()
    {
        // Location of the unit on the battle field
        Vector3Int worldPosition;
        combatants = new List<Combatant>();

        // Turn allies into combatants, giving them index 0-3
        for (int i = 0; i < allyPositions.Length; i++) {
            if (allyParty[i] != null) {
                // Assign model's location in this combat
                worldPosition = allyPositions[i];

                var combatant = ScriptableObject.CreateInstance<Combatant>();
                combatant.Initialize(allyParty[i], allyParty.dicePool, i, worldPosition);
                combatants.Add(combatant);

                // Initialize passives
                foreach (var passive in allyParty[i].GetPassives()) {
                    passive.Initialize(combatant);
                }
            }
            else {
                // Make sure to add null spot if no combatant exists in that location
                combatants.Add(null);
            }
        }

        // Turn allies into combatants, giving them index 4-7
        for (int i = 0; i < enemyPositions.Length; i++) {
            if (enemyParty[i] != null) {
                // Assign model's location in this combat
                worldPosition = enemyPositions[i];

                var combatant = ScriptableObject.CreateInstance<Combatant>();
                // Make sure to offset index by 4 since they are on the enemy team
                combatant.Initialize(enemyParty[i], enemyParty.dicePool, i + 4, worldPosition);
                combatants.Add(combatant);

                // Initialize passives
                foreach (var passive in enemyParty[i].GetPassives()) {
                    passive.Initialize(combatant);
                }
            }
            else {
                // Make sure to add null spot if no combatant exists in that location
                combatants.Add(null);
            }
        }
    }

    /// Spawn models
    private IEnumerator SetupCombat()
    {
        // Generate combatants for this particular combat
        GenerateCombatants();

        // Spawn all combatants models onto map
        CombatManagerUI.instance.SpawnModels(combatants);

        yield return null;
    }

    private IEnumerator GenerateDiceRolls() {
        // Spawn die slot outlines
        CombatManagerUI.instance.SpawnDiceOutlines(combatants);

        foreach (var combatant in combatants) {
            var die = combatant.unit.dice;
            if (combatant != null && die != null) {
                // Spawn die (UI)
                CombatManagerUI.instance.SpawnDie(combatant);

                // Roll die (logic)
                die.Roll();
                
                // Roll die (UI)
                CombatEvents.instance.TriggerOnRoll(die);
                // CombatManagerUI.instance.RollDie(combatant.partyIndex);

                // Wait until animation is over
                yield return new WaitForSeconds(DiceUI.rollDuration + 0.5f);
            }
        }
    }

    /// Generate turn order queue based on speed values of all combatants
    private void GenerateTurnOrder() {
        // Filter out dead combatants
        List<Combatant> livingCombatants = combatants.Where(combatant => !combatant.unit.IsDead()).ToList();

        // Randomize list
        livingCombatants.Sort((combatant1, combatant2) => UnityEngine.Random.value.CompareTo(UnityEngine.Random.value));
        // Sort by speed value
        livingCombatants.Sort((combatant1, combatant2) => combatant2.unit.speed.CompareTo(combatant1.unit.speed));
        // Turn into queue
        turnQueue = new Queue<Combatant>(livingCombatants);

        // Trigger event
        CombatEvents.instance.TriggerFormQueue(turnQueue);
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

            // Trigger event
            CombatEvents.instance.TriggerDequeue(currentCombatant);

            // Handle visuals
            CombatManagerUI.instance.ClearActions();

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

    public void SelectAction(Action action, Dice die) {
        // Update selected action
        selectedAction = action;

        // Update selected die
        selectedDie = die;

        // If given action is null, then Player wants to de-select action
        if (selectedAction != null) {
            // Select a default target if possible
            SelectDefaultTarget();
        }
        else {
            // Clear any targets
            selectedTarget = null;

            // Update visuals
            CombatManagerUI.instance.ClearTargets();
        }
    }

    private void SelectDefaultTarget() {
        // Check self
        if (selectedAction.canTargetSelf && selectedAction.checkTargetConstraints(currentCombatant)) {
            // Select self
            SelectTarget(currentCombatant);
            return;
        }

        foreach (var combatant in combatants) {
            // Skip combatant if it's dead
            if (combatant.unit.IsDead()) continue;

            if (combatant != null && combatant.index != currentCombatant.index) {
                if (selectedAction.canTargetAllies && combatant.isAlly() && selectedAction.checkTargetConstraints(combatant)) {
                    // Select target
                    SelectTarget(combatant);
                    return;
                }
                else if (selectedAction.canTargetEnemies && !combatant.isAlly() && selectedAction.checkTargetConstraints(combatant)) {
                    // Select target
                    SelectTarget(combatant);
                    return;
                }
            }
        }

        print("NO DEFAULT TARGET WAS ABLE TO BE CHOSEN FOR: " + selectedAction.name);
    }

    public Combatant GetCombatantAtPosition(Vector3Int position) {
        foreach (var combatant in combatants) {
            if (combatant.worldPosition == position) {
                // Check if target is dead
                if (combatant.unit.IsDead()) {
                    CombatEvents.instance.TriggerOnFeedback("You cannot target DEAD units.");
                    return null;
                }
                return combatant;
            }
        }

        // If nothing found, return null
        return null;
    }

    public void SelectTarget(Combatant combatant) {
        // Check if target is valid
        // TODO

        // Clear any previous targets if possible
        if (selectedTarget != null) {
            // Clear visuals
            CombatManagerUI.instance.ClearTargets();
            // Reset list of targets
            selectedTarget = null;
        }

        // Set new selected target
        selectedTarget = combatant;

        // Update visuals
        CombatManagerUI.instance.HighlightTarget(selectedTarget.worldPosition);
    }

    public void Confirm() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        if (selectedTarget != null) {
            coroutine = StartCoroutine(ConfirmAction());
        }
        else {
            coroutine = StartCoroutine(EndTurn());
        }
    }

    private IEnumerator ConfirmAction() {
        // Error handling ~~~~~~~~~~~~~~~~~~
        if (selectedAction == null) {
            throw new Exception("Action has been confirmed without selecting an action");
        }

        if (selectedTarget == null) {
            throw new Exception("Action has been confirmed without selecting any targets");
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Stop listening for actions
        CombatEvents.instance.onDieInsert -= SelectAction;

        // Perform selected action on selected target using selected die
        selectedAction.Perform(selectedTarget.index, combatants, selectedDie);

        // Trigger event
        CombatEvents.instance.TriggerOnActionConfirm(selectedAction);

        // Clear visuals
        CombatManagerUI.instance.ClearTargets();

        // Clear action
        selectedAction = null;

        // Clear target
        selectedTarget = null;

        // Check if enemies are all dead
        if (enemyParty.IsDead()) {
            // Win combat
            CombatManagerUI.instance.EnterState("YOU WIN!");

            yield return EndCombat();
        }
        // Check if allies are all dead
        else if (allyParty.IsDead()) {
            // Lose combat
            CombatManagerUI.instance.EnterState("YOU LOSE :(");

            yield return EndCombat();
        }

        // End Turn
        yield return EndTurn();
    }

    public bool hasActionBeenChoosen() {
        return selectedAction != null;
    }
    
}
