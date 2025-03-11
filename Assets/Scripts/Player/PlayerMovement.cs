using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float wallrunSpeed;
    public bool isWallrunning;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool isDoubleJump;
    public bool isSprinting;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        walkSpeed = moveSpeed;
        sprintSpeed = 7f;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        Debug.Log(grounded);

        MyInput();
        SpeedControl();
        StateHandler();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        var gamepad = Gamepad.current;

        if ((Input.GetKeyDown(KeyCode.Space) || (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame)) && readyToJump && (grounded || isDoubleJump || isWallrunning))
        {
            isDoubleJump = false;
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || (gamepad != null && gamepad.leftStickButton.wasPressedThisFrame)) && grounded && !isSprinting)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
        }
        if ((Input.GetKeyUp(KeyCode.LeftShift) || (gamepad != null && gamepad.leftStickButton.wasReleasedThisFrame)) && grounded && isSprinting)
        {
            isSprinting = false;
            moveSpeed = walkSpeed;
        }
    }

    private void StateHandler()
    {
        if (isWallrunning)
        {
            moveSpeed = wallrunSpeed;
        }
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        if (!grounded)
        {
            rb.linearDamping = 0;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}