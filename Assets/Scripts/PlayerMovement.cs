using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool isSprinting;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Audio")]
    public AudioSource moveAudioSource;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 lastMoveDirection; // Track last direction of movement

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
            lastMoveDirection = Vector3.zero; // Reset last move direction when grounded
        }
        else
        {
            rb.drag = 0;
        }

        // Play or stop audio based on movement
        if (grounded && (horizontalInput != 0 || verticalInput != 0))
        {
            if (!moveAudioSource.isPlaying)
            {
                moveAudioSource.Play(); // Play audio when moving
            }
        }
        else
        {
            if (moveAudioSource.isPlaying)
            {
                moveAudioSource.Stop(); // Stop audio when not moving or jumping
            }
        }
    }



    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Check for sprinting only when grounded
        if (grounded)
        {
            if (Input.GetKey(sprintKey))
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false; // Reset sprinting if key is not pressed
            }

            // Update last move direction when grounded
            if (horizontalInput != 0 || verticalInput != 0)
            {
                lastMoveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
            }
        }
        // In the air, isSprinting remains unchanged
    }

    private void MovePlayer()
    {
        // Set the move direction based on whether the player is grounded or in the air
        if (grounded)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // Update last move direction when grounded
            if (horizontalInput != 0 || verticalInput != 0)
            {
                lastMoveDirection = moveDirection.normalized;
            }
        }
        else
        {
            // In the air: blend last move direction with current input
            float airControlFactor = isSprinting ? 0.5f : 0.7f; // Adjust these values to control air movement

            // Maintain momentum from last direction
            Vector3 airMomentum = lastMoveDirection * (isSprinting ? sprintSpeed : moveSpeed) * 0.6f; // Higher weight for last direction

            // Allow some control in all directions
            Vector3 inputMomentum = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized * (isSprinting ? sprintSpeed * 0.4f : moveSpeed * 0.3f); // Less weight for new input

            moveDirection = airMomentum + inputMomentum;
        }

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed; // Use sprint speed if sprinting

        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
