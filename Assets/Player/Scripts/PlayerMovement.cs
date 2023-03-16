using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerHeight = 2f;

    [SerializeField] Transform Orientation;

    [Header("Key Bindings")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Jump")]
    public float jumpForce = 15f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    public float airMultiplier = 0.4f;

    float horizontalMovement;
    float verticalMovement;

    bool isGrounded;
    float groundDistance = 0.4f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Drag")]
    float groundDrag = 6f;
    float airDrag = 2f;

    Vector3 moveDirection;
    Vector3 slopeMovementDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else{
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        MyInput();
        ControlDrag();
        ControlSpeed();

        //jump code below
        if(Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jump();
        }

        slopeMovementDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = Orientation.forward * verticalMovement + Orientation.right * horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if(isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMovementDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
