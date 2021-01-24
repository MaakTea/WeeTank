using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text victoryText;
    public Text lostText;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {

	}
    public void EnableText(Text text) 
    {
        text.gameObject.SetActive(true);
    }
}
