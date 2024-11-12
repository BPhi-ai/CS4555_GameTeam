using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public long health;
    public long maxHealth;
    public float regenRate = 10f;

    public void Start()
    {
        health = maxHealth;
    }

    public void StartEntityRegen()
    {
        regenRate = 10f;
    }

    public void StopEntityRegen()
    {
        regenRate = 0f;
    }

    public void HealEntity()
    {
        if (health < maxHealth)
        {
            health += (long)regenRate;
        }
    }

    public void DamageEntity(long dmgVal)
    {
        health -= dmgVal;
    }
}
