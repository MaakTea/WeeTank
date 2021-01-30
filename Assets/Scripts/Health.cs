using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    //public Manager manager;

    public int baseHealth = 100;
    public int currentHealth;

	public GameObject onDeathPrefab;
	public GameObject[] leaveBehind;

	// Use this for initialization
	void Start () 
    {
        currentHealth = baseHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void OnHit(BulletScript bullet)
    {
        currentHealth -= bullet.damage;
        if (currentHealth <= 0) 
        {
			foreach (GameObject go in leaveBehind)
				if (go != null)
					go.transform.SetParent(this.gameObject.transform.parent, true);

			Destroy(this.gameObject);

			if (onDeathPrefab != null)
			{
				Instantiate(onDeathPrefab, this.transform.position, this.transform.rotation);
			}
        }
    }
}
