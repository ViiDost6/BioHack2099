using UnityEngine;

public class AimCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 

    private void Update()
    {
        //camera's position must be 0.5 more to the right than the player's position
        //camera's position must be 0.5 more to the top than the player's position
        Vector3 viewDir = player.position - new Vector3(transform.position.x + 0.5f, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput   = Input.GetAxis("Horizontal");
        float verticalInput     = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    } 
}
