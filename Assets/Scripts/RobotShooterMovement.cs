using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShooterMovement : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float speed = 5f;

    public Camera droneCamera;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKey("w"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(0f, 0f, 1f * Time.deltaTime * speed);
            //movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if(Input.GetKey("a"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(-1f * Time.deltaTime * speed, 0, 0);
            //movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if(Input.GetKey("s"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(0f, 0f, -1f * Time.deltaTime * speed);
            //movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if(Input.GetKey("d"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(1f * Time.deltaTime * speed, 0, 0);
            //movement = transform.TransformDirection(movement);
            transform.position += movement;
        }

        if (!RotateTowardsMarkedEnemies()) // If you can't find a marked enemy to look at, look at the mouse instead
        {
            LookAtMouse();
        }
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
                transform.rotation = Quaternion.LookRotation(direction);
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
