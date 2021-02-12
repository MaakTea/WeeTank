using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour 
{
    public static Manager singleton;

    public List<GameObject> players;

    public int respawns = 0;

    public List<Transform> respawnPoints;
    public List<GameObject> tanks;

    public bool autoPlay;

    public bool loading;

    void Awake()
    {
        if (singleton != null)
        {
            Destroy(this);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(this.gameObject);

        if (UIManager.singleton)
            UIManager.singleton.Fade(0.0f);
    }

	// Use this for initialization
	void Start () 
    {
        Debug.Log("Start", this);
        Time.timeScale = 1;
        
        if (!loading)
            if (UIManager.singleton)
                UIManager.singleton.Fade(0.0f);


        tanks.Clear();
        players.Clear();

        Tank[] tank = FindObjectsOfType<Tank>();
        foreach(Tank t in tank)
        {
            Debug.Log("tank ", t);
            tanks.Add(t.gameObject);
        }

        //Transform[] rs = FindObjectsOfType<Transform>();
        //foreach(Transform t in rs) 
        //{
        //if (t.parent.tag == "Respawn") respawnPoints.Add(t);
        //}

        PlayerInput[] p = FindObjectsOfType<PlayerInput>();
        foreach(PlayerInput i in p) 
        {
            players.Add(i.gameObject);
        }
        autoPlay = players.Count > 0 ? false : true;

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
        if (loading) return;

        tanks.RemoveAll(t => t == null);
        players.RemoveAll(t => t == null);
        
		// -- our tank got destroyed
        // -- Restart the level
        if (players.Count == 0 && !autoPlay) 
        {
            UIManager.singleton.EnableText(UIManager.singleton.lostText);
            Time.timeScale = 0.5f;
            StartCoroutine(Wait(3, Application.loadedLevel));
			return;
        }
        
		// -- destroyed enemy tanks
        // -- Start the next level
        if(tanks.Count <= 1 && !autoPlay) 
        {
            UIManager.singleton.EnableText(UIManager.singleton.victoryText);
            Time.timeScale = 0.5f;
            StartCoroutine(Wait(3, Application.loadedLevel+1));
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
        loading = true;

        //yield return new WaitForSecondsRealtime(seconds);
        for(float f=0; f<=1.0f; f+=0.1f/seconds)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            UIManager.singleton.Fade(f);
        }

        if (levelIndex != Application.levelCount) 
            Application.LoadLevel(levelIndex);
        else
            Application.LoadLevel(0);

        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        Start();

        for (float f = 0; f <= 1.0f; f += 0.1f / seconds)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            UIManager.singleton.Fade(1.0f-f);
        }

        loading = false;
    }

    public static void LoadLevel(int levelindex)
    {
        //Application.LoadLevel(levelindex);
        singleton.StartCoroutine(singleton.Wait(2, levelindex));
    }
}
