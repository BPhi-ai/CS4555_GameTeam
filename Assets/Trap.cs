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
            

            if (other.name == "robotSphere")
            {
                try
                {
                    print("Touching Sphere");
                    Entity entity = other.GetComponent<Entity>();

                    entity.DamageEntity(damageValue);
                }
                catch (System.Exception e)
                {
                    print("Cannot find the Robot Sphere script\n");
                    print(e);
                }
            }
            else if (other.name == "PBRCharacter")
            {
                try
                {
                    print("Touching Robot");
                    Entity entity = other.GetComponent<Entity>();

                    entity.DamageEntity(damageValue);
                }
                catch (System.Exception e)
                {
                    print("Cannot find the Shooter Robot Entity Script\n");
                    print(e);
                }
            }
        }
    }
}
