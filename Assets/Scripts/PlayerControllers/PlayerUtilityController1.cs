using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilityController : MonoBehaviour
{
    private Rigidbody rb; 
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float speedRotate = 100f;

    public LayerMask whatIsGround;
    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform groundCheck3;
    public ParticleSystem part;

    public float groundCheckRadius = 0.0f;

    // public float groundCheckDistance;

    [SerializeField] private bool isGrounded = false;
    private bool canJump = false;

    private Vector3 direction;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        // This was a temporary movement for my cube, will add stuff to the controller later.
        CheckIfCanJump();
        CheckInput();
    }

    private void FixedUpdate()
    {
        CheckSurrounding();
    }

    #region Get Input Function
    // Get keyboard input from the user
    private void CheckInput()
    {

        float moveX = 0;
        float moveZ = 0;
    
        if (Input.GetKey(KeyCode.I))
        {
            moveZ = 1f;
        }
        if (Input.GetKey(KeyCode.J))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.K))
        {
            moveZ = -1f;
        }
        if (Input.GetKey(KeyCode.L))
        {
            moveX = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Vector3 jump = new Vector3(0f, 1f, 0f);
            rb.AddForce(jump * 10f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.O))
        {
            Debug.Log("Shoot Stun Projectile/Stun Explosion");
            part.Play();
        }

        // Normalize movement so diagonal movement isn't faster
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        movement = transform.TransformDirection(movement);
        transform.position += movement * speed * Time.deltaTime;
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

    // Return whether the player is grounded if at least one of the legs reside on the ground
    private bool CheckIfGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck1.position, -transform.up, out hit, groundCheckRadius) ||
               Physics.Raycast(groundCheck2.position, -transform.up, out hit, groundCheckRadius) ||
               Physics.Raycast(groundCheck3.position, -transform.up, out hit, groundCheckRadius);
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    #endregion
}
