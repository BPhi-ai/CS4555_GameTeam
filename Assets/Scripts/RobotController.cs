using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerMovementWithAnimation : MonoBehaviour
{
    public GameObject bulletBlock;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float timeFix = 0;


    public float moveSpeed = 5f;      // Speed of movement
    public float rotationSpeed = 720f; // Speed of rotation
    private Vector3 moveDirection;

    public AudioSource audioSource;

    private Animator animator;        // Reference to the Animator component

    public Camera droneCamera;

    public Entity entity;
    public ParticleSystem fire;
    public ParticleSystem ember;
    public ParticleSystem fireLight;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();
    }

    void Update()
    {
        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal");  // A and D keys
        float moveZ = Input.GetAxis("Vertical");    // W and S keys

        // Adding these so that movement is relative to the camera
        Vector3 camForward = droneCamera.transform.forward;
        Vector3 camRight = droneCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        Vector3 vectorMovementX = moveX * camRight;
        Vector3 vectorMovementZ = moveZ * camForward;

        // Calculate the movement direction
        moveDirection = (vectorMovementX + vectorMovementZ).normalized;

        // If there is movement input, move and rotate the character

        MoveAndRotate(); // probably works outside of the below if statement
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);  // Trigger the movement animation
        }
        else
        {
            animator.SetBool("isMoving", false); // Stop the movement animation (idle state)
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKey(KeyCode.O) || Input.GetButton("Fire1"))
        {
            timeFix += Time.deltaTime;
            animator.SetBool("isShooting", true);
            if(timeFix > 0.25)
            {
                // Spawn the bullet at the firePoint's position and rotation
                GameObject bullet = Instantiate(bulletBlock, firePoint.position, firePoint.rotation);
                audioSource.Play();

                // Apply velocity to the bullet's Rigidbody to make it move forward
                // Rigidbody rb = bullet.GetComponent<Rigidbody>();
                // rb.velocity = firePoint.forward * bulletSpeed;
                timeFix = 0;
            }
        } else { 
            animator.SetBool("isShooting", false);
            timeFix = 0; // adding this to ensure the counter is reset so the time is consistent
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }*/

        CheckForDeath();
    }

    IEnumerator Reload()
    {
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("isReloading",false);
    }

    IEnumerator Jump()
    {
        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("isJumping", false);
    }


    // Method to move and rotate the object based on input
    void MoveAndRotate()
    {
        // Move the GameObject
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the GameObject to face the direction of movement
        
        /*
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        */

        if (!RotateTowardsMarkedEnemies())
        {
            LookAtMouse();
        }
    }

    bool RotateTowardsMarkedEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy.GetComponent<Outline>().enabled)
            {
                //print("Marked enemy found");
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                //float turnAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                //float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, -turnAngle, rb.velocity.magnitude * Time.deltaTime);
                //GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, smoothAngle, transform.eulerAngles.z));
                //transform.rotation = Quaternion.LookRotation(direction);
                return true;
            }
        }
        return false;
    }

    void LookAtMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Ray ray = droneCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 worldMousePosition = ray.GetPoint(rayDistance);

            Vector3 direction = (worldMousePosition - transform.position).normalized;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    #region Other Check Functions
    public void CheckForDeath()
    {
        if (entity.state == Entity.States.DEAD)
        {
            fire.Play();
            ember.Play();
            fireLight.Play();

        }
    }
    #endregion

}
