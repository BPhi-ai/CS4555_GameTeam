using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public string entityName;

    GameObject entity;
    public void Start()
    {
        entity = GameObject.Find(entityName);
        SetMaxHealth();
    }

    public void SetMaxHealth()
    {
        slider.maxValue = entity.GetComponent<Entity>().maxHealth;
    }

    public void SetHealth(long val)
    {
        slider.value = val;
    }
}
