using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum States {ALIVE, DEAD}
    public States state = States.ALIVE;

    public long health;
    public long maxHealth;
    public float regenRate = 10f;
    public float healRadius = 7.0f;

    

    public Healthbar healthBar;

    public void Start()
    {
        healthBar = GetComponentInChildren<Healthbar>();
        healthBar.SetMaxHealth(maxHealth);
        health = maxHealth;
    }

    #region Regen Health Section
    public void StartEntityRegen()
    {
        regenRate = 10f;
    }

    public void StopEntityRegen()
    {
        regenRate = 0f;
    }

    public void CheckHeal(string goName)
    {
        GameObject other = GameObject.Find(goName);
        float distance = Vector3.Distance(
            transform.position,
            other.transform.position
        );

        if (distance < healRadius)
        {
            HealEntity();
            Entity otherEntity = other.GetComponent<Entity>();
            otherEntity.HealEntity();
            healthBar.SetHealth(health);
            other.GetComponentInChildren<Healthbar>().SetHealth(otherEntity.health);
        }
    }

    public void CheckAliveStatus()
    {
        if (health <= 0f)
        {
            state = States.DEAD;
        }
    }

    public void HealEntity()
    {
        if (health < maxHealth)
        {
            health += (long)regenRate;
        }
    }
    #endregion

    #region Damage Entity
    public void DamageEntity(long dmgVal)
    {
        health -= dmgVal;
        healthBar.SetHealth(health);
    }
    #endregion
}
