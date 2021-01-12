using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public Tank tankScript;
    public Turret turretScript;

    public Camera mouseCam;

    public bool hitValid;
    public RaycastHit hit;


    // Use this for initialization
    void Start () 
    {
        if (mouseCam == null)
            mouseCam = Camera.main;
    }

    // Update is called once per frame
    void Update () {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxisRaw("Mouse X");

        Vector3 mousePos = Input.mousePosition;
        Quaternion toMouseDir = Quaternion.Euler(transform.position-mousePos);

        tankScript.steering = x;
        tankScript.gas = y;

        turretScript.rotInput = mouseX;
        turretScript.mouseLocation = mousePos;
        //turretScript.rotateToMouse = toMouseDir;

        Ray ray = mouseCam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * mouseCam.farClipPlane, Color.gray);

        hitValid = Physics.Raycast(ray, out hit);

        turretScript.targetValid = hitValid;
        turretScript.targetPos = hit.point;
        turretScript.fireInput = Input.GetMouseButton(0);
    }
}
