using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    //public Manager manager;

    public int baseHealth = 100;
    public int currentHealth;

	public GameObject onDeathPrefab;

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
            Destroy(this.gameObject);
			if (onDeathPrefab != null)
			{
				Instantiate(onDeathPrefab, this.transform.position, this.transform.rotation);
			}
        }
    }
}
