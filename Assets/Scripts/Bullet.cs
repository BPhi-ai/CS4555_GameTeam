using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f;
    public long damage = 10;

    public bool fromEnemy = true;

    public float timeToDelete = 5.0f;
    private float timePassed = 0.0f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timePassed += Time.deltaTime;
        if (timePassed >= timeToDelete)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && fromEnemy)
        {
            Entity entity = other.GetComponent<Entity>();
            entity.DamageEntity(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy") && !fromEnemy)
        {
            Entity entity = other.GetComponent<Entity>();
            entity.DamageEntity(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
