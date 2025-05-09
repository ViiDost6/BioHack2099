using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed = 3.5f;
    public float stoppingDistance = 3.5f;
    public float retreatDistance = 2f;
    public float attackDistance = 3.5f;
    public float attackCooldown = 2f;
    public float health = 100f;
    public float damage = 10f;
    public GameObject bullet; // Reference to the bullet prefab
    public Transform emissionPoint; // Reference to the point where the bullet will be instantiated

    //This funtion animates the enemy making it seem to fly

    private void Start()
    {
        // Initialize the enemy's position and rotation if needed
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z); // Set initial height
        transform.rotation = Quaternion.Euler(0, 0, 0); // Set initial rotation
    }

    public void Update()
    {
        AnimateFlying();
    }
    private void AnimateFlying()
    {
        //Varies y position to simulate flying
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.Sin(Time.time * speed) * 0.5f + 2.0f; // Adjust the amplitude and offset as needed
        //Lerps the position to make it look smoother
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arma"))
        {
            float damage = player.GetComponentInChildren<WeaponManager>().damage; // Assuming the weapon has a damage property
            health -= damage * 5; // Reduce enemy health by the weapon damage
            Debug.Log("Enemy hit! Health: " + health); // Log the enemy's health for debugging
            if (health <= 0)
            {
                Die(); // Call the die method
            }
        }
    }

    private void Die()
    {
        // Handle enemy death (e.g., play animation, destroy object, etc.)
        Destroy(gameObject); // Destroy the enemy object
    }
}
