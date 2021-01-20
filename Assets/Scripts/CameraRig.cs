using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour 
{
    public Transform target;
    public float positionStrength = 0.1f;
    public float rotationStrength = 0;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (target!=null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, positionStrength);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationStrength);       //Slurm =o]
        }
	}
}
