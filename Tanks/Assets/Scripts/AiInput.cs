using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : MonoBehaviour {

    public Tank tankScript;
    public Turret turretScript;

    public Transform target;
    public Transform[] targets;

    public float firetimer = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetDirection = target.position - transform.position;
        targetDirection = Quaternion.Inverse(transform.rotation)*targetDirection;
        float angle = Mathf.Atan2(targetDirection.x , targetDirection.z) * Mathf.Rad2Deg;
        if(angle > 180) 
        {
            angle -= 360;
        }

        if((target.position - transform.position).magnitude < 4) 
        {
            target = targets[Random.Range(0,targets.Length)];
        }

        float x = Mathf.Clamp(angle,-1,1);
        float y = 1;

        if (Mathf.Abs(angle) > 5) 
        {
            y = 0;
        }

        tankScript.steering = x;
        tankScript.gas = y;

        //if (false)
        {
            turretScript.targetValid = true;
            turretScript.targetPos = target.position;
        }

        if(firetimer > 0) 
        {
            firetimer -= Time.deltaTime;
        }

        if (firetimer <= 0)
        {
            turretScript.fireInput = true;
            firetimer = 5;
        }
        else 
        {
            turretScript.fireInput = false;
        }
    }
}
