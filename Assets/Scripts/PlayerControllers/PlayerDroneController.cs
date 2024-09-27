using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        // Raycasts
        OutlineEnemy();
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

    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;


    // Code referenced by https://github.com/DA-LAB-Tutorials/YouTube-Unity-Tutorials/blob/main/OutlineSelection.cs
    // However, this likely needs a rework in the future if time permits one
    void OutlineEnemy()
    {
        if (highlight != null)
        {
            // Reset the highlight outline if we are no longer hovering over it
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            Transform hitTransform = raycastHit.transform;

            if (hitTransform.CompareTag("Enemy") && hitTransform != selection)
            {
                highlight = hitTransform;
                var outline = highlight.gameObject.GetComponent<Outline>();

                if (outline != null)
                {
                    // outline.enabled = true; im now scrapping outlines due to inconsistency
                    outline.OutlineColor = Color.blue;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                // Select the current enemy (highlighted object)
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }

                selection = highlight;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                selection.gameObject.GetComponent<Outline>().OutlineColor = Color.yellow;

                // Clear the highlight as it has now become a selection
                highlight = null;
            }
            else if (selection != null)
            {
                // Deselect the current selection if clicking outside an enemy
                selection.gameObject.GetComponent<Outline>().OutlineColor = Color.blue;
                selection.gameObject.GetComponent<Outline>().enabled = false;
                selection = null;
            }
        }
    }
}
