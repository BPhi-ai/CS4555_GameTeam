using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum States {WANDER, STUNNED, CHASE}
    public States state = States.WANDER;

    public float maxRange = 10000;
    public float speed = 2.5f;
    private GameObject closestPlayer = null;
    public ParticleSystem lightning;
    public ParticleSystem muzzleFlash;

    public GameObject bullet;
    public Transform firePoint;

    RaycastHit hit;

    private float timeToWander = 0.0f;
    private float timeToShoot = 0.0f;

    private Vector3 startPosition;
    public float wanderOffset = 10.0f; // Wandering radius
    public Vector3 randomPosition; // this is the random position to wander to

    private float timeStunned = 0.0f;
    public float timeLightningInterval = 2.0f;
    private float timePassed = 0.0f; // Used for tracking when to play lightning particle

    private Animator animator;        // Reference to the Animator component

    private void Start()
    {
        startPosition = transform.position;
        randomPosition = startPosition;

        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
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

                    // rotate the enemy to look at the random position
                    Vector3 direction = randomPosition - transform.position;
                    direction.y = 0;

                    transform.rotation = Quaternion.LookRotation(direction);
                }

                // Move the enemy to the random position at all times
                transform.position = Vector3.MoveTowards(transform.position, randomPosition, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, randomPosition) > 0.1f)
                {
                    animator.SetFloat("Y", 1);
                    animator.SetBool("Aiming", true);
                }
                else
                {
                    animator.SetFloat("Y", 0);
                    animator.SetBool("Aiming", false);
                }
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

                animator.SetFloat("Y", 0);
                animator.SetBool("Aiming", false);

                break;
            case States.CHASE:
                if (!SeesPlayer())
                {
                    state = States.WANDER;
                    print("Can't see the player anymore!");
                    break;
                }
                Vector3 chaseDirection = closestPlayer.transform.position - transform.position;
                chaseDirection.y = 0;

                transform.rotation = Quaternion.LookRotation(chaseDirection);

                animator.SetFloat("Y", 0);
                animator.SetBool("Aiming", true);

                timeToShoot += Time.deltaTime;
                if (timeToShoot >= 1.0f)
                {
                    timeToShoot = 0.0f; // resets the shooting timer
                    Shoot();
                }
                break;
        }
    }

    public void Shoot()
    {
        muzzleFlash.Play();

        GameObject bulletObject = Instantiate(bullet, firePoint.position, transform.rotation);
    }

    public void Stun()
    {
        state = States.STUNNED;
    }

    public bool SeesPlayer()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayerSoFar = null; 
        float closestDistanceSoFar = float.MaxValue;
        float offsetY = 0.6f; // less than 1 now because the utility bot is short
        bool foundPlayer = false;

        foreach (GameObject player in allPlayers)
        {
            Vector3 playerPosition = player.transform.position;
            playerPosition.y += offsetY;

            Vector3 enemyPosition = transform.position;
            enemyPosition.y += offsetY;

            if (Vector3.Distance(transform.position, playerPosition) <= maxRange)
            {
                if (Physics.Raycast(enemyPosition, (playerPosition - enemyPosition), out hit, maxRange))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
                        if (distanceToPlayer < closestDistanceSoFar)
                        {
                            foundPlayer = true;
                            closestPlayerSoFar = hit.collider.gameObject;
                            closestDistanceSoFar = distanceToPlayer;
                        }
                    }
                }
            }
        }

        if (foundPlayer)
        {
            closestPlayer = closestPlayerSoFar;
            Debug.DrawRay(transform.position, (closestPlayer.transform.position - transform.position).normalized * maxRange, Color.red);
            return true;
        }
        else
        {
            closestPlayer = null;
            return false;
        }
    }
}
