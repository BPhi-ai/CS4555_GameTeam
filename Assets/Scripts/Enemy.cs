using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum States {IDLE, WANDER, STUNNED, CHASE}
    public States state = States.IDLE;

    public float maxRange = 10000;
    public GameObject closestPlayer = null;

    RaycastHit hit;

    void FixedUpdate()
    {
        switch (state)
        {
            case States.IDLE:
                if (SeesPlayer()) {
                    state = States.CHASE;
                    print("I see the player!");
                    break;
                }
                break;
            case States.WANDER:
                break;
            case States.STUNNED:
                break;
            case States.CHASE:
                if (!SeesPlayer())
                {
                    state = States.IDLE;
                    print("Can't see the player anymore!");
                    break;
                }
                break;
        }
    }

    public bool SeesPlayer()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        float offsetY = 1;

        foreach (GameObject player in allPlayers)
        {
            Vector3 playerPosition = player.transform.position;
            playerPosition.y += offsetY;
            Debug.DrawRay(transform.position, (playerPosition - transform.position).normalized * maxRange, Color.red);
            if (Vector3.Distance(transform.position, playerPosition) <= maxRange)
            {
                if (Physics.Raycast(transform.position, (playerPosition - transform.position), out hit, maxRange))
                {
                    //print("I hit something called " + hit.collider.name);
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
