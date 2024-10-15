using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShooterMovement : MonoBehaviour
{
    private Rigidbody rb;
    Animator anim;
    [SerializeField] private float speed = 5f;

    public Camera droneCamera;

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    
    void Update()
    {
        CheckInput();
        /*
        if (!moving)
        {
            anim.SetTrigger("Idle");
        }*/

        if (!RotateTowardsMarkedEnemies()) // If you C
        {
            LookAtMouse();
        }
    }

    private void CheckInput()
    {
        float moveX = 0;
        float moveZ = 0;

        Vector3 camForward = droneCamera.transform.forward;
        Vector3 camRight = droneCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        bool moving = false;
        if (Input.GetKey("w"))
        {
            moving = true;
            anim.SetBool("Run", true);
            moveX = 1f;
        }
        if (Input.GetKey("a"))
        {
            moving = true;
            anim.SetBool("Run", true);
            moveZ = -1f;
        }
        if (Input.GetKey("s"))
        {
            moving = true;
            anim.SetBool("Run", true);
            moveX = -1f;
        }
        if (Input.GetKey("d"))
        {
            moving = true;
            anim.SetBool("Run", true);
            moveZ = 1f;
        }

        // Normalize movement so diagonal movement isn't faster
        Vector3 forwardRelativeVertInput = moveX * camForward;
        Vector3 rightRelativeVertInput = moveZ * camRight;

        Vector3 cameraRelativeMovement = forwardRelativeVertInput + rightRelativeVertInput;
        cameraRelativeMovement = cameraRelativeMovement.normalized;

        transform.position += cameraRelativeMovement * speed * Time.deltaTime;

    }

    bool RotateTowardsMarkedEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in allEnemies)
        {
            if (enemy.GetComponent<Outline>().enabled)
            {
                //print("Marked enemy found");
                Vector3 direction = enemy.transform.position - transform.position;
                direction.y = 0;
                float turnAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, -turnAngle, rb.velocity.magnitude * Time.deltaTime);
                GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, smoothAngle, transform.eulerAngles.z));
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

            Vector3 direction = worldMousePosition - transform.position;
            direction.y = 0;

            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
