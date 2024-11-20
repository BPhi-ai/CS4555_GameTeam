using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    [SerializeField] private Material material;
    private Color defaultColor;

    public Obstacles obstacles;

    private void Awake()
    {
        defaultColor = new Color(0.07843135f, 0.7529412f, 1f, 1f);
        material.SetColor("_Color", defaultColor);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Contact");
            material.SetColor("_Color", Color.white);

            obstacles.Explode();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Leaving Button");
            material.SetColor("_Color", defaultColor);
        }
    }
}
