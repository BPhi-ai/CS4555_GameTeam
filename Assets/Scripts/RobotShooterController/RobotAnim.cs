using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnim : MonoBehaviour
{
    // Change the animation for Utility Robot
    Vector3 rot = Vector3.zero;
    float rotSpeed = 40f;

    Animator anim;
    [SerializeField] private PlayerUtilityController player;

    // Use this for initialization
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
    }

    void CheckKey()
    {
        // Walk Animation (Messy)
        if ((Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.J) ||
            Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L)) && player.GetIsGrounded())
        {
            anim.SetBool("Walk_Anim", true);
        }
        else if (!player.GetIsGrounded())
        {
            anim.SetBool("Walk_Anim", false);
        }
        else if (Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.J) ||
                 Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.L) || player.GetIsGrounded())
        {
            anim.SetBool("Walk_Anim", false);
        }

        /*
        // Rotate Left
        if (Input.GetKey(KeyCode.J))
        {
            rot[1] -= rotSpeed * Time.fixedDeltaTime;
        }

        // Rotate Right
        if (Input.GetKey(KeyCode.L))
        {
            rot[1] += rotSpeed * Time.fixedDeltaTime;
        }
        */

        /*
		// Roll
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (anim.GetBool("Roll_Anim"))
			{
				anim.SetBool("Roll_Anim", false);
			}
			else
			{
				anim.SetBool("Roll_Anim", true);
			}
		}*/

        // Close
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
            else
            {
                anim.SetBool("Open_Anim", false);
            }
        }
    }
}
