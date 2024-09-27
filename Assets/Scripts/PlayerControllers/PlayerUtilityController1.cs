using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilityController : MonoBehaviour
{
    private Rigidbody rb; 
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float speedRotate = 100f;

    public LayerMask whatIsGround;
    public Transform groundCheck;

    public float groundCheckRadius = 0.0f;

    // public float groundCheckDistance;

    [SerializeField] private bool isGrounded = false;
    private bool canJump = false;

    private Vector3 direction;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent <Rigidbody>();
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

    private void CheckInput()
    {
        
        if (Input.GetKey(KeyCode.I))
        {
            Vector3 movement = new Vector3(0f, 0f, 1f * Time.deltaTime * speed);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey(KeyCode.J))
        {
            Vector3 movement = new Vector3(-1f * Time.deltaTime * speed, 0, 0);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey(KeyCode.K))
        {
            Vector3 movement = new Vector3(0f, 0f, -1f * Time.deltaTime * speed);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey(KeyCode.L))
        {
            Vector3 movement = new Vector3(1f * Time.deltaTime * speed, 0, 0);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            Vector3 jump = new Vector3(0f, 1f, 0f);
            rb.AddForce(jump * 0.3f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.O))
        {
            Debug.Log("Shoot Stun Projectile");
        }
    }

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

    private bool CheckIfGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck.position, -transform.up, out hit, groundCheckRadius);
    }
}
