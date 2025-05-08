using UnityEditor.SearchService;
using UnityEngine;

public class FlyingEnemyVision : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public GameObject enemy; // Reference to the enemy object
    private Vector3 forceDirection; // Direction of the force applied to the bullet
    private float attackTimer = 0f; // Timer for attack cooldown
    private Transform objetivo; // Target transform for the enemy to follow
    private bool isFollowing = false; // Flag to check if the enemy is following the player

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //if the player enters the trigger, the drone will follow them until it dies
            objetivo = other.transform;
            isFollowing = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemy.GetComponent<FlyingEnemy>().attackCooldown <= attackTimer)
            {
                // If the player is within attack distance and the cooldown is over, shoot the bullet
                GameObject bulletInstance = Instantiate(enemy.GetComponent<FlyingEnemy>().bullet, enemy.GetComponent<FlyingEnemy>().emissionPoint.position, Quaternion.identity);
                Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();

                // Apply force to the bullet
                bulletRb.AddForce(forceDirection * bulletInstance.GetComponent<EnemyBullet>().speed, ForceMode.Impulse);

                // Destroy the bullet after its lifetime
                Destroy(bulletInstance, bulletInstance.GetComponent<EnemyBullet>().lifeTime);
                attackTimer = 0f; // Reset the attack timer after shooting
            }
            else
            {
                attackTimer += Time.deltaTime; // Increment the attack timer
            }
        }
    }
    void Update()
    {
        forceDirection = (player.transform.position - enemy.GetComponent<FlyingEnemy>().emissionPoint.position).normalized;
        // Update the force direction to always point towards the player
        //also makes the drown look at the player
        Vector3 lookDirection = player.transform.position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(lookDirection);

        // Move the enemy towards the player if it is following
        if (isFollowing)
        {
            // Move the enemy towards the player
            Vector3 direction = (objetivo.position - enemy.transform.position).normalized;
            enemy.transform.position += direction * enemy.GetComponent<FlyingEnemy>().speed * Time.deltaTime;

            // Check if the enemy is within stopping distance from the player
            if (Vector3.Distance(enemy.transform.position, objetivo.position) < enemy.GetComponent<FlyingEnemy>().stoppingDistance)
            {
                // Stop moving and start attacking
                isFollowing = false;
            }
        }
    }
}
