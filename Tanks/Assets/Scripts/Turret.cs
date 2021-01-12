﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;

    public Transform barrel;

    public bool targetValid;
    public Vector3 targetPos;

    [Range(-1, 1)]
    public float rotInput;

    public Vector3 mouseLocation;
    public Quaternion rotateToMouse;

    public float turnSpeed = 1;
    //public float fireSpeed = 1;

    public bool fireInput;
    public float reloadSpeed;
    public float reloadTimer;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (targetValid)
        {
            // -- black line from US to the TARGET --
            Debug.DrawLine(transform.position, targetPos, Color.black);

            // -- setting the euler angle to rotate to --
            Vector3 euler = Quaternion.LookRotation(targetPos - transform.position).eulerAngles;
            //transform.rotation = Quaternion.Euler(new Vector3(0, euler.y, 0));
            // -- rotating towards the euler angle --
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, euler.y, 0)), turnSpeed * Time.deltaTime);
            //barrel.localRotation = Quaternion.Euler(new Vector3(euler.x, 0, 0));
        }

        //transform.rotation *= Quaternion.Euler(0, 0, -rotInput * turnSpeed * Time.deltaTime);
        //transform.rotation *= Quaternion.RotateTowards(transform.rotation, rotateToMouse, turnSpeed);

        //Debug.DrawRay(transform.position, mouseLocation*5, Color.red);

        // -- Fireing --
        if (reloadTimer <= 0)
        {
            if (fireInput)
            {
                Shoot();
                Reload();
            }
        }
        else
        {
            reloadTimer -= Time.deltaTime;
        }
    }

    void Reload()
    {
        reloadTimer = reloadSpeed;
    }

    void Shoot() 
    {
        Debug.DrawRay(transform.position, targetPos - transform.position, Color.red, 1.5f);

        // -- Creating the bullet, where, and it's rotation --
        GameObject go = Instantiate(bullet, barrel.position, barrel.rotation);
        BulletScript bs = go.GetComponent<BulletScript>();
        // -- setting the owner --
        bs.owner = transform.parent.gameObject;
    }
}
