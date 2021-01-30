using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour 
{
    public GameObject player;
    public Health healthScript;

    public int respawns = 0;

    public List<Transform> respawnPoints;
    public List<GameObject> tanks;

    public UIManager uiManager;

    public bool autoPlay;

	// Use this for initialization
	void Start () 
    {
        Time.timeScale = 1;

        Tank[] tank = FindObjectsOfType<Tank>();
        foreach(Tank t in tank)
        {
            tanks.Add(t.gameObject);
        }

        //Transform[] rs = FindObjectsOfType<Transform>();
        //foreach(Transform t in rs) 
        //{
        //if (t.parent.tag == "Respawn") respawnPoints.Add(t);
        //}

        if (autoPlay) 
        {
            foreach(GameObject t in tanks) 
            {
                
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        tanks.RemoveAll(t => t == null);
        
		// -- our tank got destroyed
        // -- Restart the level
        if (player == null && !autoPlay) 
        {
            uiManager.EnableText(uiManager.lostText);
            Time.timeScale = 0.5f;
            StartCoroutine(Wait(5, Application.loadedLevel));
			return;
        }
        
		// -- destroyed enemy tanks
        // -- Start the next level
        if(tanks.Count <= 1 && !autoPlay) 
        {
            uiManager.EnableText(uiManager.victoryText);
            Time.timeScale = 0.5f;
            StartCoroutine(Wait(5, Application.loadedLevel+1));
			return;
        }
        if (autoPlay) 
        {
            if (tanks.Count <= 1) 
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        if (autoPlay) 
        { 
            
        }
	}

    public void Spawn(GameObject item, Transform pos)
    {
        Instantiate(item, pos);
    }

    IEnumerator Wait(int seconds, int levelIndex) 
    {
        yield return new WaitForSecondsRealtime(seconds);
        if (levelIndex != Application.levelCount) Application.LoadLevel(levelIndex);
        else Application.LoadLevel(0);
    }
}
