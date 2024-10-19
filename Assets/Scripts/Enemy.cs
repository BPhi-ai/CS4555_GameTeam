using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum States {WANDER, STUNNED, CHASE}
    public States state = States.WANDER;

    public float maxRange = 10000;
    public float speed = 2.5f;
    public GameObject closestPlayer = null;
    public ParticleSystem lightning;

    RaycastHit hit;

    private float timeToWander = 0.0f;

    private Vector3 startPosition;
    public float wanderOffset = 25.0f; // Wandering radius
    public Vector3 randomPosition; // this is the random position to wander to

    private float timeStunned = 0.0f;
    public float timeLightningInterval = 2.0f;
    private float timePassed = 0.0f; // Used for tracking when to play lightning particle

    private void Start()
    {
        startPosition = transform.position;
        randomPosition = startPosition;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case States.WANDER:
                timeToWander += Time.deltaTime;

                // If the enemy sees the player, we switch it to a chase state
                if (SeesPlayer()) {
                    state = States.CHASE;
                    print("I see the player! The player is: " + closestPlayer.name);
                    break;
                }

                // Every 5 seconds, I pick a new position for the enemy to walk to
                if (timeToWander >= 5.0f) 
                {
                    timeToWander = 0.0f; // resets the wandering timer

                    float randomX = Random.Range(-wanderOffset, wanderOffset);
                    float randomZ = Random.Range(-wanderOffset, wanderOffset);

                    randomPosition = new Vector3(startPosition.x + randomX, startPosition.y, startPosition.z + randomZ);
                }

                // Move the player to the random position at all times
                transform.position = Vector3.MoveTowards(transform.position, randomPosition, speed * Time.deltaTime);

                break;
            case States.STUNNED:
                timeStunned += Time.deltaTime;
                
                timePassed += Time.deltaTime;

                if (timePassed >= timeLightningInterval)
                {
                    lightning.Play();
                    timePassed = 0.0f;
                }

                if (timeStunned >= 5.0f)
                {
                    timeStunned = 0.0f;
                    timePassed = 0f;

                    state = States.WANDER;
                }
                break;
            case States.CHASE:
                if (!SeesPlayer())
                {
                    state = States.WANDER;
                    print("Can't see the player anymore!");
                    break;
                }
                break;
        }
    }

    public void Stun()
    {
        state = States.STUNNED;
    }

    public bool SeesPlayer()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayerSoFar = allPlayers[0]; // if this is a source of error, there isnt any players in the scene with this enemy. Added it to compare check for the closest player (needs an initial player to check)
        float offsetY = 1;
        bool foundPlayer = false;



        foreach (GameObject player in allPlayers)
        {
            Vector3 playerPosition = player.transform.position;
            playerPosition.y += offsetY;
            // Debug.DrawRay(transform.position, (playerPosition - transform.position).normalized * maxRange, Color.red);
            if (Vector3.Distance(transform.position, playerPosition) <= maxRange)
            {
                if (Physics.Raycast(transform.position, (playerPosition - transform.position), out hit, maxRange))
                {
                    //print("I hit something called " + hit.collider.name);
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        if (Vector3.Distance(transform.position, playerPosition) < Vector3.Distance(transform.position, closestPlayerSoFar.transform.position))
                        {
                            foundPlayer = true;
                            closestPlayerSoFar = hit.collider.gameObject;
                        }
                    }
                }
            }
        }

        if (!foundPlayer)
        {
            return false;
        }
        else
        {
            closestPlayer = closestPlayerSoFar;
            Debug.DrawRay(transform.position, (closestPlayer.transform.position - transform.position).normalized * maxRange, Color.red); // Draw a ray to the closest player
            return true;
        }
    }
}
