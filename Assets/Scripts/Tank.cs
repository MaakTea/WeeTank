using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour 
{
    [Range(-1,1)]
    public float steering;
    [Range(-1,1)]
    public float gas;

    public float moveSpeed = 1;
    public float turnSpeed = 1;
	public int team;

    public Transform hitbox;

	public AudioSource audio_engine;
	public AudioSource audio_track;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        float x = steering;
        float y = gas;

        transform.rotation *= Quaternion.Euler(0, x         * turnSpeed * Time.deltaTime, 0);
		//Vector3 rot = new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad), 0); //player.rotation.eulerAngles.z

		transform.position += transform.forward * y         * moveSpeed * Time.deltaTime;

		UpdateAudio();
    }

	void UpdateAudio()
	{
		if (audio_engine)
		{
			if (!audio_engine.isPlaying)
				audio_engine.Play();
			float f = 0.5f + Mathf.Clamp01(Mathf.Abs(gas) + Mathf.Abs(steering));
			audio_engine.volume = Mathf.Lerp(audio_engine.volume, f, Time.deltaTime / 0.2f);
			audio_engine.pitch = Mathf.Lerp(audio_engine.pitch, f, Time.deltaTime / 0.2f);
		}
		if (audio_track)
		{
			if (!audio_track.isPlaying)
				audio_track.Play();
			float f = Mathf.Abs(gas);// + Mathf.Abs(steering);
			audio_track.volume = Mathf.Lerp(audio_track.volume, Mathf.Lerp(0.0f, 0.5f, f), Time.deltaTime / 0.2f);
			//audio_track.pitch = Mathf.Lerp(audio_track.pitch, f, Time.deltaTime / 0.2f);
		}
	}
}
