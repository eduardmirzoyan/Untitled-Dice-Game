using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Slime")]
public class SlimeAI : ScriptableObject
{
    // Should choose best action given all the combatants, actions and die pool
    public (Action, Dice, Combatant) SelectBestActionAndTarget(List<Combatant> combatants, List<Action> actions, List<Dice> dicepool) {
        Action bestAction = null;
        Dice bestDie = null;
        int heuristic = 0;
        foreach (var action in actions) {
            if (action is AttackAction) {

                var attack = (AttackAction) action;
                foreach (var die in dicepool) {
                    // Make sure die passes contraints
                    if (attack.checkDieConstraints(die)) {
                        // Check to see if attack is better than previous
                        if (attack.FinalDamageValue() > heuristic) {
                            heuristic = attack.FinalDamageValue();
                            bestDie = die;
                            bestAction = action;
                        }
                    }
                }
            }
        }

        // Now choose random target
        int choice = Random.Range(0, 4);

        // Return the best pair so far
        return (bestAction, bestDie, combatants[choice]);
    }
}
