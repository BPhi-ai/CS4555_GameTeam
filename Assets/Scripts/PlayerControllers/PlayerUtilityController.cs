using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilityController : MonoBehaviour
{
    private Rigidbody rb; 
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float speedRotate = 100f;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent <Rigidbody>();
    }

    void Update()
    {
        /* This was a temporary movement for my cube, will add stuff to the controller later.
        if (Input.GetKey("i"))
        {
            Vector3 movement = new Vector3(0f, 0f, 1f * Time.deltaTime * speed);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey("j"))
        {
            Vector3 movement = new Vector3(-1f * Time.deltaTime * speed, 0, 0);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey("k"))
        {
            Vector3 movement = new Vector3(0f, 0f, -1f * Time.deltaTime * speed);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey("l"))
        {
            Vector3 movement = new Vector3(1f * Time.deltaTime * speed, 0, 0);
            movement = transform.TransformDirection(movement);
            transform.position += movement;
        }
        if (Input.GetKey("space"))
        {
            Debug.Log("Jumped");
        }
        */
    }
}
