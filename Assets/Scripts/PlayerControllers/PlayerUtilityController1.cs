using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilityController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float speedRotate = 100f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private Transform cam;

    public LayerMask whatIsGround;
    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform groundCheck3;
    public ParticleSystem lightPart;
    public ParticleSystem shockWavePart;
    public ParticleSystem lightningPart;


    public float groundCheckRadius = 0.0f;
    public float stunCooldown = 0f;
    public float stunRadius = 0.0f;
    public float rotSpeed;

    private float lastStunTime;


    // public float groundCheckDistance;

    [SerializeField] private bool isGrounded = false;
    private bool canJump = false;
    [SerializeField] private bool isStunReady = true;

    private Vector3 direction;
    private Vector3 moveDirection;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // This was a temporary movement for my cube, will add stuff to the controller later.
        CheckIfCanJump();
        if (CheckIsStunReady())
        {
            isStunReady = true;
        }
        CheckInput();
        // ChangeRotationToZero();
    }

    private void FixedUpdate()
    {
        CheckSurrounding();
    }

    #region Get Input Function
    // Get keyboard input from the user
    private void CheckInput()
    {
        

        // Get input for ijkl 
        float moveX = 0;
        float moveZ = 0;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        if (Input.GetKey(KeyCode.I))
        {
            moveX = 1f;
        }
        if (Input.GetKey(KeyCode.J))
        {
            moveZ = -1f;
        }
        if (Input.GetKey(KeyCode.K))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.L))
        {
            moveZ = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Vector3 jump = new Vector3(0f, 1f, 0f);
            rb.AddForce(jump * 10f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.O) && isStunReady)
        {
            // Play necessary particle effects
            lightningPart.Play();
            lightPart.Play();
            shockWavePart.Play();

            // Check who is in radius
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                // Stun enemies here 
                if (distance <= stunRadius)
                {
                    Debug.Log(enemy.name + " is a distance of " + distance);
                }

            }

            lastStunTime = Time.time;
            isStunReady = false;
        }

        // Normalize movement so diagonal movement isn't faster
        Vector3 forwardRelativeVertInput = moveX * camForward;
        Vector3 rightRelativeVertInput = moveZ * camRight;

        moveDirection = forwardRelativeVertInput + rightRelativeVertInput;

        moveDirection.Normalize();

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        

        if (moveDirection != Vector3.zero)
        {
            //Debug.Log("I am rotating because " + moveDirection + " is not zero");
            //Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
            // transform.rotation = Quaternion.LookRotation(moveDirection);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            Debug.Log("toRotation = " + toRotation);
            Debug.Log("currentRotation before = " + transform.rotation);
            transform.rotation = toRotation;
            Debug.Log("currentRotation after = " + transform.rotation);
            //transform.forward = moveDirection;
        }

        // Rotate the player

        
        
    }
    #endregion

    #region Check If Player Can Jump Functions
    // Check if player can jump based off is Grounded
    private void CheckIfCanJump()
    {
        if (isGrounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckSurrounding()
    {
        if (CheckIfGrounded())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void ChangeRotationToZero()
    {
        if (CheckForRotation())
        {
            transform.rotation = Quaternion.identity;
        }
    }

    private bool CheckForRotation()
    {
        if (transform.rotation != Quaternion.identity)
        {
            return true; // Return true if rotated off 0,0,0 
        }

        return false;
    }

    // Return whether the player is grounded if at least one of the legs reside on the ground
    private bool CheckIfGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck1.position, -transform.up, out hit, groundCheckRadius) ||
               Physics.Raycast(groundCheck2.position, -transform.up, out hit, groundCheckRadius) ||
               Physics.Raycast(groundCheck3.position, -transform.up, out hit, groundCheckRadius);
    }

    private bool CheckIsStunReady()
    {
        if ((Time.time - lastStunTime) < stunCooldown)
        {
            return false;
        }

        return true;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    #endregion
}
