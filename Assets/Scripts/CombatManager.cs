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

    [Header("Combat Settings")]
    public float rollTime = 0.3f;
    public float waitTime = 1f;
    public float attackTime = 1f;
    public float hoverTime = 1f;
    public float spawnTime = 1f;

    [SerializeField] private CombatState state;
    [SerializeField] private Party allyParty;
    [SerializeField] private Party enemyParty;
    [SerializeField] public Combatant currentCombatant;
    [SerializeField] private Queue<Combatant> turnQueue;
    [SerializeField] private int roundNumber;

    [Header("Settings")]
    [SerializeField] private Vector3Int[] allyPositions;
    [SerializeField] private Vector3Int[] enemyPositions;

    [Header("Combatants")]
    [SerializeField] public List<Combatant> combatants;

    [Header("Selection")]
    [SerializeField] public Dice selectedDie;
    [SerializeField] public Action selectedAction;
    [SerializeField] public Combatant selectedTarget;

    [Header("Temp Shit")]
    [SerializeField] private StatusEffect statusEffect;

    private Coroutine coroutine;
    private string endMessage = "Draw";
    public bool isPlayerTurn;

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
        
        // Get party from Game manager
        allyParty = GameManager.instance.allyParty;
        enemyParty = GameManager.instance.enemyParty;
    }

    private void Start() {
        // Deploy party
        GameManager.instance.DeployPlayerParty();

        // Start combat
        coroutine = StartCoroutine(StartCombat());
    }

    private void Update() {
        // Temp confirm action
        if (Input.GetKeyDown(KeyCode.Space) && state == CombatState.TurnStart) {
            Confirm();
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.H) && state == CombatState.TurnStart) {
            // Emulate adding effect
            currentCombatant.AddStatusEffect(Instantiate(statusEffect));
        }

        if (Input.GetKeyDown(KeyCode.J) && state == CombatState.TurnStart)
        {
            // Emulate removing effect
            //currentCombatant.RemoveStatusEffect(statusEffect);
        }
    }

    /// Main Turn-based logic ~~~~~~~~~~~~~~~~~~~~~~
    # region Turn-based Logic 
    
    private IEnumerator StartCombat() {
        // Debug Feedback
        print("Combat Start");

        yield return new WaitForSeconds(0.5f);

        // Trigger event
        GameEvents.instance.TriggerOnSetParty(allyParty);

        // Change state
        state = CombatState.RoundStart;

        // Set round number
        roundNumber = 0;

        // Set up board
        yield return SetupCombat();

        // Visuals
        CombatEvents.instance.TriggerOnShowBanner("Combat Start");
        yield return new WaitForSeconds(0.5f);

        // Trigger event
        CombatEvents.instance.TriggerOnCombatStart(0);

        // Trigger event
        yield return CombatEvents.instance.TriggerSkillCombatStart(new ActionInfo(0.5f));

        // Visuals
        CombatEvents.instance.TriggerOnHideBanner("Combat Start");
        yield return new WaitForSeconds(0.5f);

        // Start the first round
        yield return StartRound();
    }

    private IEnumerator StartRound()
    {
        // Debug Feedback
        print("Round Start");

        // Change state
        state = CombatState.RoundStart;

        // Visuals
        CombatEvents.instance.TriggerOnShowBanner("Round Start");
        yield return new WaitForSeconds(0.5f);

        // Increment round counter
        roundNumber++;

        // Trigger Event
        CombatEvents.instance.TriggerOnRoundStart(0);

        // Roll Dice in sequence
        yield return GenerateDiceRolls();

        // Trigger skill event
        yield return CombatEvents.instance.TriggerSkillRoundStart(new ActionInfo(0.5f));

        // Generate turn order as a queue
        GenerateTurnOrder();

        // Visuals
        CombatEvents.instance.TriggerOnHideBanner("Round Start");
        yield return new WaitForSeconds(0.5f);

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

        // If unit is dead, then pass action
        if (currentCombatant.unit.IsDead()) {
            // Pass their action
            yield return ConfirmAction();
        }

        // SO FAR, it's a player turn if the combatant is an ally
        isPlayerTurn = currentCombatant.isAlly();

        // Visuals
        CombatEvents.instance.TriggerOnShowBanner("Turn Start: " + currentCombatant.unit.name);
        yield return new WaitForSeconds(0.5f);

        // Trigger event
        CombatEvents.instance.TriggerOnTurnStart(0);

        // Trigger skill event
        yield return CombatEvents.instance.TriggerSkillTurnStart(new ActionInfo(0.5f), currentCombatant);

        // If Unit is NPC then let it decide its own action
        if (currentCombatant.unit.ai != null) {
        
            // Find best triple using AI
            (Action, Dice, Combatant) bestChoice = 
                    currentCombatant.unit.ai.SelectBestActionAndTarget(combatants, currentCombatant.unit.GetActions(), currentCombatant.dicePool);
            // Debugging
            print(currentCombatant.unit.name + " is deciding it's action...");

            yield return new WaitForSeconds(0.5f);

            // Select action
            SelectAction(bestChoice.Item1, bestChoice.Item2);
            // Debugging
            print(currentCombatant.unit.name + " choose action: " + bestChoice.Item1.name 
                                                    + " with die value: " + bestChoice.Item2.GetValue());

            // Visually show die being selected here
            // TODO

            // Select target
            SelectTarget(bestChoice.Item3);
            // Debugging
            print(currentCombatant.unit.name + " choose it's target: " + bestChoice.Item3.unit.name);

            // Trigger event
            CombatEvents.instance.TriggerOnFeedback(currentCombatant.unit.name + " used <color=yellow>" + bestChoice.Item1.name + "</color> on " + bestChoice.Item3.unit.name);

            yield return new WaitForSeconds(1.5f);

            // Confirm action
            yield return ConfirmAction();
        }
        else {
            // Start listening for events
            CombatEvents.instance.onActionSelect += SelectAction;

            // Trigger event for visuals
            CombatEvents.instance.TriggerOnPlayerTurnStart(0);
        }

        // Figure out from here cuz after this another coroutine should be started after turn is chosen
        yield return null; 
    }

    private IEnumerator EndRound() {
        // Change state to roundstart
        state = CombatState.RoundEnd;

        // Debug Feedback
        print("Round End");

        // Visuals
        CombatEvents.instance.TriggerOnShowBanner("Round End");
        yield return new WaitForSeconds(0.5f);
        
        // Trigger event
        CombatEvents.instance.TriggerOnRoundEnd(0);

        // Trigger round end events
        yield return CombatEvents.instance.TriggerSkillRoundEnd(new ActionInfo(0.5f));

        // Visuals
        CombatEvents.instance.TriggerOnHideBanner("Round End");
        yield return new WaitForSeconds(0.5f);

        // Start new round
        yield return StartRound();
    }

    private IEnumerator EndCombat() {
        // Change state
        state = CombatState.CombatEnd;

        // Debug Feedback
        print("Combat End");

        // Visuals
        CombatEvents.instance.TriggerOnShowBanner("Combat End: " + endMessage);
        yield return new WaitForSeconds(0.5f);

        // Trigger event
        CombatEvents.instance.TriggerOnCombatEnd(0);

        // Trigger end of combat events
        yield return CombatEvents.instance.TriggerSkillCombatEnd(new ActionInfo(0.5f));

        // Terminate passives
        foreach (var combatant in combatants) {
            foreach (var passive in combatant.unit.GetPassives()) {
                passive.Uninitialize();
            }
        }

        // End, for now
        yield return null;
    }

    #endregion

    /// Helper functions ~~~~~~~~~~~~~~~~~~~~~~~~

    /// Give the party members important info regarding current combat
    private void GenerateCombatants() {
        // Location of the unit on the battle field
        Vector3Int hexPosition;
        combatants = new List<Combatant>();

        // Turn allies into combatants, giving them index 0-3
        for (int i = 0; i < allyPositions.Length; i++) {
            if (allyParty[i] != null) {
                // Assign model's location in this combat
                hexPosition = allyPositions[i];

                // Create combatant SO
                var combatant = ScriptableObject.CreateInstance<Combatant>();
                combatant.Initialize(allyParty[i], allyParty.dicePool, i, hexPosition);
                combatants.Add(combatant);

                // Initialize skills
                foreach (var skill in allyParty[i].GetSkills()) {
                    skill.Initialize(combatant);
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
                hexPosition = enemyPositions[i];

                // Create combatant SO
                var combatant = ScriptableObject.CreateInstance<Combatant>();
                // Make sure to offset index by 4 since they are on the enemy team
                combatant.Initialize(enemyParty[i], enemyParty.dicePool, i + 4, hexPosition);
                combatants.Add(combatant);

                // Initialize skills
                foreach (var skill in enemyParty[i].GetSkills()) {
                    skill.Initialize(combatant);
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

        for (int i = 0; i < 4; i++) {
            // Spawn two units at a time
            CombatEvents.instance.TriggerOnSpawnCombatant(combatants[i], spawnTime);
            CombatEvents.instance.TriggerOnSpawnCombatant(combatants[i + 4], spawnTime);

            // Wait for animation
            yield return new WaitForSeconds(spawnTime / 2);
        }

        yield return null;
    }

    private IEnumerator GenerateDiceRolls() {
        foreach (var combatant in combatants) {
            var die = combatant.unit.dice;
            if (combatant != null && die != null) {

                // Set the die state based on if combatant is alive
                if (combatant.unit.IsDead()) die.Exhaust();
                else die.Replenish();

                // Spawn die (UI)
                CombatManagerUI.instance.SpawnDie(combatant);

                // Roll die (logic)
                die.Roll();
                
                // Trigger event
                CombatEvents.instance.TriggerOnRoll(die);

                // Wait until animation is over

                // Make animation fast if in mode
                if (GameManager.instance.fastMode) yield return new WaitForSeconds(0.1f);
                else yield return new WaitForSeconds(rollTime + waitTime);
                
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

            // Visuals
            CombatEvents.instance.TriggerOnHideBanner("Turn End");
            yield return new WaitForSeconds(0.5f);

            // Trigger event
            CombatEvents.instance.TriggerOnTurnEnd(0);
            
            // Trigger event
            if (isPlayerTurn)
                CombatEvents.instance.TriggerOnPlayerTurnEnd(0);

            // Remove the current unit from queue
            turnQueue.Dequeue();

            // Trigger event
            CombatEvents.instance.TriggerDequeue(currentCombatant);

            // If all units took their turn, start new round if possible or end combat
            
            // Check if enemies are all dead
            if (enemyParty.IsDead()) {
                // Win combat
                endMessage = "YOU WIN!";

                yield return EndCombat();
            }
            // Check if allies are all dead
            else if (allyParty.IsDead()) {
                // Lose combat
                endMessage = "YOU LOSE :(";

                yield return EndCombat();
            }
            else if (turnQueue.Count == 0) {
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
        if (selectedAction == null) {
            // Check if any targets need to be de-selected
            if (selectedTarget != null) {
                // Clear target
                selectedTarget = null;

                // Trigger event
                CombatEvents.instance.TriggerOnTargetSelect(null);
            }
        }
    }

    private void SelectDefaultTarget() {
        // Check self
        if (selectedAction.canTargetSelf && selectedAction.CheckTargetConstraints(currentCombatant)) {
            // Select self
            SelectTarget(currentCombatant);
            return;
        }

        foreach (var combatant in combatants) {
            // Skip combatant if it's dead
            if (combatant.unit.IsDead()) continue;

            if (combatant != null && combatant.index != currentCombatant.index) {
                
                if (currentCombatant.isAlly()) {
                    if (selectedAction.canTargetAllies && combatant.isAlly() && selectedAction.CheckTargetConstraints(combatant))
                    {
                        // Select target
                        SelectTarget(combatant);
                        return;
                    }
                    else if (selectedAction.canTargetEnemies && !combatant.isAlly() && selectedAction.CheckTargetConstraints(combatant))
                    {
                        // Select target
                        SelectTarget(combatant);
                        return;
                    }
                }
                else {
                    if (selectedAction.canTargetAllies && !combatant.isAlly() && selectedAction.CheckTargetConstraints(combatant))
                    {
                        // Select target
                        SelectTarget(combatant);
                        return;
                    }
                    else if (selectedAction.canTargetEnemies && combatant.isAlly() && selectedAction.CheckTargetConstraints(combatant))
                    {
                        // Select target
                        SelectTarget(combatant);
                        return;
                    }
                }
            }
        }

        print("NO DEFAULT TARGET WAS ABLE TO BE CHOSEN FOR: " + selectedAction.name);
    }

    public Combatant GetCombatantAtPosition(Vector3Int position) {
        foreach (var combatant in combatants) {
            if (combatant.hexPosition == position) {
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

        // Check if target passes constraints
        if (!selectedAction.CheckTargetConstraints(combatant)) {
            selectedAction.ShowTargetConstraintFeedback(combatant);
            return;
        }

        // Clear any previous targets if possible
        if (selectedTarget != null) {
            // Reset list of targets
            selectedTarget = null;

            // Trigger event
            CombatEvents.instance.TriggerOnTargetSelect(null);
        }

        // Set new selected target
        selectedTarget = combatant;

        // Event
        CombatEvents.instance.TriggerOnTargetSelect(selectedTarget);
    }

    public void Pass() {
        // Deubug
        print(currentCombatant.unit.name + " passed their turn.");

        // Clear action
        selectedAction = null;

        // Clear die
        selectedDie = null;

        // Clear target
        selectedTarget = null;

        // Change to confirm
        Confirm();
    }

    public void Confirm() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(ConfirmAction());
    }

    private IEnumerator ConfirmAction() {
        // Stop listening for actions
        CombatEvents.instance.onActionSelect -= SelectAction;

        // Trigger event
        CombatEvents.instance.TriggerOnPreActionConfirm(selectedAction);

        // Trigger event
        CombatEvents.instance.TriggerOnActionConfirm(currentCombatant, selectedAction, selectedDie, selectedTarget);

        // Error handling ~~~~~~~~~~~~~~~~~~
        if (selectedAction == null || selectedDie == null || selectedTarget == null) {
            // Deubug
            print("Turn has been confirmed without selecting an action and die and target.");

            // Clear potentional action
            selectedAction = null;

            // Clear potentional die
            selectedDie = null;

            // Clear potential target
            selectedTarget = null;
            
            yield return EndTurn();
        }

        // Move to target animation
        if (selectedAction is AttackAction)
            yield return TravelToTarget();

        // Perform selected action on selected target using selected die
        selectedAction.Perform(selectedTarget.index, combatants, selectedDie);

        // Trigger event
        CombatEvents.instance.TriggerOnActionPerformed(selectedAction);

        // Update uses and cooldown of action
        selectedAction.SetCooldown();
        selectedAction.UpdateUses();

        // Set selected die innactive
        selectedDie.Exhaust();

        // Trigger event
        CombatEvents.instance.TriggerOnExhaust(selectedDie);

        // Hover for dramatic effect
        if (selectedAction is AttackAction)
            yield return HoverOverTarget();

        // Then return model
        currentCombatant.modelTransform.position = CombatManagerUI.instance.GetCellCenter(currentCombatant.hexPosition);

        // Clear action
        selectedAction = null;

        // Clear die
        selectedDie = null;

        // Clear target
        selectedTarget = null;

        // End Turn
        yield return EndTurn();
    }

    private IEnumerator TravelToTarget() {
        var startPos = currentCombatant.modelTransform.position;
        var endPos = selectedTarget.modelTransform.position;
        var direction = (endPos - startPos).normalized / 2;
        float timer = 0;
        
        while (timer < attackTime) {
            // Lerp model towards other model
            currentCombatant.modelTransform.position = Vector3.Lerp(startPos, endPos - direction, timer / attackTime);

            // Increment time
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator HoverOverTarget() {
        var startPos = currentCombatant.modelTransform.position;
        var endPos = selectedTarget.modelTransform.position;
        var direction = (endPos - startPos).normalized / 2;
        float timer = 0;

        while (timer < hoverTime)
        {
            // Lerp model towards other model
            currentCombatant.modelTransform.position = Vector3.Lerp(startPos, endPos + direction, timer / hoverTime);

            // Increment time
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public bool hasActionBeenChoosen() {
        return selectedAction != null;
    }
    
    private void Add() {
        
    }
}
