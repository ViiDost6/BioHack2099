using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallrunForce, maxWallrunTime, maxWallrunTimer;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWall, rightWall;
    private bool isWallRight, isWallLeft;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;

    //Input variables
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;


    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.isWallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, out rightWall, wallCheckDistance, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWall, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //State 1 - Wallrunning
        if ((isWallLeft || isWallRight) && verticalInput > 0 && AboveGround())
        {
            if (pm.isWallrunning)
            {
                WallRunningMovement();
                StartWallrun();
            }
            else
            {
                if (pm.isWallrunning)
                {
                    StopWallrun();
                }
            }
        }
    }

    private void StartWallrun()
    {
        pm.isWallrunning = true;   
    }
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        Vector3 wallNormal = isWallRight ? rightWall.normal : leftWall.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //forward force
        rb.AddForce(wallForward * wallrunForce, ForceMode.Force);

        //push wall force
        if (!(isWallLeft && horizontalInput > 0) && !(isWallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }
    private void StopWallrun()
    {
        pm.isWallrunning = false;
        rb.useGravity = true;
    }
}
