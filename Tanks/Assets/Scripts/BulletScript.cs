using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float bulletSpeed = 1;
    public int damage = 1;

    public GameObject owner;

    bool destroyed;

	// Use this for initialization
	void Start () 
    {
        Destroy(this.gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
	}

    void Hit(Collider collider)
    {
        if (destroyed) return;

        Debug.Log("Hit: " + collider.name);
        Destroy(this.gameObject);
        destroyed = true;
    }

    public void OnCollisionEnter(Collision collision) 
    {
        Debug.Log("coll");
        Hit(collision.collider);
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.transform.IsChildOf(owner.transform)) return;

        Debug.Log("trigger");
        Hit(c);
    }
}
