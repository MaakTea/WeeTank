using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiInputNav : MonoBehaviour 
{
    public Tank tankScript;
    public Turret turretScript;
	public NavMeshAgent agent;

	public Transform target;
	public Transform[] targets;

	public Vector3 moveTarget;

    // Use this for initialization
    void Start ()
	{
		agent.updatePosition = false;
		agent.updateRotation = false;
		agent.updateUpAxis = false;

		moveTarget = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () 
    {
		agent.nextPosition = transform.position;

		// do we have a valid target?
		if (target == null)
		{
			agent.ResetPath();
			target = targets[Random.Range(0, targets.Length)];
			tankScript.steering = 0;
			tankScript.gas = 0;
			return;
		}

		//are we there yet?
		if ((target.position - transform.position).magnitude < 2)
		{
			agent.ResetPath();
			target = targets[Random.Range(0, targets.Length)];
			tankScript.steering = 0;
			tankScript.gas = 0;
			return;
		}

		//Debug.DrawLine(transform.position, moveTarget, Color.green);

		if (!agent.hasPath)
		{//set target
			agent.SetDestination(target.position);
			return;
		} else
		if (agent.pathPending)
		{//wait
			tankScript.steering = 0;
			tankScript.gas = 0;
			moveTarget = transform.position;
			return;
		} else
		{//path is ready
		 //NavMeshHit hit;
		 //if (agent.SamplePathPosition(NavMesh.AllAreas, 2.0F, out hit))
		 //{
		 //	moveTarget = hit.position;
		 //}
			moveTarget = agent.path.corners[1];

			//moveTarget = transform.position + agent.desiredVelocity;
		}
		moveTarget = transform.position + agent.desiredVelocity;

		Vector3 targetDirection = moveTarget - transform.position;

			targetDirection = Quaternion.Inverse(transform.rotation) * targetDirection;
			// -- setting the angle to turn --
			float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
			if (angle > 180)
			{
				angle -= 360;
			}
			tankScript.steering = Mathf.Clamp(angle*0.1f, -1, 1);
			tankScript.gas = Mathf.Abs(tankScript.steering) > 0.5f ? 0.25f : 1;
	}
}
