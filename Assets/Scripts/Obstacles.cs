using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject[] obstaclesToRemove;

    public AudioSource explodeSound;

    private bool exploded = false;

    public void Explode()
    {
        if (!exploded)
        {
            exploded = true;
            explosion.Play();
            explodeSound.Play();
            foreach (GameObject obstacle in obstaclesToRemove)
            {
                Destroy(obstacle);
            }
        }
        
    }
}
