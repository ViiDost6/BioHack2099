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
    public float speed = 2.0f; // Speed of the enemy
    public float attackRange = 1.0f; // Range within which the enemy can attack the player
    public float attackDamage = 10.0f; // Damage dealt to the player on attack
    public float health = 100.0f; // Health of the enemy
    public float attackCooldown = 3.0f; // Time between attacks
    private float lastAttackTime = 0.0f; // Time of the last attack
    public Animator animator; // Reference to the enemy's animator component
    private PlayerHealth playerHealth; // Reference to the player's health component
    private bool isDead = false; // Flag to check if the enemy is dead
    private bool hasTakenDamage = false; // Flag to check if the enemy has taken damage
    private int damage = 50;

    void Start()
    {
        // Get the animator component attached to this enemy
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag
        playerHealth = player.GetComponent<PlayerHealth>(); // Get the PlayerHealth component from the player object

        //tells navmesh where to go
        navMesh.SetDestination(player.transform.position); // Set the destination of the NavMeshAgent to the player's position
    }


    void Update()
    {
        navMesh.destination = player.transform.position; // Update the destination of the NavMeshAgent to the player's position
        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= 10.0f && distanceToPlayer > navMesh.stoppingDistance && !isDead)
        {
            animator.SetTrigger("playerInRange");
            navMesh.isStopped = false; // Resume the NavMeshAgent
            animator.SetBool("Walk", true); // Set the animator's "isMoving" parameter to true
        }
        else if (distanceToPlayer <= navMesh.stoppingDistance && !isDead)
        {
            //Enemy attacks the player
            animator.SetBool("Walk", false); // Set the animator's "isMoving" parameter to false
            AttackPlayer();
        }
        else
        {
            navMesh.isStopped = true; // Stop the NavMeshAgent
            animator.SetBool("Walk", false); // Set the animator's "isMoving" parameter to false
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
        if (other.CompareTag("Arma") && !hasTakenDamage && player.GetComponentInChildren<Animator>().GetInteger("Hit") > 0)
        {
            // Get the damage value from the player's weapon manager
            WeaponManager weaponManager = player.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                damage = weaponManager.damage; // Get the damage value from the player's weapon manager
            }

            // Call the TakeDamage method with the damage value from the player's weapon manager
            TakeDamage(damage); // Call the TakeDamage method with the damage value from the player's weapon manager
        }
    }

    private void ResetDamageFlag()
    {
        hasTakenDamage = false;
    }
}
