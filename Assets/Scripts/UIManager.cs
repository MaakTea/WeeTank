using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public static UIManager singleton;


    public Text victoryText;
    public Text lostText;

    public RawImage fadeScreen;

	// Use this for initialization
	void Awake() 
    {
        //if (singleton != null)
        //{
        //    Debug.LogWarning("Multiple UIManagers!", this);
        //    Destroy(this);
        //}

        singleton = this;
	}
	
	// Update is called once per frame
	void Update () 
    {

	}
    public void EnableText(Text text) 
    {
        text.gameObject.SetActive(true);
    }

    public void Fade(float f)
    {
        if (fadeScreen)
            fadeScreen.color = new Color(0, 0, 0, f);
    }

    public void LoadLevel(int levelindex)
    {
        Manager.LoadLevel(levelindex);
    }

}
