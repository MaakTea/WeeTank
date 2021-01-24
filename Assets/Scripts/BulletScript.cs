using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float bulletSpeed = 1;
    public int damage = 10;
    public int bounces = 1;
        public bool bounced = false;
        public float bounceTimer = 0f;

    public GameObject owner;
    public Object explosion;

    bool destroyed;

	// Use this for initialization
	void Start () 
    {
        Destroy(this.gameObject, 10);
        Debug.Log("Shot from: " + owner.name);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (bounced) 
        { 
            bounceTimer -= Time.fixedDeltaTime;
        }
        transform.position += transform.forward * bulletSpeed * Time.fixedDeltaTime;
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            transform.rotation = Quaternion.Inverse(transform.rotation);
        }
	}

    void Hit(Collider collider, Collision collision)
    {
        if (destroyed) return;
        //Debug.Log(bounceTimer);
        //Debug.Log("Bounces: " + bounces);
        if (bounceTimer > 0) return;

        Debug.Log("Hit: " + collider.name);
        if (collider.name != "hitbox" && bounces >= 1 && collision != null)
        {
            //Debug.Log("Bounced");
            bounceTimer = 0.25f;
            Bounced(collision.contacts[0].normal);
            bounced = true;
            bounces--;
            return;
        }

        Health h = collider.GetComponentInParent<Health>();
        //h = collision.collider.GetComponentInParent<Health>();
        if (h != null) h.OnHit(this);

        Destroy(this.gameObject);
        destroyed = true;
        GameObject explode = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explode, 3);
    }

    public void OnCollisionEnter(Collision collision) 
    {
        //Debug.Log("coll");
        Hit(collision.collider, collision);
    }

    public void OnTriggerEnter(Collider c)
    {
		if (owner != null && c.transform.IsChildOf(owner.transform)) return;

        //Debug.Log("trigger");
        Hit(c, null);
    }

    public void Bounced(Vector3 wallNormal) 
    {
        //transform.position -= (transform.forward*2) * bulletSpeed * Time.deltaTime;
        //transform.rotation = Quaternion.Euler(0, transform.rotation.y-180, 0);
        //transform.rotation = Quaternion.Inverse(transform.rotation);
        Vector3 newForward = Vector3.Reflect(transform.forward, wallNormal);
        transform.rotation = Quaternion.LookRotation(newForward);
    }
}
