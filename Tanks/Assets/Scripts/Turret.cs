using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
    public GameObject bullet;

    public Transform barrel;

    public bool targetValid;
    public Vector3 targetPos;

    [Range(-1,1)]
    public float rotInput;

    public Vector3 mouseLocation;
    public Quaternion rotateToMouse;

    public float turnSpeed = 1;
    public float fireSpeed = 1;

    public bool fireInput;
    public float fireTimer;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (targetValid)
        {
            Debug.DrawLine(transform.position, targetPos, Color.black);

            Vector3 euler = Quaternion.LookRotation(targetPos - transform.position).eulerAngles;
            //transform.rotation = Quaternion.Euler(new Vector3(0, euler.y, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, euler.y, 0)), turnSpeed*Time.deltaTime);
            //barrel.localRotation = Quaternion.Euler(new Vector3(euler.x, 0, 0));
        }

        //transform.rotation *= Quaternion.Euler(0, 0, -rotInput * turnSpeed * Time.deltaTime);
        //transform.rotation *= Quaternion.RotateTowards(transform.rotation, rotateToMouse, turnSpeed);

        //Debug.DrawRay(transform.position, mouseLocation*5, Color.red);

        if(fireTimer > 0) 
        {
            fireTimer -= Time.deltaTime;
        }
        else 
        {
            if (fireInput)
            {
                Debug.DrawRay(transform.position, targetPos - transform.position, Color.red, 1.5f);
                fireTimer = 1;

                GameObject go = Instantiate(bullet, barrel.position, barrel.rotation);
                BulletScript bs = go.GetComponent<BulletScript>();
                bs.owner = transform.parent.gameObject;
            }
        }
	}
}
