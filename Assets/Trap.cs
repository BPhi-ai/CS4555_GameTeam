using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public long damageValue = 20;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            try
            {
                PlayerUtilityController otherController = other.GetComponent<PlayerUtilityController>();
                otherController.DamageSphereRobot(damageValue);
            }
            catch (System.Exception e)
            {
                print("Cannot find the Robot Sphere script");
            }

        }
    }
}
