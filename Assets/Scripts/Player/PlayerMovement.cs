using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;
using Unity.Mathematics;

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
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        walkSpeed = moveSpeed;
        sprintSpeed = 7f;
        animator.SetBool("GreatSword", true);
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        animator.SetBool("Jump", !grounded);

        MyInput();
        SpeedControl();
        StateHandler();
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //Recibe la velocidad en x y z como absoutos en un vector2
        Vector2 input = new Vector2(horizontalInput, verticalInput);
        Animate(input);
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
            animator.SetBool("Sprint", true);
            moveSpeed = sprintSpeed;
        }
        if ((Input.GetKeyUp(KeyCode.LeftShift) || (gamepad != null && gamepad.leftStickButton.wasReleasedThisFrame)) && grounded && isSprinting)
        {
            isSprinting = false;
            animator.SetBool("Sprint", false);
            moveSpeed = walkSpeed;
        }
        if (Input.GetKeyDown(KeyCode.F) && grounded && moveSpeed == 0)
        {
 
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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

        //blocks movement while blocking and attacking
        if (animator.GetBool("Block") || animator.GetInteger("Hit") > 0)
        {
            moveDirection = Vector3.zero;
        }

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

    private void Animate(Vector2 input)
    {
        //suaviza el paso de un valor a otro
        float targetHorizonal = Mathf.Lerp(animator.GetFloat("Direction"), horizontalInput, Time.deltaTime * 10f);
        float targetVertical = Mathf.Lerp(animator.GetFloat("Speed"), verticalInput, Time.deltaTime * 10f);

        animator.SetFloat("Direction", targetHorizonal);
        animator.SetFloat("Speed", targetVertical);
    }
}