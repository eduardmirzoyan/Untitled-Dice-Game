using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hurtParticlesPrefab;
    [SerializeField] private GameObject deadParticlesPrefab;

    [Header("Settings")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string hurtState = "Hurt";
    [SerializeField] private string deadState = "Dead";

    [SerializeField] private Combatant combatant;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    public void Initialize(Combatant combatant) {
        this.combatant = combatant;

        // Subscribe to related events
        CombatEvents.instance.onTakeDamage += Hurt;
        CombatEvents.instance.onDie += Dead;

        // Default to idle state
        animator.Play(idleState);
    }

    public void Hurt(Combatant combatant, int value) {
        if (this.combatant == combatant) {
            // Change animation
            animator.Play(hurtState);
            // Spawn visuals
            if (hurtParticlesPrefab != null)
                Instantiate(hurtParticlesPrefab, transform);
        }
    }

    public void Dead(Combatant combatant) {
        if (this.combatant == combatant) {
            // Change animation
            animator.Play(deadState);
            // Spawn visuals
            if (deadParticlesPrefab != null)
                Instantiate(deadParticlesPrefab, transform);
        }
    }
    
}
