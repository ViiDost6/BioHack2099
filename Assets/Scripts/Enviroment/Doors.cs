using UnityEngine;

public class Doors : MonoBehaviour
{
    // Defines the behavior for the scifi door, which moves up and down when opened or closed.
    // The door will move up and down based on the doorTransform's position.
    public GameObject door; // Reference to the door GameObject
    private Transform doorTransform; // Transform component of the door
    private Collider doorCollider; // Collider component of the door
    public float speed = 1.0f; // Speed of the door opening/closing
    public bool canDoorBeOpened = true; // Flag to check if the door can be opened

    private void Start()
    {
        doorTransform = door.transform; // Get the Transform component of the door
        doorCollider = door.GetComponent<BoxCollider>(); // Get the Collider component of the door
        if (doorCollider == null)
        {
            Debug.LogError("No collider found on the door object.");
        }
    }

    // Called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        if (canDoorBeOpened && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger.");
            // Open the door by moving it up
            doorTransform.position += speed * Time.deltaTime * Vector3.up;
        }
    }

    // Called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit(Collider other)
    {
        if (canDoorBeOpened && other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger.");
            // Close the door by moving it down
            doorTransform.position -= speed * Time.deltaTime * Vector3.up;
        }
    }
}
