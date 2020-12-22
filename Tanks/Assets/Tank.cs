using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    [Range(-1,1)]
    public float steering;
    [Range(-1,1)]
    public float gas;

    public float moveSpeed = 1;
    public float turnSpeed = 1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = steering;
        float y = gas;

        transform.rotation *= Quaternion.Euler(0, 0, -x         * turnSpeed * Time.deltaTime);
        //Vector3 rot = new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), 0); //player.rotation.eulerAngles.z
        transform.position += transform.up * y                  * moveSpeed * Time.deltaTime;


    }
}
