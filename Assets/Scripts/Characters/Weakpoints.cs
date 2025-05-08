using UnityEngine;

public class Weakpoints : MonoBehaviour
{
    //Gets a collider and a rigidbody
    public GameObject player;
    public Collider weakpointCollider;
    public GameObject weaponManager;
    public GameObject enemy;
    private int currentWeaponIndex = 0;

    void Start()
    {
        weakpointCollider = GetComponent<Collider>();
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex; // Gets the current weapon index from the player
    }

    void Update()
    {
        //Checks if the player is using the weakpoint weapon
        currentWeaponIndex = weaponManager.GetComponent<WeaponManager>().currentWeaponIndex;
        if (currentWeaponIndex == 1)
        {
            weakpointCollider.enabled = true; // Enable the weakpoint collider
        }
        else
        {
            weakpointCollider.enabled = false; // Disable the weakpoint collider
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (currentWeaponIndex == 1 && player.GetComponentInChildren<Animator>().GetInteger("Hit") >= 1) // Checks if the player is using the weakpoint weapon and is attacking
        {
            enemy.GetComponent<Enemy>().health = 0; // Set health to 0 if weakpoint is hit
            //Enemy sript calls the Die method to handle enemy death
        }
    }
}
