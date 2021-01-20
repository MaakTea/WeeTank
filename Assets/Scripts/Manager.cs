using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public Transform player;
    public Transform turret;
    public Transform[] enemies;
    public Transform[] points;
    public Transform enemyTarget;

    public bool stable = false;
    public float stableTimer = 0;
    //public bool shooting = false;
    //public float shootTimer = 0;

    public float Movespeed;

    Vector3 targetDirection;



    // Use this for initialization
    void Start () {
        stable = true;
        for (int i = 0; i < enemies.Length; i++)
        {
            //enemies[i].position = points[1].position;
        }
        enemyTarget = points[1];
    }
	
	// Update is called once per frame
	void Update () 
    {
        #region Player
        /*float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        player.rotation *= Quaternion.Euler(0, 0, -x);
        Vector3 rot = new Vector3(Mathf.Cos((player.rotation.eulerAngles.z + 90)*Mathf.Deg2Rad), Mathf.Sin((player.rotation.eulerAngles.z+90)*Mathf.Deg2Rad), 0); //player.rotation.eulerAngles.z
        player.position += rot * y * Movespeed * Time.deltaTime;

        float mouseX = Input.GetAxisRaw("Mouse X");

        turret.rotation *= Quaternion.Euler(0, 0, -mouseX);
        if (Input.GetKey(KeyCode.Z)) 
        {
            stableTimer += 1*Time.deltaTime;

            if (stable == false && stableTimer < 2)
            {
                stableTimer = 2;
                stable = true;
            }
            if (stable == true && stableTimer < 2)
            {
                stableTimer = 2;
                stable = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z)) 
        {
            stableTimer = 0;
        }

        if (stable) 
        {
            turret.rotation *= Quaternion.Euler(0, 0, -mouseX + x);
        }


        Vector3 turretRot = new Vector3(Mathf.Cos((turret.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((turret.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), 0);

        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.DrawRay(turret.position, turretRot * 15,Color.red, 0.5f);
        }

        //Debug.DrawRay(player.position ,rot*5);
        if(Input.GetKeyDown(KeyCode.Z)) { Debug.Log("Stablalizing"); }*/
        #endregion
        #region Enemies
        

        if (enemies[0].position == points[1].position)
        {
            Rotate(points[0], enemies[0]);
        }
        if (enemies[0].position == points[0].position)
        {
            Rotate(points[1], enemies[0]);
        }

        #endregion
    }

    void Rotate(Transform target, Transform subject) 
    {
        /*
        // Determine which direction to rotate towards
        targetDirection = point.position - subject.position;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(subject.up, targetDirection, turnSpeed, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(subject.position, newDirection*5, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        subject.rotation *= Quaternion.LookRotation(newDirection);
        */

        targetDirection = (target.position - subject.position).normalized;
        float targetAngle = Mathf.Acos(targetDirection.x);
        float angleDif = targetAngle - (subject.rotation.eulerAngles.z+90);
        if(angleDif > 180) 
        {
            angleDif -= 180;
        }
        if(angleDif < -180) 
        {
            angleDif += 180;
        }
        subject.rotation *= Quaternion.Euler(0, 0, Mathf.Lerp(0, angleDif, 2 * Time.deltaTime));
        //Mathf.Lerp(0, angleDif, 4 * Time.deltaTime);

        Debug.DrawRay(subject.position, targetDirection * 5, Color.red);
    }

    void MoveToTarget(Transform target, Transform subject)
    {
        targetDirection = (target.position - subject.position).normalized;
        subject.position += subject.rotation * targetDirection * Time.deltaTime * Movespeed;
    }
}
