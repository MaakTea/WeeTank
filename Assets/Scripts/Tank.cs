using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour 
{
    [Range(-1,1)]
    public float steering;
    [Range(-1,1)]
    public float gas;

    public float moveSpeed = 5;
	public float reverseSpeed = 3;
	public float turnSpeed = 60;

	public int team;

    public Transform hitbox;

	public AudioSource audio_engine;
	public AudioSource audio_track;

	public float currentThrottle;
	public float currentTurn;

	// Use this for initialization
	void Start ()
	{
		
	}

	private void FixedUpdate()
	{
		currentThrottle = Mathf.MoveTowards(currentThrottle, gas, Time.fixedDeltaTime / 0.2f);
		currentTurn = Mathf.MoveTowards(currentTurn, steering, Time.fixedDeltaTime / 0.2f);
	}


	// Update is called once per frame
	void Update ()
	{
		float x = currentTurn;// steering;
		float y = currentThrottle;// gas;

        transform.rotation *= Quaternion.Euler(0, x * turnSpeed * Time.deltaTime, 0);
		//Vector3 rot = new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), 0); //player.rotation.eulerAngles.z

		float speed = y > 0 ? moveSpeed : reverseSpeed;
		transform.position += transform.forward * y * speed * Time.deltaTime;

		UpdateAudio();
    }

	void UpdateAudio()
	{
		if (audio_engine)
		{
			if (!audio_engine.isPlaying)
				audio_engine.Play();
			float f = 0.5f + Mathf.Clamp01(Mathf.Abs(currentThrottle) + Mathf.Abs(currentTurn));
			audio_engine.volume = Mathf.Lerp(audio_engine.volume, f, Time.deltaTime / 0.2f);
			audio_engine.pitch = Mathf.Lerp(audio_engine.pitch, f, Time.deltaTime / 0.2f);
		}
		if (audio_track)
		{
			if (!audio_track.isPlaying)
				audio_track.Play();
			float f = Mathf.Abs(currentThrottle);// + Mathf.Abs(currentTurn);
			audio_track.volume = Mathf.Lerp(audio_track.volume, Mathf.Lerp(0.0f, 0.5f, f), Time.deltaTime / 0.2f);
			//audio_track.pitch = Mathf.Lerp(audio_track.pitch, f, Time.deltaTime / 0.2f);
		}
	}
}
