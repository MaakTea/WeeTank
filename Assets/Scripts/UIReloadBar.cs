using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReloadBar : MonoBehaviour {

    public Slider slider;
    public Turret turret;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        slider.value = turret.reloadTimer;
	}
}
