using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneController : MonoBehaviour
{
    // Reference: https://www.youtube.com/watch?v=rxa4N4z65pg

    public float sensitivity;
    public float speed;

    void Update()
    {
        if (Input.GetMouseButton(1)) // Holding right click
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Movement();
            Rotation();
        }
        else
        {
            Movement();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Rotation()
    {
        Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        float pitch = eulerRotation.x;
        if (pitch > 180) pitch -= 360;

        pitch += mouseInput.x * sensitivity * Time.deltaTime * 50;
        float yaw = eulerRotation.y + mouseInput.y * sensitivity * Time.deltaTime * 50;

        pitch = Mathf.Clamp(pitch, -80f, 80f); // Limits the up/down rotation

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void Movement()
    {
        float horizontalInput = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput += 1;
        }

        float verticalInput = 0;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput += 1;
        }

        Vector3 input = new Vector3(horizontalInput, 0f, verticalInput);

        transform.Translate(input * speed * Time.deltaTime);
    }
}
