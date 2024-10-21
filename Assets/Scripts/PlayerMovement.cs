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

    [Header("Audio Source")]
    public AudioSource moveAudioSource;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;

    bool grounded;

    [Header("Orientation")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 lastMoveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // I don't remember why I did this. :(
        readyToJump = true; // You won't be able to jump if this isn't added in the Start method. Idk why. 
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Check if the user is on the ground

        MyInput();

        SpeedControl();

        // Gives drag when grounded and updates last move direction 
        if (grounded)
        {
            rb.drag = groundDrag;
            lastMoveDirection = Vector3.zero; // Reset last move direction when grounded
        }
        else
        {
            rb.drag = 0;
        }

        // Play or stop audio based on movement. This is mostly broken as it stops the audio abrubtly as opposed to letting the clip finish then stopping after jumping. 
        if (grounded && (horizontalInput != 0 || verticalInput != 0)) // Is player grounded and moving. This needs to be updated as it will still produce footsteps while standing still if stuck on a wall for example. 
        {
            if (!moveAudioSource.isPlaying)
            {
                moveAudioSource.Play();
            }
        }
        else // Stop audio when not moving or jumping
        {
            if (moveAudioSource.isPlaying)
            {
                moveAudioSource.Stop();
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

        if (Input.GetKey(jumpKey) && readyToJump && grounded) // Is user pressing jump, readyToJump, and on the ground. 
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
                isSprinting = false;
            }

            // Update last move direction when grounded
            if (horizontalInput != 0 || verticalInput != 0)
            {
                lastMoveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
            }
        }
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
            Vector3 airMomentum = lastMoveDirection * (isSprinting ? sprintSpeed : moveSpeed) * 0.6f;

            // Allow some control in all directions
            Vector3 inputMomentum = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized * (isSprinting ? sprintSpeed * 0.4f : moveSpeed * 0.3f);

            moveDirection = airMomentum + inputMomentum;
        }

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed; // Use sprint speed if sprinting

        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force); // Add force to player to actually move them. 
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
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Set the velocity of the Rigid Body
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Add upward force to the Rigid Body
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
