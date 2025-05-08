using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //tells the bullet how fast to move
    public float speed = 1f;
    //tells the bullet how long to live
    public float lifeTime = 2f;
    //tells the bullet how much damage to do
    public int damage = 1;

    //tells the bullet the target
    public Vector3 target;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a health component
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.currentHealth -= damage; // Reduce player's health by bullet damage
                Debug.Log("Player hit! Health: " + playerHealth.currentHealth); // Log the player's health for debugging
            }
        }
        Destroy(gameObject); // Destroy the bullet on collision
    }
}
