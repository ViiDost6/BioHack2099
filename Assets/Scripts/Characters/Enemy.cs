using Unity.AI.Navigation;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    private GameObject player; // Reference to the player object
    public NavMeshAgent navMesh; // Reference to the NavMeshSurface component for navigation
    public float detectionRange = 3.0f; // Range within which the enemy can detect the player
    public float attackRange = 1.0f; // Range within which the enemy can attack the player
    public float attackDamage = 10.0f; // Damage dealt to the player on attack
    public float health = 100.0f; // Health of the enemy
    public float attackCooldown = 3.0f; // Time between attacks
    public Transform returnPoint; // Point to which the enemy returns when not attacking


    private float lastAttackTime = 0.0f; // Time of the last attack
    public Animator animator; // Reference to the enemy's animator component
    private PlayerHealth playerHealth; // Reference to the player's health component
    private bool isDead = false; // Flag to check if the enemy is dead
    private bool hasTakenDamage = false; // Flag to check if the enemy has taken damage
    private int damage = 50; // Default value

    void Start()
    {
        // Get the animator component attached to this enemy
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag
        playerHealth = player.GetComponent<PlayerHealth>(); // Get the PlayerHealth component from the player object
        animator = GetComponent<Animator>(); // Get the Animator component attached to this enemy
        //tells navmesh where to go
        navMesh = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this enemy
        navMesh.SetDestination(player.transform.position); // Set the destination of the NavMeshAgent to the player's position
        navMesh.isStopped = false;
        damage = 50; // Set the default damage value
    }


    void Update()
    {
        if (navMesh != null)
        {
            // Check if the player is within attack range
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= attackRange)
            {
                // Attack the player if the cooldown period has passed
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
            
            if (health <= 0 && !isDead)
            {
                Die(); // Call the Die method
            }

            // Updates pathfinding
            if (distanceToPlayer <= detectionRange && !isDead)
            {
                animator.SetTrigger("playerInRange");
                navMesh.SetDestination(player.transform.position); // Update the destination of the NavMeshAgent to the player's position
                animator.SetBool("Walk", true); // Set the animator's "isMoving" parameter to true
            }
            else if (distanceToPlayer <= navMesh.stoppingDistance && !isDead)
            {
                //Enemy attacks the player
                navMesh.isStopped = true; // Stop the NavMeshAgent
                animator.SetBool("Walk", false); // Set the animator's "isMoving" parameter to false
                AttackPlayer();
            }
            else if (!isDead)
            {
                navMesh.SetDestination(returnPoint.position); // Set the destination to the return point
                animator.SetBool("Walk", true); // Set the animator's "isMoving" parameter to true
                navMesh.isStopped = false; // Resume the NavMeshAgent
            }
        }
    }

    void AttackPlayer()
    {
        // Check if the cooldown period has passed
        if (Time.time >= lastAttackTime + attackCooldown && !player.GetComponentInChildren<Animator>().GetBool("Block"))
        {
            // Gets a random number between 0 and 2 that decides the attack animation
            int attackType = Random.Range(0, 3); // Randomly choose an attack type (0 or 1 or 2)

            animator.SetInteger("atNum", attackType); // Set the attack type in the animator
            // Set the animator's "isAttacking" parameter to true
            animator.SetTrigger("Attack");
            // Deal damage to the player (assuming the player has a PlayerHealth script)
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.isDead) // Check if playerHealth is not null and player is not dead
            {
                playerHealth.currentHealth -= attackDamage; // Reduce the player's health
            }
            // Update the last attack time
            lastAttackTime = Time.time;
        }
    }

    void TakeDamage(int damage)
    {
        // Reduce the enemy's health by the damage amount
        health -= damage;
        hasTakenDamage = true; // Set the flag to true
        Debug.Log("Enemy took damage! Remaining health: " + health);
        // Check if the enemy is dead
        if (health <= 0 && !isDead)
        {
            Die();
        }
        //trigger the damage animation
        animator.SetTrigger("Hit");
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        GetComponent<Collider>().enabled = false;
        this.enabled = false; // Disable this script
        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the enemy collides with the player's attack
        if (other.CompareTag("Arma") && !hasTakenDamage && player.GetComponentInChildren<Animator>().GetInteger("Hit") > 0 && hasTakenDamage == false)
        {
            // Call the TakeDamage method with the damage value from the player's weapon manager
            TakeDamage(damage); // Call the TakeDamage method with the damage value from the player's weapon manager
        }
        // Reset the damage flag after a short delay
        Invoke("ResetDamageFlag", 0.5f); // Reset the damage flag after a short delay
    }

    private void ResetDamageFlag()
    {
        hasTakenDamage = false;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage; // Update the damage value
    }
}