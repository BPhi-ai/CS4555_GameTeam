using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShooterMovement : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float speed = 5f;
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
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if(Input.GetKey("a"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(-1f * Time.deltaTime * speed, 0, 0);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if(Input.GetKey("s"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(0f, 0f, -1f * Time.deltaTime * speed);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if(Input.GetKey("d"))
        {
            anim.SetTrigger("Run");
            Vector3 movement = new Vector3(1f * Time.deltaTime * speed, 0, 0);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
    }
}
