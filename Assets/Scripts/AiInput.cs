using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : MonoBehaviour 
{
    public Tank tankScript;
    public Turret turretScript;

	public Transform target;
	public Transform[] targets;

    // Use this for initialization
    void Start ()
	{
		turretScript.reloadSpeed = 5;
	}

	// Update is called once per frame
	void Update () 
    {
		//OLD AI

		// do we have a valid target?
		if (target != null)
		{
			// -- setting the TARGET's direction --
			Vector3 targetDirection = target.position - transform.position;
			targetDirection = Quaternion.Inverse(transform.rotation) * targetDirection;
			// -- setting the angle to turn --
			float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
			if (angle > 180)
			{
				angle -= 360;
			}

			// -- reaching target --
			if ((target.position - transform.position).magnitude < 4)
			{
				target = targets[Random.Range(0, targets.Length)];
			}

			// -- switching the steering from -1 to 1 --
			float x = Mathf.Clamp(angle, -1, 1);
			float y = 1;

			// -- if not looking in the direction of the TARGET --
			if (Mathf.Abs(angle) > 5)
			{
				y = 0;
				turretScript.fireInput = false;
			} else
			{
				turretScript.fireInput = true;
			}

			tankScript.steering = x;
			tankScript.gas = y;

			turretScript.targetValid = true;
			turretScript.targetPos = target.position;
		} else
		{
			target = targets[Random.Range(0, targets.Length)];
		}

		//ToDo: STOP PUSHING WALLS! detect if we hit a wall or obstacle and do something about it
	}
}
