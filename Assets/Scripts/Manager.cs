using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour 
{
    public GameObject player;

    public int respawns = 0;

    public List<Transform> respawnPoints;
    public List<GameObject> tanks;

    public UIManager uiManager;

	// Use this for initialization
	void Start () 
    {
        Time.timeScale = 1;

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
        // -- our tank got destroyed
        // -- Restart the level
        if (player == null) 
        {
            uiManager.EnableText(uiManager.lostText);
            Time.timeScale = 0.5f;
            StartCoroutine(Wait(5));
        }
        // -- destroyed enemy tanks
        // -- Start the next level
        if(tanks.Count <= 1) 
        {
            uiManager.EnableText(uiManager.victoryText);
            Time.timeScale = 0.5f;
            StartCoroutine(Wait(5));
        }

        if (Input.GetKeyDown(KeyCode.V)) uiManager.EnableText(uiManager.victoryText);
	}
    public void Respawn(GameObject item)
    {
        Destroy(item);
        Instantiate(item);
    }

    IEnumerator Wait(int seconds) 
    {
        yield return new WaitForSecondsRealtime(seconds);

        Application.LoadLevel(Application.loadedLevel);
    }
}
