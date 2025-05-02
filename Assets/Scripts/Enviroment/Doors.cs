using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour
{
    private Collider doorCollider; // Collider component of the door
    private Transform doorTransform; // Transform component of the door
    public float speed = 0.5f; // Speed of the door opening/closing in seconds
    public float openHeight = 4.0f; // Height to which the door opens
    public bool canDoorBeOpened = true; // Flag to check if the door can be opened

    private Vector3 closedPosition; // Original position of the door
    private Vector3 openPosition; // Target position when the door is open
    private Coroutine doorCoroutine; // Reference to the active coroutine

    private void Start()
    {
        doorTransform = this.transform; // Get the Transform component of the door
        doorCollider = this.GetComponent<BoxCollider>(); // Get the Collider component of the door
        if (doorCollider == null)
        {
            Debug.LogError("No collider found on the door object.");
        }

        // Store the closed and open positions
        closedPosition = doorTransform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDoorBeOpened && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger.");
            // Start opening the door
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(openPosition));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (canDoorBeOpened && other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger.");
            // Start closing the door
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(MoveDoor(closedPosition));
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        Vector3 startPosition = doorTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < speed)
        {
            doorTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the door reaches the exact target position
        doorTransform.position = targetPosition;
    }
}
