using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : MonoBehaviour {

    public Tank tankScript;
    public Turret turretScript;

    public Transform target;
    public Transform[] targets;

    //public float reloadSpeed;
    //public float reloadTimer = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        // -- setting the TARGET's direction --
        Vector3 targetDirection = target.position - transform.position;
        targetDirection = Quaternion.Inverse(transform.rotation)*targetDirection;
        // -- setting the angle to turn --
        float angle = Mathf.Atan2(targetDirection.x , targetDirection.z) * Mathf.Rad2Deg;
        if(angle > 180) 
        {
            angle -= 360;
        }

        // -- reaching target --
        if((target.position - transform.position).magnitude < 4) 
        {
            target = targets[Random.Range(0,targets.Length)];
        }

        // -- switching the steering from -1 to 1 --
        float x = Mathf.Clamp(angle,-1,1);
        float y = 1;

        // -- if not looking in the direction of the TARGET --
        if (Mathf.Abs(angle) > 5) 
        {
            y = 0;
            turretScript.fireInput = false;
        }
        else
        {
            turretScript.fireInput = true;
        }

        tankScript.steering = x;
        tankScript.gas = y;


        turretScript.targetValid = true;
        turretScript.targetPos = target.position;
        turretScript.reloadSpeed = 5;
    }
}
