using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour 
{
    public GameObject player;

    public int respawns = 0;

    public List<Transform> respawnPoints;
    public List<GameObject> tanks;

	// Use this for initialization
	void Start () 
    {
        Tank[] tank = FindObjectsOfType<Tank>();
        foreach(Tank t in tank) 
        {
            tanks.Add(t.gameObject);
        }
        Transform[] rs = FindObjectsOfType<Transform>();
        foreach(Transform t in rs) 
        {
            //if (t.parent.tag == "Respawn") respawnPoints.Add(t);
        }
	}

    // Update is called once per frame
    void Update()
    {
        tanks.RemoveAll(t => t == null);
        if (player == null) 
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        if(tanks.Count <= 1) 
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
    public void Respawn(GameObject item)
    {
        Destroy(item);
        Instantiate(item);
    }
}
